/******************************************************************************
 * File        : PointerController.cs
 * Version     : 1.0
 * Author      : Severi Kangas (severi.kangas@lapinamk.com), Miika Puljujärvi (miika.puljujarvi@lapinamk.fi)
 * Copyright   : Lapland University of Applied Sciences
 * Licence     : MIT-Licence
 * 
 * Copyright (c) 2021 Lapland University of Applied Sciences
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 * 
 * This script utilises Valve's SteamVR-plugin, which can be downloaded from Unity Asset Store.
 * 
 *****************************************************************************/

using UnityEngine;
using Valve.VR.Extras;

public class PointerController : SteamVR_LaserPointer
{

    public float lineWidthNormal;
    public float lineWidthOnHit;
    public bool showHitOrbOnHit = true;
    private Transform hitOrb;
    public Material hitOrbMat;
    public LayerMask hitOrbLayers;

    // Send information on pointer entering and leaving an element through events. 
    public delegate void OnPointerEnter(Transform _t);
    public static event OnPointerEnter OnEnter;
    public delegate void OnPointerExit(Transform _t);
    public static event OnPointerExit OnExit;
    public delegate void OnPointerClicked (Transform _t);
    public static event OnPointerClicked OnClicked;


    private void Awake ()
    {
        thickness = lineWidthNormal;
        hitLayer = hitOrbLayers;
    }

    public override void OnPointerClick(PointerEventArgs e)
    {
        base.OnPointerClick(e);
        OnClicked?.Invoke(e.target);

        var pointerReader = e.target.gameObject.GetComponent<PointerReader>();
        if (pointerReader != null)
        {
            pointerReader.FireButton();
        }
        
        //Debug.Log(e.distance+" | "+e.flags.ToString()+" | "+e.fromInputSource.ToString()+" | "+e.target);
    }

    public override void OnPointerIn(PointerEventArgs e)
    {
        base.OnPointerIn(e);
        thickness = lineWidthOnHit;
        OnEnter?.Invoke(e.target);
        ShowHitOrb(true, e.target);
    }

    public override void OnPointerOut(PointerEventArgs e)
    {
        base.OnPointerOut(e);
        thickness = lineWidthNormal;
        OnExit?.Invoke(e.target);
        pointer.transform.localScale = new Vector3(thickness, 1f, thickness);
        ShowHitOrb(false, e.target);
    }

    void CreateHitOrb ()
    {
        hitOrb = GameObject.CreatePrimitive(PrimitiveType.Sphere).transform;
        hitOrb.localScale = Vector3.one * 0.0075f;
        hitOrb.GetComponent<MeshRenderer>().material = hitOrbMat;
        hitOrb.GetComponent<SphereCollider>().enabled = false;
    }

    void ShowHitOrb (bool _show, Transform _hitTarget)
    {
        if (!showHitOrbOnHit) return;
        if (hitOrbLayers != (hitOrbLayers | 1 << (_hitTarget.gameObject.layer))) return;

        if (null == hitOrb) CreateHitOrb();

        hitOrb.gameObject.SetActive(_show);
    }


    public override void PointerHit (RaycastHit _hit)
    {
        base.PointerHit(_hit);

        if (!showHitOrbOnHit) return;
        if (hitOrbLayers != (hitOrbLayers | 1<< _hit.transform.gameObject.layer)) return;       // Check if _hit layer is one of hitOrbLayers.

        if(hitOrb == null) CreateHitOrb();
        hitOrb.position = _hit.point;
    }


}


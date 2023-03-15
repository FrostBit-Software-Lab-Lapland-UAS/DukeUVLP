using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Keep two lists of objects that should be moved out of an area defined by a Collider (trigger) attached to this object.
/// First list (Items) defines the objects that are followed, second list (Inside) is the current IN / OUT state of the objects.
/// </summary>
public class ClearAreaBounds : MonoBehaviour
{

    // --- --- --- Variables --- --- ---

    public List<Transform> clearableItems = new List<Transform>();
    public List<Transform> clearableItemsInside;
    public bool areaIsClear = false;

    public delegate void AreaCleared (bool _isClear);
    public static event AreaCleared AreaIsCleared;




    // --- --- --- Methods --- --- ---

    // --- MonoBehaviour ---

    private void Start ()
    {
        clearableItemsInside = new List<Transform>();
    }

    /// <summary>
    /// Add object to (Inside) if it is part of the (Items).
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter (Collider other)
    {
        if (clearableItems.Contains(other.transform)) {
            if (areaIsClear) ToggleAreaClearFlag();
            if(!clearableItemsInside.Contains(other.transform)) {
                clearableItemsInside.Add(other.transform);
            }
        }
    }

    /// <summary>
    /// Remove object from (Inside) if it is part of the (Items).
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit (Collider other)
    {
        if(clearableItems.Contains(other.transform)) {
            if (clearableItemsInside.Contains(other.transform)) {
                clearableItemsInside.Remove(other.transform);
            }
            if (CheckIfAreaIsClear()) {
                ToggleAreaClearFlag();
            }
        }
    }

    /// <summary>
    /// Called whenever an object found on (Items). Checks if it was the last object to leave the area.
    /// </summary>
    /// <returns></returns>
    private bool CheckIfAreaIsClear ()
    {
        return clearableItemsInside.Count == 0 ? true : false;
    }

    /// <summary>
    /// Set the flag for cleared and send information on the change through an event. 
    /// </summary>
    private void ToggleAreaClearFlag ()
    {
        areaIsClear = !areaIsClear;
        AreaIsCleared?.Invoke(areaIsClear);
    }



}

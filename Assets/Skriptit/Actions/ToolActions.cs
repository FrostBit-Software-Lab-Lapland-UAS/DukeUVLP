using UnityEngine;
using Valve.VR;

public class ToolActions : MonoBehaviour
{
    public GameObject toolObject;
    public GameObject targetObject;

    SteamVR_ActionSet steamVRAction;
    SteamVR_Action_Boolean vrActionBoolean;
    SteamVR_Action_Boolean vrActionBooleanSecond;

    bool controllerTrigger;
    bool controllerGrip;

    void Awake() {
        vrActionBoolean = SteamVR_Actions._default.GrabGrip;
        vrActionBooleanSecond = SteamVR_Actions._default.GrabPinch;
    }

    void Start() {

        controllerTrigger = SteamVR_Actions._default.GrabPinch[SteamVR_Input_Sources.Any].state;
        controllerGrip = SteamVR_Actions._default.GrabGrip[SteamVR_Input_Sources.Any].state;
        steamVRAction.Activate(SteamVR_Input_Sources.Any, 0, true);

    }

    void OnTriggerEnter(Collider other) {
        
        if(other.gameObject.tag == "Tool" && controllerGrip) {

            if(controllerTrigger){

                targetObject.transform.parent = toolObject.transform;

            }

        }

    }

}

using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
 
public class VR_UI_Handler : MonoBehaviour
{
    [SerializeField] GameObject TaskMenu;
    [SerializeField] GameObject VRPausemenu;
    [SerializeField] SteamVR_Input_Sources rightHand;
    [SerializeField] SteamVR_Input_Sources leftHand;

    Hand hand;

    bool _options = false;
    bool _tasks = false;

    // private void OnEnable() {
        
    //     if (hand == null) {
    //         hand = this.GetComponent<Hand>();
    //     }

    // } 


    void Update()
    {
        //if options button on right is clicked --> Open task menu
        //if options button on left is clicked --> Open Options menu

        if(SteamVR_Input.GetStateDown("Tasks", rightHand)) {
            _tasks = !_tasks;
        }
        if(_tasks == true){
            TaskMenu.SetActive(true);
        }
        else
            TaskMenu.SetActive(false);



        if(SteamVR_Input.GetStateDown("OptionsPress", leftHand))
        {
        _options = !_options;
        }
        if(_options == true){
            VRPausemenu.SetActive(true);
        }
        else
            VRPausemenu.SetActive(false);

    }

}

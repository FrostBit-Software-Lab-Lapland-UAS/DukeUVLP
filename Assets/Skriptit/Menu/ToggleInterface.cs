using UnityEngine;

public class ToggleInterface : MonoBehaviour
{

    [SerializeField]
    GameObject _interfaceObject;
    
    bool Toggled = false;

    // Update is called once per frame
    void Update()
    {
        if(Toggled == true){
            _interfaceObject.SetActive(true);
        }
        else
            _interfaceObject.SetActive(false);
    }

    public void onClick(){

        Toggled = !Toggled;        

    }
}

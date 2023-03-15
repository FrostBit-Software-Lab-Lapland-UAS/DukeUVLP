using UnityEngine;
using Pressure;

public class OnIndicatorTrigger : MonoBehaviour
{
    public PressureUp _pressureScript;
    public EventController _eventController;
    public TriggerEvents _triggerEvents;
    
    [HideInInspector]
    public bool _rightPressure;

    // PM: testataan
    public ParticleController partController;



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "RightPressure")
        {
            _rightPressure = true;

            _triggerEvents._animatedObjectFive.enabled = false;
            _triggerEvents._animatedObjectSix.enabled = true;
            _eventController.InfoSet[3].SetActive(false);
            _eventController.InfoSet[4].SetActive(true);

            // PM: testiksi piiloon t�m�
            _pressureScript.StopPressureRising();
            // PM: kokeillaan t�t� t�nne
            partController.ToggleParticles(true);
        }
    }

    // PM: t�m� oli kommentoitu, aktivoidaanpa kokeeksi
    // nyt toimii huonosti eli partController ei ehdi k�ynnisty� lainkaan
    private void OnTriggerExit(Collider other)
    { 
    if (other.gameObject.tag == "RightPressure")
        {
            _rightPressure = false;
            partController.ToggleParticles(false);
            //PressureGameobject[1].SetActive(false);
            //PressureGameobject[1].SetActive(false);
        }
    }

}

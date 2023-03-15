using UnityEngine;

public class AirpumpEventTrigger : MonoBehaviour
{

    // Boolean arvo, jotta voidaan tarkastaa onko tarvittavat kohdat toteutuneet seuraavaa eventtiä varten.
    public bool[] _step;


    // Event systemin "trigger" metodit.
    public void StepOne()
    {
        _step[0] = true;
        AirpumpEvents.EventOne();
    }
    public void StepTwo()
    {
        _step[1] = true;
        AirpumpEvents.EventTwo();
    }
    public void StepThree()
    {
        _step[2] = true;
        AirpumpEvents.EventThree();
    }
    public void StepFour()
    {
        _step[3] = true;
        AirpumpEvents.EventFour();
    }
    public void StepFive()
    {
        _step[4] = true;
        AirpumpEvents.EventFive();
    }
    public void StepSix()
    {
        _step[5] = true;
        AirpumpEvents.EventSix();
    }
    public void StepSeven()
    {
        _step[6] = true;
        AirpumpEvents.EventSeven();
    }
    public void StepEight()
    {
        _step[7] = true;
        AirpumpEvents.EventEight();
    }
}

using UnityEngine;

public class PlayAnimations : MonoBehaviour
{
    public AirpumpEventController _airEventController;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Socket"))
        {
            _airEventController._animation[0].SetBool("PlayTubeSocket", false);
        }
    
        if (other.CompareTag("Hose"))
        {
            _airEventController._animation[1].SetBool("PlayHose", false);
        }

        if (other.CompareTag("Tester"))
        {
            _airEventController._animation[1].SetBool("PlayTester", false);
        }
    }

}

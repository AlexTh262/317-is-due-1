using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    public bool flashlightOn;
    public Light flashlight;
    [SerializeField] public GameObject flashLightRef;
    [SerializeField] private AudioSource flashlightSoundSource;
    [SerializeField] private AudioClip flashlightClick;
    void Start()
    {
        flashlight.enabled = false;
        flashlightOn = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) 
        {
            if (!flashlightOn)
            {
                flashlight.enabled = true;
                flashlightOn = true;
                flashlightSoundSource.PlayOneShot(flashlightClick);
            } else
            {
                flashlight.enabled = false;
                flashlightOn = false;
            }
        }
    }
}

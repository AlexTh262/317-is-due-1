using UnityEngine;

public class AmbientSoundManager : MonoBehaviour
{

    [SerializeField] private AudioSource ambienceAudioSource;
    
    void Start()
    {
        ambienceAudioSource.Play();
    }

    void Update()
    {
        
    }


}
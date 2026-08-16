using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

public class EventManager : MonoBehaviour
{

    //Variables for ambient footstep events
    [SerializeField] private AudioSource setFootstepAudioSource;
    [SerializeField] private AudioClip[] setGrassAudioClips = default;
    [SerializeField] private AudioClip[] setWoodAudioClips = default;
    [SerializeField] private Transform setFootstepAudioListenerPos;
    private static AudioSource footstepAudioSource;
    private static AudioClip[] grassAudioClips;
    private static AudioClip[] woodAudioClips;
    private static Transform footstepAudioListenerPos;


    //Invisible wall variables
    private GameObject invisibleWallPathEntrance;

    //Situational flags
    public static bool firstStepsPlayed = false;

    //Variables for running shadow event
    [SerializeField] private Transform start;
    [SerializeField] private Transform end;
    private static Transform startStatic;
    private static Transform endStatic;

    //Variables for incoming bus event
    [SerializeField] public GameObject bus;
    public static GameObject staticBus;
    public static Vector3 incomingBusTarget;

    void Start()
    {
        //Setting static footstep variables
        footstepAudioSource = setFootstepAudioSource;
        grassAudioClips = setGrassAudioClips;
        woodAudioClips = setWoodAudioClips;
        footstepAudioListenerPos = setFootstepAudioListenerPos;

        //Setting up invisible wall variables
        invisibleWallPathEntrance = GameObject.FindWithTag("InvisibleWalls/PathEntrance");

        //Setting up running shadow variables
        startStatic = start;
        endStatic = end;

        //Setting up incoming bus variables
        staticBus = bus;

        
    }

    void Update()
    {
        if (firstStepsPlayed)
        {
            Captions.AddTextToQueue("What was that sound? Footsteps?", 1f);
            Captions.AddTextToQueue("Anyway, since I have time, I'll take a walk down that path back there.", 3);

            CurrentObjectiveHandler.SetCurrentObjective("Walk down the dirt path");

            DisableInvisibleWall(invisibleWallPathEntrance);
            firstStepsPlayed = false;
        }

    }

    //General event purpose functions
    public static bool IsNotInView(Vector3 pos, Transform camTransform)
    {
        Vector3 verticalVector;
        verticalVector = Vector3.ProjectOnPlane((pos - camTransform.position), camTransform.right);

        Debug.Log(Vector3.Dot(camTransform.forward, (pos - camTransform.position).normalized));
        Debug.Log(Vector3.Angle(camTransform.forward, verticalVector));

        if (Vector3.Dot(camTransform.forward, (pos - camTransform.position).normalized) > 0.848f | Vector3.Angle(camTransform.forward, verticalVector) < 90)
        {
            Debug.Log("In View: True");
            return true;
        }
        else
        {
            Debug.Log("In View: False");
            return false;
        }
    }

    public static bool IsInView(Vector3 pos, Transform camTransform)
    {
        if (Vector3.Dot(camTransform.forward, (pos - camTransform.position).normalized) > 0.848f)
        {
            return false;
        }
        else
        {
            return true;
        }
    }


    public static void DisableInvisibleWall(GameObject wallObject)
    {
        wallObject.SetActive(false);
    }


    //Footstep Event Functions
    public static IEnumerator PlayAmbientFootsteps(Vector3 pos)
    {
        footstepAudioListenerPos.position = pos;

        Stopwatch sw = new();
        sw.Start();

        while (sw.Elapsed < TimeSpan.FromSeconds(3))
        {
            if (Physics.Raycast(footstepAudioListenerPos.position, Vector3.down, out RaycastHit hit, 3))
            {
                Debug.Log("Shooting Raycast");
                switch (hit.collider.tag)
                {
                    case "FootstepSounds/Grass":
                        footstepAudioSource.PlayOneShot(grassAudioClips[Random.Range(0, grassAudioClips.Length - 1)]);
                        break;
                    case "FootstepSounds/Wood":
                        footstepAudioSource.PlayOneShot(woodAudioClips[Random.Range(0, woodAudioClips.Length - 1)]);
                        break;
                    default:
                        footstepAudioSource.PlayOneShot(grassAudioClips[Random.Range(0, grassAudioClips.Length - 1)]);
                        break;
                }
            }

            footstepAudioSource.pitch = Random.Range(0.9f, 1.1f);
            yield return new WaitForSeconds(0.75f);

        }

        firstStepsPlayed = true;
        yield return null;

    }

    public static void Move()
    {
        Teleport.Move(endStatic.position);
              
    }

    public static void PrepareRunningShadowEvent()
    {
        Teleport.MoveToPos(startStatic);
        Teleport.Rotate(endStatic);
    }

    public static void PrepareIncomingBusEvent()
    {
        BusMovement.EnableMeshes(staticBus);
        staticBus.GetComponent<AudioSource>().Play();
    }

    public static void MoveBusTowardsPlayer(GameObject bus, Vector3 targetPos) 
    {
        BusMovement.MoveBusTowardsPlayer(bus, targetPos);
    }


    
}
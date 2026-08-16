using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Camera : MonoBehaviour
{

    //Camera variables
    public float sensitivityX;
    public float sensitivityY;

    public Transform orientation;

    float xRotation;
    float yRotation;

    public static bool rotationToggle;
    
    //footstep sound variables

    [SerializeField] private float stepSpeed = 90f;
    [SerializeField] private float sprintMultiplier = 8f;
    [SerializeField] private AudioSource camAudioSource = default;
    [SerializeField] private AudioClip[] grassAudioClips = default;
    [SerializeField] private AudioClip[] concreteAudioClips = default;
    [SerializeField] private AudioClip[] woodAudioClips = default;
    private float stepTimer = 0;
    private float GetTimerMultiplier => Movement.isSprinting ? stepSpeed / sprintMultiplier : stepSpeed;

    [SerializeField] public GameObject intText;

    //entity teleportation variables

    private GameObject[] sightingObjects;
    private Queue<Transform> sightingPositions = new Queue<Transform> { };
    private Transform currentSightingPos;
    private bool teleported = false;
    private bool busStopSignSeen = false;

    [SerializeField] private Transform playerPos;

    //Coroutine variables
    private IEnumerator coroutine2;

    //GameObjects
    public static GameObject itemObj;
    private GameObject player;
    [SerializeField] private GameObject flashlight;

    //Transform variables
    [SerializeField] private Transform cameraPos;
    [SerializeField] private Transform shadowPriestHeadTransform;

    //Rotation variables
    private float rotateSpeed = 35f;
    private Vector3 targetDirection;


    //Sound clips
    [SerializeField] private AudioClip jumspcareSound;

    //Bool flags
    private bool flashlightState;
    private bool fullyRotated = false;

    IEnumerator WaitDisableMeshes()
    {
        yield return new WaitForSeconds(3);
        Teleport.DisableMeshes();
        currentSightingPos = sightingPositions.Dequeue();

    }

    IEnumerator WaitEnableActionsAndMovement()
    {
        yield return new WaitForSeconds(3);
        player.GetComponent<MonoBehaviour>().enabled = true;
        player.GetComponent<PlayerActions>().enabled = true;
        rotationToggle = true;

        if (!flashlightState)
        {
            flashlight.GetComponentInChildren<Light>().enabled = false;
        }

        fullyRotated = true;
    }

    IEnumerator WaitChangeTeleportedBool()
    {
        yield return new WaitForSeconds(2);
        teleported = true;
    }

    IEnumerator RotateTo()
    {
        while (Vector3.Angle(transform.position, targetDirection) > 7 & !fullyRotated)
        {
            Debug.Log("Running RotateTo");
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(targetDirection), rotateSpeed * Time.deltaTime);
            Debug.Log(Vector3.Angle(transform.position, targetDirection));

            yield return null;
        }       
        yield return null;
    }



    void Start()
    {
        //Setting camera parameters
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        rotationToggle = true;

        //Instantiating sighting object array and queue
        sightingObjects = GameObject.FindGameObjectsWithTag("Sighting");
        System.Array.Sort(sightingObjects, (a, b) => a.name.CompareTo(b.name));

        foreach (GameObject obj in sightingObjects)
        {
            sightingPositions.Enqueue(obj.GetComponent<Transform>());
        }
        currentSightingPos = sightingPositions.Dequeue();

        intText.GetComponent<TMP_Text>().enabled = false;

        player = GameObject.Find("Player");


    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (rotationToggle)
        {
            float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensitivityX;
            float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensitivityY;

            yRotation += mouseX;
            xRotation -= mouseY;

            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            //rotate cam and orientation
            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
            orientation.rotation = Quaternion.Euler(0, yRotation, 0);
        }
        
        //Checks if player is looking at sighting pos and teleports enemy model
        if (CheckForSighting(currentSightingPos) & !teleported)
        {
            Debug.Log("Running CheckForSighting");
            Teleport.MoveToPos(currentSightingPos);
            Teleport.Rotate(transform);
            Movement.freezeMovement();
            DisableActionsAndMovement();
            targetDirection = shadowPriestHeadTransform.position - transform.position;

            StartCoroutine(RotateTo());

            if (flashlight.GetComponentInChildren<Light>().enabled != true) 
            {
                flashlightState = false;
                flashlight.GetComponentInChildren<Light>().enabled = true;
            } else
            {
                flashlightState = true;
            }

            camAudioSource.PlayOneShot(jumspcareSound);

            Captions.AddTextToQueue("What's that?", 2f);

            StartCoroutine(WaitDisableMeshes());
            StartCoroutine(WaitEnableActionsAndMovement());

            teleported = true;

        } else if (!CheckForSighting(currentSightingPos)) 
        {
            if (sightingPositions.Count > 0)
            {
                teleported = false;
            } else
            {
                coroutine2 = WaitChangeTeleportedBool();
                StartCoroutine(coroutine2);
            }
        }

        stepManager();

        if (!busStopSignSeen)
        {
            LookingAtBusScreen();
        }

        CheckForItem();

        CheckForDoor();

        CheckForBus();

    }

    bool CheckForSighting(Transform pos)
    {
        if (Vector3.Dot(transform.forward,(pos.position - transform.position).normalized) > 0.01f && Vector3.Distance(transform.position, pos.position) < 4f)
        {
            return true;
        } else
        {
            return false;
        }
    }

    void stepManager()
    {
        if (!Movement.isMoving)
        {
            return;
        }

        stepTimer -= Time.deltaTime;

        if (stepTimer <= 0)
        {
            if (Physics.Raycast(playerPos.position, Vector3.down, out RaycastHit hit, 3))
            {
                switch (hit.collider.tag)
                {
                    case "FootstepSounds/Grass":
                        camAudioSource.PlayOneShot(grassAudioClips[Random.Range(0, grassAudioClips.Length - 1)]);
                        break;
                    case "FootstepSounds/Concrete":
                        camAudioSource.PlayOneShot(concreteAudioClips[Random.Range(0, concreteAudioClips.Length - 1)]);
                        break;
                    case "FootstepSounds/Wood":
                        camAudioSource.PlayOneShot(woodAudioClips[Random.Range(0, woodAudioClips.Length - 1)]);
                        break;
                    default:
                        camAudioSource.PlayOneShot(grassAudioClips[Random.Range(0, grassAudioClips.Length - 1)]);
                        break;
                }
            }

            stepTimer = GetTimerMultiplier;
            camAudioSource.pitch = Random.Range(0.9f, 1.1f);
        }

    }

    private void LookingAtBusScreen()
    {
        if (Physics.Raycast(playerPos.position, transform.forward, out RaycastHit hit, 7))
        {
            Debug.DrawRay(playerPos.position, playerPos.forward, Color.blue);
            if (hit.collider.tag == "BusStopScreen")
            {
                Debug.Log("Looking at screen");
                Captions.AddTextToQueue("10 minutes? That's longer than I expected...", 3);
                Captions.AddTextToQueue("I'll just wait here then.", 4);
                busStopSignSeen = true;
            } 
        } 
    }

    private void CheckForItem()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit itemHit, 2))
        {
            Debug.DrawRay(transform.position, transform.forward, Color.red);

            if (itemHit.collider.tag.Contains("Items") == true)
            {
                intText.GetComponent<TMP_Text>().enabled = true;
                if (Input.GetKeyDown(KeyCode.E))
                {
                    Movement.freezeMovement();
                    itemHit.collider.gameObject.GetComponent<ItemHandling>().InspectItem();
                }
            }

            else if (itemHit.collider.tag == "Interactables/Door" | itemHit.collider.tag == "Interactables/Bus" & !ItemHandling.inspecting)
            {
                intText.GetComponent<TMP_Text>().enabled = true;
            }
            else
            {
                intText.GetComponent<TMP_Text>().enabled = false;
            }
        }

    }

    private void DisableActionsAndMovement()
    {
        player.GetComponent<MonoBehaviour>().enabled = false;
        player.GetComponent<PlayerActions>().enabled = false;
        Movement.isMoving = false;
        rotationToggle = false;
    }

    private void CheckForDoor()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit doorHit, 3))
        {
            if (doorHit.collider.tag == "Interactables/Door")
            {
                intText.GetComponent<TMP_Text>().enabled = true;
                Animator doorAnim = doorHit.collider.GetComponentInParent<Animator>();
                if (Input.GetKeyDown (KeyCode.E))
                {
                    if (doorAnim.GetCurrentAnimatorStateInfo(0).IsName("New State"))
                    {
                        doorAnim.SetTrigger("Open");
                    }
                    if (doorAnim.GetCurrentAnimatorStateInfo(0).IsName("Door Open"))
                    {
                        doorAnim.SetTrigger("Close");
                    }
                    if (doorAnim.GetCurrentAnimatorStateInfo(0).IsName("Door Closed"))
                    {
                        doorAnim.SetTrigger("Open");
                    }

                }
                
            } else if (doorHit.collider.tag.Contains("Items/") | doorHit.collider.tag == "Interactables/Door" & !ItemHandling.inspecting)
            {
                intText.GetComponent<TMP_Text>().enabled = true;
            } else
            {
                intText.GetComponent<TMP_Text>().enabled = false;
            }
        }
    }

    private void CheckForBus()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit busHit, 3))
        {
            if (busHit.collider.tag == "Interactables/Bus")
            {
                intText.GetComponent<TMP_Text>().enabled = true;
                if (Input.GetKeyDown(KeyCode.E))
                {
                    Application.Quit();
                }
            } else if (busHit.collider.tag.Contains("Items/") | busHit.collider.tag == "Interactables/Door" & !ItemHandling.inspecting)
            {
                intText.GetComponent<TMP_Text>().enabled = true;
            } else
            {
                intText.GetComponent<TMP_Text>().enabled = false;
            }
        }
    }
}
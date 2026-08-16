using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class FinalEventHandler : MonoBehaviour
{

    private static SkinnedMeshRenderer[] skinnedMeshRenderers;
    private NavMeshAgent navMeshAgent;
    public static bool eventRunning;
    private static bool runCoroutine = false;
    [SerializeField] public GameObject[] invisibleWalls;
    [SerializeField] private Transform target;
    private Animator animator;
    private Vector3 targetDirection;
    private bool rotating = false;
    private float rotateSpeed = 200f;
    [SerializeField] private Transform cam;
    [SerializeField] private GameObject player;
    [SerializeField] private AudioSource camAudioSource = default;
    [SerializeField] private AudioClip jumspcareSound;
    [SerializeField] private GameObject flashlight;
    private bool flashlightState = false;
    private Vector3 lastPos;
    private float distTravelled;


    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        DisableMeshes();
        animator = GetComponent<Animator>();
        navMeshAgent.updateUpAxis = false;
        lastPos = cam.position;
    }

    IEnumerator WaitChangePath()
    {

        navMeshAgent.destination = cam.position;
        while (eventRunning)
        {

            //distTravelled = Vector3.Distance(cam.position, navMeshAgent.destination);
            distTravelled = Vector3.Distance(new Vector3(cam.position.x, 0, cam.position.z), new Vector3(navMeshAgent.destination.x, 0, navMeshAgent.destination.z));
            navMeshAgent.isStopped = false;
            if (!navMeshAgent.pathPending & distTravelled > 0.8f)
            {
                if (distTravelled > 1.8f)
                {
                    navMeshAgent.ResetPath();
                    navMeshAgent.destination = target.position;
                }
                navMeshAgent.destination = target.position;
                lastPos = target.position;
            }
            yield return new WaitForSeconds(0.5f);
        }
        if (!eventRunning)
        {
            navMeshAgent.isStopped = true;
        }
        yield return null;
    }

    IEnumerator RotateTo()
    {

        rotating = true;
        DisableActionsAndMovement();
        StartCoroutine(WaitChangeRotating());
        camAudioSource.PlayOneShot(jumspcareSound);
        while (rotating)
        {            
            cam.rotation = Quaternion.RotateTowards(cam.rotation, Quaternion.LookRotation(targetDirection), rotateSpeed * Time.deltaTime);

            yield return null;
        }
        yield return new WaitForSeconds(0.3f);
        player.GetComponent<MonoBehaviour>().enabled = true;
        player.GetComponent<PlayerActions>().enabled = true;

        if (!flashlightState)
        {
            flashlight.GetComponentInChildren<Light>().enabled = false;
        }

        Camera.rotationToggle = true;
        yield return null;
    }

    IEnumerator WaitChangeRotating()
    {
        yield return new WaitForSeconds(1.2f);
        rotating = false;
    }

    void Update()
    {
     
        Debug.Log(distTravelled);

        if (runCoroutine)
        {

            if (flashlight.GetComponentInChildren<Light>().enabled != true)
            {
                flashlightState = false;
                flashlight.GetComponentInChildren<Light>().enabled = true;
            }
            else
            {
                flashlightState = true;
            }

            targetDirection = transform.position - cam.position;
            StartCoroutine(RotateTo());
            StartCoroutine(WaitChangePath());
            GetComponent<Collider>().enabled = true;
            DestroyInvisibleWalls();
            runCoroutine = false;
        }

        if (eventRunning)
        {
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 3))
            {
                Debug.DrawRay(transform.position, transform.forward, Color.red);

                if (hit.collider.tag == "Player")
                {
                    eventRunning = false;
                    //Movement.freezeMovement();
                    RespawnManager.respawn = true;
                    DisableMeshes();
                }
            }
        }
        
    }

    private void FixedUpdate()
    {
        if (navMeshAgent.isStopped == true)
        {
            animator.SetTrigger("Not Moving");
        } else
        {
            animator.SetTrigger("Moving");
        }      
    }

    private void DisableMeshes()
    {
        foreach (SkinnedMeshRenderer skinnedMeshRenderer in skinnedMeshRenderers)
        {
            skinnedMeshRenderer.enabled = false;
        }
        GetComponent<Collider>().enabled = false;
    }

    private void DestroyInvisibleWalls()
    {
        foreach (GameObject wall in invisibleWalls)
        {
            Destroy(wall);
        }
    }

    public static void EnableMeshes()
    {
        foreach (SkinnedMeshRenderer skinnedMeshRenderer in skinnedMeshRenderers)
        {
            skinnedMeshRenderer.enabled = true;
        }

    }


    public static void StartEvent()
    {
        eventRunning = true;
        runCoroutine = true;           
        CurrentObjectiveHandler.SetCurrentObjective("Get the 317");
    }


    private void DisableActionsAndMovement()
    {
        player.GetComponent<MonoBehaviour>().enabled = false;
        player.GetComponent<PlayerActions>().enabled = false;
        Movement.isMoving = false;
        Camera.rotationToggle = false;
    }

}
using System.Collections;
using TMPro;
using UnityEngine;

public class ItemHandling : MonoBehaviour
{

    [SerializeField] private Transform mainCam;
    public static bool inspecting = false;
    private Vector3 startPos;
    private Quaternion startRotation;
    [SerializeField] private float inspectDistance;
    private GameObject player;
    public static bool inspected = false;
    [SerializeField] public Transform footstepAudioEventPos;
    [SerializeField] public GameObject intText;

    IEnumerator PlayAmbientFootsteps(Vector3 pos)
    {
        yield return new WaitForSeconds(3);
        while (!inspected)
        {
            Debug.Log("Running first coroutine");
            if (!EventManager.IsNotInView(pos, mainCam))
            {
                yield return new WaitForSeconds(1);
                StartCoroutine(EventManager.PlayAmbientFootsteps(pos));
                inspected = true;
            }
            yield return null;
        }
    }


    void Start()
    {
        startPos = transform.position;
        startRotation = transform.rotation;
        player = GameObject.Find("Player");
    }

    void Update()
    {
        if (inspecting)
        {
            intText.GetComponent<TMP_Text>().enabled = false;
        }
    }

    public void InspectItem()
    {
        //Disable player movement and controls when inspecting item.
        player.GetComponent<MonoBehaviour>().enabled = false;
        player.GetComponent<PlayerActions>().enabled = false;

        if (!inspecting)
        {
            Movement.isMoving = false;
            Camera.rotationToggle = false;

            transform.position = UnityEngine.Camera.main.ViewportToWorldPoint(new Vector3(0.5f,0.5f, inspectDistance)) ;
            switch (tag)
            {
                case "Items/Newspaper":
                    transform.rotation = Quaternion.LookRotation(mainCam.position);
                    transform.rotation = Quaternion.Euler(0, 180, -180);
                    break;
                case "Items/Book":
                    transform.rotation = Quaternion.LookRotation(mainCam.position);
                    transform.rotation = Quaternion.Euler(90, 180, 0);
                    break;
                default:
                    transform.rotation = Quaternion.LookRotation(mainCam.position);
                    transform.rotation = Quaternion.Euler(0, 180, -180);
                    break;
            }


            transform.LookAt(mainCam.transform.position, transform.up);
            mainCam.LookAt(transform.position);

            inspecting = true;

        } else
        {
            player.GetComponent<MonoBehaviour>().enabled = true;
            player.GetComponent<PlayerActions>().enabled = true;
            Camera.rotationToggle = true;

            if (!EventManager.firstStepsPlayed & tag == "Items/Newspaper")
            {
                StartCoroutine(PlayAmbientFootsteps(footstepAudioEventPos.position));
            }

            if (tag == "Items/Book")
            {
                ColliderEvent.EnableFinalEventCollider();
                Captions.AddTextToQueue("I need to get out of here", 1.5f);
                Captions.AddTextToQueue("I need to cut through the woods to make it.", 1.5f);

            }

            transform.position = startPos;
            transform.rotation = startRotation;
           
            inspecting = false;

        }

    }
}
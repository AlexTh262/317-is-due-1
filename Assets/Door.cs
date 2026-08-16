using UnityEngine;

public class Door : MonoBehaviour
{
    public float interactionDistance;
    public GameObject intText;

    void Start()
    {
        intText.SetActive(false);
    }

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            if (hit.collider.gameObject.tag == "Door") 
            {
                Debug.Log("Hitting door collider");
                GameObject doorParent = hit.collider.transform.root.gameObject;
                Animator doorAnim = doorParent.GetComponent<Animator>();
                intText.SetActive(true);               
                if (Input.GetKeyDown(KeyCode.E)) 
                {
                    if (doorAnim.GetCurrentAnimatorStateInfo(0).IsName("New State"))
                    {
                        doorAnim.SetTrigger("Open");
                        //Debug.Log("Opening Door From Start State");
                    }
                    if (doorAnim.GetCurrentAnimatorStateInfo(0).IsName("Door Open"))
                    {
                        doorAnim.SetTrigger("Close");
                        //Debug.Log("Closing door");
                    }
                    if (doorAnim.GetCurrentAnimatorStateInfo(0).IsName("Door Close"))
                    {
                        doorAnim.SetTrigger("Open");
                        //Debug.Log("Opening door");
                    }
                }
            }
        }
        else
        {
            intText.SetActive(false);
        }
    }
}

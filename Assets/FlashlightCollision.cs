using UnityEngine;

public class FlashlightCollision : MonoBehaviour
{
    public GameObject flashlight;

    private void OnTriggerEnter(Collider other)
    {
        if (other != null && other.GetType() != typeof(CapsuleCollider))
        {
            flashlight.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other != null && other.GetType() != typeof(CapsuleCollider))
        {
            flashlight.GetComponent<MeshRenderer>().enabled = true;
        }

    }
}

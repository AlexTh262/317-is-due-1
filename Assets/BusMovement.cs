using UnityEngine;

public class BusMovement : MonoBehaviour
{

    public static float speed = 5f;
    public static float eventSpeed = 10f;

    public static Transform orientation;
    public static Rigidbody rb;

    public static Vector3 targetPos = new Vector3(672.940002f, 0.24f, 978.700012f);

    public static bool arrived = false;
    public static bool reachedPlayer = false;


    //Variables for disabling and enabling meshes
    private static MeshRenderer[] meshRenderers;

    void Start()
    {
        orientation = GetComponent<Transform>();
        meshRenderers = GetComponentsInChildren<MeshRenderer>();
        DisableMeshes();
        rb = GetComponent<Rigidbody>();
    }

    public static void MoveBus(GameObject bus)
    {
        //rb.AddForce(orientation.forward * speed, ForceMode.Force);
        if (bus.GetComponent<Transform>().position != targetPos)
        {
            //Vector3 newPos = Vector3.MoveTowards(bus.GetComponent<Rigidbody>().position, targetPos, speed * Time.deltaTime);
            bus.GetComponent<Transform>().position = targetPos;
        } else
        {
            arrived = true;
        }
    }

    public static void DisableMeshes()
    {
        foreach (MeshRenderer meshRenderer in meshRenderers)
        {
            meshRenderer.enabled = false;
        }
    }

    public static void EnableMeshes(GameObject bus)
    {
        meshRenderers = bus.GetComponentsInChildren<MeshRenderer>();

        foreach (MeshRenderer meshRenderer in meshRenderers)
        {
            meshRenderer.enabled = true;
        }

       
    }

    public static void MoveBusTowardsPlayer(GameObject bus, Vector3 targetPos)
    {
        targetPos.y = 0;
        if (Vector3.Distance(bus.GetComponent<Rigidbody>().position, targetPos) > 5f)
        {
            Vector3 newPos = Vector3.MoveTowards(bus.GetComponent<Rigidbody>().position, targetPos, eventSpeed * Time.deltaTime);
            bus.GetComponent<Rigidbody>().MovePosition(newPos);
            bus.transform.LookAt(newPos);
        } else
        {
            reachedPlayer = true;
            Destroy(bus);
        }
    }
}
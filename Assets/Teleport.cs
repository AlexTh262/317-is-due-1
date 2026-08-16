using UnityEngine;

public class Teleport : MonoBehaviour
{

    public static Transform pos;
    public static SkinnedMeshRenderer[] skinnedMeshRenderers;
    private static Rigidbody rb;
    private static float speed = 5f;
    public static bool reachedTargetPosition = false;


    void Start()
    {
        pos = transform;
        skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        DisableMeshes();
        rb = GetComponent<Rigidbody>();
    }

    public static void DisableMeshes()
    {
        foreach (SkinnedMeshRenderer skinnedMeshRenderer1 in skinnedMeshRenderers)
        {
            skinnedMeshRenderer1.enabled = false;
        }
    }

    public static void EnableMeshes()
    {
        foreach (SkinnedMeshRenderer skinnedMeshRenderer1 in skinnedMeshRenderers)
        {
            skinnedMeshRenderer1.enabled = true;
        }
    }

    public static void MoveToPos(Transform newPos)
    {
        EnableMeshes();
        pos.position = newPos.position;
        Debug.Log(pos.position);
        
    }

    public static void Rotate(Transform pos1)
    {
        pos.LookAt(pos1);
    }

    public static void Move(Vector3 target)
    {
        if (rb.position != target)
        {
            Debug.Log("Moving");
            Vector3 newPos = Vector3.MoveTowards(rb.position, target, speed * Time.deltaTime);
            rb.MovePosition(newPos);
        } else
        {
            Debug.Log("Reached target position");
            reachedTargetPosition = true;
        }
    }



}
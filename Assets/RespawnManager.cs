using UnityEngine;
using UnityEngine.AI;

public class RespawnManager : MonoBehaviour
{

    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject priest;
    [SerializeField] private Transform priestSpawn;
    [SerializeField] private GameObject player;
    public static bool respawn;


    void Update()
    {
        if (respawn)
        {
            player.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
            player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            player.transform.position = spawnPoint.position;
            priest.GetComponent<NavMeshAgent>().Warp(priestSpawn.position);
            ColliderEvent.EnableFinalEventCollider();
            Captions.AddTextToQueue("I need to get out of here.", 1f);
            Captions.AddTextToQueue("I need to cut through the woods to make it.", 1.5f);
            player.transform.position = spawnPoint.position;
            respawn = false;
        }
    }
  
}
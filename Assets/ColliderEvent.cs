using UnityEngine;

public class ColliderEvent : MonoBehaviour
{

    private bool cachedRunningShadowEvent = false;
    private bool cachedIncomingBusEvent = false;
    private bool cachedEnteringChurch = false;
    private Vector3 incomingBusTarget;
    [SerializeField] public GameObject finalEventCollider;
    public static GameObject staticFinalEventCollider;

    private void Start()
    {
        staticFinalEventCollider = finalEventCollider;
    }


    private void FixedUpdate()
    {
        if(cachedRunningShadowEvent)
        {
            EventManager.Move();
            if (Teleport.reachedTargetPosition )
            {
                cachedRunningShadowEvent = false;
                gameObject.tag = null;
                Teleport.DisableMeshes();
            }
        }

        if (cachedIncomingBusEvent)
        {
            incomingBusTarget = GameObject.FindWithTag("MainCamera").GetComponent<Transform>().position;
            EventManager.MoveBusTowardsPlayer(EventManager.staticBus, incomingBusTarget);
            if (BusMovement.reachedPlayer)
            {
                cachedIncomingBusEvent = false;
                gameObject.tag = null;
            }
        }

        if (cachedEnteringChurch)
        {
            cachedEnteringChurch = false;
            gameObject.tag = null;
        }


    }

    private void OnTriggerEnter(Collider other)
    {     
        switch (gameObject.tag)
        {
            case "ColliderEvents/RunningShadow":
                EventManager.PrepareRunningShadowEvent();
                cachedRunningShadowEvent = true;
                break;
            case "ColliderEvents/IncomingBus":
                Debug.Log("Crossing collider");
                EventManager.PrepareIncomingBusEvent();
                cachedIncomingBusEvent = true;
                break;
            case "ColliderEvents/EnteringChurch":
                Captions.AddTextToQueue("A church? Is this the one mentioned in that newspaper I read?", 2);
                Captions.AddTextToQueue("I should check it out, maybe I'll get answers", 2);
                CurrentObjectiveHandler.SetCurrentObjective("Explore the church");
                cachedEnteringChurch = true;
                break;
            case "ColliderEvents/FinalEvent":
                FinalEventHandler.StartEvent();
                FinalEventHandler.EnableMeshes();
                DisableFinalEventCollider();
                break;
            case null:
                break;
        }
    
    }

    public static void EnableFinalEventCollider()
    {
        staticFinalEventCollider.GetComponent<Collider>().enabled = true;
    }

    public static void DisableFinalEventCollider()
    {
        staticFinalEventCollider.GetComponent<Collider>().enabled = false;
    }

}
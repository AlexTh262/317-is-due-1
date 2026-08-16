using TMPro;
using UnityEngine;
public class BusStopSign : MonoBehaviour
{

    float gameTime = 660f;
    float startTime;

    public GameObject bus;

    static bool hasRun = false;
    static bool isDue = false;

    TextMeshProUGUI busStopText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startTime = Time.time;
        busStopText = GetComponent<TextMeshProUGUI>();

    }

    // Update is called once per frame
    void Update()
    {
        float elapsedTime = Time.time - startTime;

        int minutes = (int)((gameTime - elapsedTime) / 60) % 60;
        string timerString = string.Format("{0:0}", minutes);

        if (minutes < 1)
        {
            busStopText.text = "317" + "                     " + "due";
            isDue = true;
        }
        else if (minutes == 1)
        {
            busStopText.text = "317" + "                      " + timerString + "min";
        }else if (FinalEventHandler.eventRunning)
        {
            busStopText.text = "317" + "                     " + "DIE";
        }
        else
        {
            busStopText.text = "317" + "                   " + timerString + " mins";
        }


    }

    private void FixedUpdate()
    {
        if (FinalEventHandler.eventRunning & hasRun == false) 
        {
            //Debug.Log("Trying to move bus");
            BusMovement.EnableMeshes(bus);
            BusMovement.MoveBus(bus);
            if (BusMovement.arrived == true) 
            {
                hasRun = true;
            }
        }

    }

}
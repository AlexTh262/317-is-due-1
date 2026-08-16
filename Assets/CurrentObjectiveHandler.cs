using System.Collections;
using TMPro;
using UnityEngine;

public class CurrentObjectiveHandler : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI objective;
    private static string currentObj;
    private static bool readyToUpdate = false;
    IEnumerator WaitClearText()
    {
        yield return new WaitForSeconds(3);
        objective.text = null;
        
    }

    void Start()
    {
        UpdateObjective("Go to bus stop");
        currentObj = "Go to bus stop";
        
    }

    // Update is called once per frame
    void Update()
    {
        if (readyToUpdate)
        {
            UpdateObjective(currentObj);
            readyToUpdate = false;
        }
        if (Input.GetKeyUp(KeyCode.Tab) && objective.text == null)
        {
            UpdateObjective(currentObj);
        }

    }

    private void UpdateObjective(string objectiveText)
    {
        objective.text = objectiveText;
        StartCoroutine(WaitClearText());
    }


    public static void SetCurrentObjective(string text)
    {
        currentObj = text;
        readyToUpdate = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Captions : MonoBehaviour
{

    [SerializeField]
    TextMeshProUGUI monologueText;

    private IEnumerator coroutine;


    public static Queue<string> monologueLinesQueue = new Queue<string> { };
    [SerializeField] private string[] linesArray;
    private bool running = false;
    public static bool textAdded = false;
    private bool initialTextComplete = false;
    private static bool runCoroutine = false;
    private static float addTextTimer;

    IEnumerator CoroutineInitialisation()
    {
        yield return new WaitForSeconds(4);
        StartCoroutine(WaitUpdateText());
    }

    IEnumerator WaitUpdateText()
    {
        Debug.Log("Executing WaitUpdateText");
        running = true;
        while (running)
        {
            UpdateCaption();
            yield return new WaitForSeconds(4);
        }

    }

    IEnumerator WaitAddText(float time)
    {
        Debug.Log("Executing WaitAddText");
        running = true;
        while (running && initialTextComplete && textAdded)
        {
            yield return new WaitForSeconds(time);
            UpdateCaption();
            StartCoroutine(WaitChangeTextAdded());
        }
    }

    IEnumerator WaitChangeTextAdded()
    {
        yield return new WaitForSeconds(25);
        textAdded = false;
    }

    void Start()
    {
        monologueText.canvasRenderer.SetAlpha(0);
        monologueText.CrossFadeAlpha(1, 2.0f, false);

        foreach (string line in linesArray) 
        {
            monologueLinesQueue.Enqueue(line);
        }
        
        coroutine = CoroutineInitialisation();
        StartCoroutine(coroutine);
    }

    void Update()
    {

        if (runCoroutine)
        {
            StartCoroutine(WaitAddText(addTextTimer));
            runCoroutine = false;
        }

    }

    private void UpdateCaption()
    {
        if (monologueLinesQueue.Count <= 0) 
        {
            monologueText.text = null;
            running = false;
            initialTextComplete = true;
            return;

        }
        else
        {
            Debug.Log("Executing UpdateCaption");
            monologueText.text = monologueLinesQueue.Dequeue();
        }
     
    }


    public static void AddTextToQueue(string text, float time)
    {
        monologueLinesQueue.Enqueue(text);
        textAdded = true;
        runCoroutine = true;
        addTextTimer = time;

        Debug.Log("Running AddTextToQueue");
        Debug.Log(textAdded);
    }


}
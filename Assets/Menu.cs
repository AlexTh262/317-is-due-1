using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

    public bool isStart;
    public bool isQuit;

    private void OnMouseUp()
    {
        if (isStart)
        {
            SceneManager.LoadScene(1);
        }
        else if (isQuit)
        {
            Application.Quit();
        }
    }
}

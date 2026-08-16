using UnityEngine;
using System.Collections;

public class ButtonControl : MonoBehaviour
{
    Material rend1;
    MeshRenderer meshRend;
    Color colour;
    
    void Start()
    {
        //rend1.SetColor("_Color", GetComponent<MeshRenderer>().material.GetColor("_FaceColor"));
        
        meshRend = GetComponent<MeshRenderer>();
        colour = meshRend.GetComponent<MeshRenderer>().material.GetColor("_FaceColor");
    }

    private void OnMouseEnter()
    {
        meshRend.material.SetColor("_FaceColor", Color.gray);
    }

    private void OnMouseExit()
    {
        meshRend.material.SetColor("_FaceColor", colour);
        ;
    }
}

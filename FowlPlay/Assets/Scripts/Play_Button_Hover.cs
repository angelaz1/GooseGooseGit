using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Play_Button_Hover : MonoBehaviour
{
    public Image owlImage;
    public Sprite owlSquint;
    public Sprite owlOpen;

    void Start() {}
    void Update() {}

    void OnMouseOver()
    {
        Debug.Log("Moused over");
        owlImage.GetComponent<SpriteRenderer>().sprite = owlSquint;
    }

    void OnMouseExit()
    { 
        Debug.Log("Moused stopped");
        owlImage.GetComponent<SpriteRenderer>().sprite = owlOpen;
    }
}

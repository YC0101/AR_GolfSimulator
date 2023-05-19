using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class GolfButton : MonoBehaviour, IPointerDownHandler
{
    public GameObject AROrigin;

    public bool isPressed = false;
    // Start is called before the first frame update
    void Start()
    {

    }


    public void OnPointerDown(PointerEventData eventData)
    {
        if (isPressed)
        {
            isPressed = false;
            EventSystem.current.SetSelectedGameObject(null);

        }
        else
        {
            EventSystem.current.SetSelectedGameObject(null);
            isPressed = true;
            EventSystem.current.SetSelectedGameObject(this.gameObject);

        }
    }


    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject != this.gameObject)
        {
            isPressed = false;
        }

        if (isPressed)
        {
            AROrigin.GetComponent<PlaceObj>().setBack();
            AROrigin.GetComponent<PlaceObj>().isBall = true;
        }
        else
        {
            AROrigin.GetComponent<PlaceObj>().isBall = false;
        }
    }
}

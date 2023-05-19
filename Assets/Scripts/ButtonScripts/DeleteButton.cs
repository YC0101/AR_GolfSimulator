using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DeleteButton : MonoBehaviour, IPointerDownHandler
{    
    public GameObject cameraAR;
    public GameObject deleteText;
    public bool isPressed = false;

    void Start()
    {
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
    }


    void Update()
    {
        if (isPressed)
        {
            if (cameraAR.GetComponent<PlaceObj>().isSelecting)
            {
                Destroy(cameraAR.GetComponent<PlaceObj>().selectedObject);
                deleteText.SetActive(true);
                isPressed = false;

            }
            else
            {
                isPressed = false;
            }
        }
    }
}

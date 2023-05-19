using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ResetButton : MonoBehaviour, IPointerDownHandler
{
    public GameObject AROrigin;

    private GameObject[] layerObj;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        layerObj = GameObject.FindGameObjectsWithTag("Ball");
        foreach(GameObject b in layerObj)
        {
            Object.Destroy(b, 0.0f);
        }

        layerObj = GameObject.FindGameObjectsWithTag("Ref");
        foreach (GameObject b in layerObj)
        {
            Object.Destroy(b, 0.0f);
        }

        layerObj = GameObject.FindGameObjectsWithTag("Hole");
        foreach (GameObject b in layerObj)
        {
            Object.Destroy(b, 0.0f);
        }

        layerObj = GameObject.FindGameObjectsWithTag("Wood");
        foreach (GameObject b in layerObj)
        {
            Object.Destroy(b, 0.0f);
        }

        layerObj = GameObject.FindGameObjectsWithTag("Wall");
        foreach (GameObject b in layerObj)
        {
            Object.Destroy(b, 0.0f);
        }

        layerObj = GameObject.FindGameObjectsWithTag("Fence");
        foreach (GameObject b in layerObj)
        {
            Object.Destroy(b, 0.0f);
        }

        layerObj = GameObject.FindGameObjectsWithTag("Stone");
        foreach (GameObject b in layerObj)
        {
            Object.Destroy(b, 0.0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

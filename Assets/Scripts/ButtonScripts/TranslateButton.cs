using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TranslateButton : MonoBehaviour, IPointerDownHandler
{
    public bool isPressed;
    public GameObject AROrigin;
    public Camera arCamera;
    public float touchMoveSpeed = 0.1f;
    Rect bottomRight = new Rect(3 * Screen.width / 4, 0, Screen.width / 4, Screen.height / 2);
    private GameObject currentSelected;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (isPressed)
        {
            isPressed = false;
            currentSelected.GetComponent<ObjOverlap>().canMove = true;
            currentSelected = null;
            //currentSelected.GetComponent<ObjOverlap>().canMove = true;
            EventSystem.current.SetSelectedGameObject(null);

        }
        else
        {
            EventSystem.current.SetSelectedGameObject(null);
            isPressed = true;
            currentSelected = AROrigin.GetComponent<PlaceObj>().selectedObject;
            currentSelected.GetComponent<ObjOverlap>().canMove = true;
            EventSystem.current.SetSelectedGameObject(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isPressed)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                Vector2 touchPos= touch.position;
                if (!bottomRight.Contains(touch.position))
                {
                    float initialDist = Vector3.Distance(currentSelected.transform.position, arCamera.transform.position);

                    Ray ray = arCamera.ScreenPointToRay(touchPos);
                    RaycastHit hitInfo;
                    Physics.Raycast(ray, out hitInfo);
                    Vector3 dir = ray.GetPoint(initialDist) - currentSelected.transform.position;
                    dir = new Vector3(dir.x, 0, dir.z);
                    currentSelected.transform.position += dir * touchMoveSpeed;
                    currentSelected.GetComponent<ObjOverlap>().maxTime = 30.0f;
                }


            }
        }
    }

        
}

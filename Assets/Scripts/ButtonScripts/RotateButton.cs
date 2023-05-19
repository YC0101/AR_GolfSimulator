using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RotateButton : MonoBehaviour, IPointerDownHandler
{
    public bool isPressed;
    public GameObject AROrigin;
    public Camera arCamera;
    public float touchMoveSpeed = 10f;
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
            Debug.Log("isPressed!");
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                Vector2 touchPos = touch.position;
                Debug.Log("Rotation1");
                if (!bottomRight.Contains(touch.position))
                {
                    float initialDist = Vector3.Distance(currentSelected.transform.position, arCamera.transform.position);
                    Debug.Log("Rotation2");
                    Ray ray = arCamera.ScreenPointToRay(touchPos);
                    RaycastHit hitInfo;
                    Physics.Raycast(ray, out hitInfo);
                    Vector3 dir = ray.GetPoint(initialDist) - currentSelected.transform.position;
                    Debug.Log("Direction is " + dir);
                    currentSelected.transform.Rotate(Vector3.down * dir.x * touchMoveSpeed, Space.Self);
                    currentSelected.GetComponent<ObjOverlap>().maxTime = 30.0f;
                }
            }
        }
    }

}

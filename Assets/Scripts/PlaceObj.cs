using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlaceObj : MonoBehaviour
{
    [SerializeField]
    private GameObject refPrefab, holePrefab, ballPrefab, fenPrefab, stonePrefab, woodPrefab, wallPrefab, selectedButton;
    public LayerMask placementLayerMask;
    private GameObject placedRef;
    private GameObject placedHole;
    private GameObject placedBall;
    private ARRaycastManager raycastManager;
    static List<ARRaycastHit> hits = new List<ARRaycastHit>();
    //private Vector3 offsetVec;

    //Decide which object to place and what method we use to place
    public bool isHole = false;
    public bool isBall = false;
    public bool isRef = false;
    public bool isFen = false;
    public bool isStone = false;
    public bool isWood = false;
    public bool isWall = false;
    public Camera cameraAR;

    //Disabled area
    Rect middleLeft = new Rect(0, 0, Screen.width / 4, Screen.height);
    Rect bottomRight = new Rect(3 * Screen.width / 4, 0, Screen.width / 4, Screen.height / 2);

    //Text Feedback
    [SerializeField]
    private GameObject deleteTextObject;

    //Select item
    public bool isSelecting = false;
    public GameObject selectedObject;
    public Material highlight;
    public Material originalMaterial;

    //button
    public GameObject TranslateButton;
    public GameObject RotateButton;

    // Start is called before the first frame update
    void Start()
    {
        raycastManager = GetComponent<ARRaycastManager>();
        deleteTextObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        checkSelect();
        if (isPlacing())
        {
            resetSelect();
        }
        // only try placing object if one-finger touch
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            // only try placing if the touch is a tap
            // (as opposed to a drag/flick)
            if (touch.phase == TouchPhase.Ended && !middleLeft.Contains(touch.position))
            {
                if (isSelecting && !bottomRight.Contains(touch.position))
                {
                    Debug.Log("Touch remove select in update");
                    EventSystem.current.SetSelectedGameObject(null);
                    selectedButton.SetActive(false);
                    setBack();
                    isSelecting = false;
                    selectedObject.GetComponent<ObjOverlap>().canMove = true;
                    selectedObject.GetComponent<MeshRenderer>().material = originalMaterial;
                    selectedObject = null;
                    //selectedObject.GetComponent<ObjOverlap>().canMove = false;
                    TranslateButton.GetComponent<TranslateButton>().isPressed = false;
                    RotateButton.GetComponent<RotateButton>().isPressed = false;
                }
                else if (isPlacing())
                {
                    

                    if (isHole && !checkHitObj(touch))
                    {
                        TryPlaceHole(touch);
                    }
                    else if (isRef && !checkHitObj(touch))
                    {
                        TryPlaceRef(touch);
                    }
                    else if (isBall && !checkHitObj(touch))
                    {
                        TryPlaceBall(touch);
                    }
                    else if (isWall)
                    {
                        TryPlaceWall(touch);
                    }
                    else if (isFen && !checkHitObj(touch))
                    {
                        TryPlaceObs(touch, fenPrefab);
                    }
                    else if (isWood && !checkHitObj(touch))
                    {
                        TryPlaceWood(touch);
                    }
                    else if (isStone && !checkHitObj(touch))
                    {
                        TryPlaceObs(touch, stonePrefab);
                    }
                    //--------------
                }
                else
                {
                    checkSelect(touch);
                }

            }
        }
    }

    //Check if an object button is being selected
    private bool isPlacing()
    {
        return (isHole || isBall || isRef || isFen || isStone || isWood || isWall);
    }

    private bool checkHitObj(Touch touch)
    {
        Ray ray = cameraAR.ScreenPointToRay(touch.position);
        RaycastHit hitInfo;
        Physics.Raycast(ray, out hitInfo);
        if(hitInfo.collider.gameObject.CompareTag("Wall") || hitInfo.collider.gameObject.CompareTag("Stone") || hitInfo.collider.gameObject.CompareTag("Wood") || hitInfo.collider.gameObject.CompareTag("Fence"))
        {
            return true;
        }
        else
        {
            return false;
        }

    }
    //======================Place Methods Starts Here============================
    //===========================================================================
    void TryPlaceHole(Touch touch)
    {
        Vector2 touchPos = touch.position;
        Ray ray = cameraAR.ScreenPointToRay(touchPos);
        RaycastHit hitInfo;
        Physics.Raycast(ray, out hitInfo);

        if (raycastManager.Raycast(touchPos, hits, TrackableType.PlaneWithinPolygon))
        {
            var hit = hits[0].pose;

            if (placedHole == null)
            {
                placedHole = Instantiate(holePrefab, hit.position, Quaternion.identity);
                placedHole.GetComponent<ObjOverlap>().mainControll = gameObject;
            }
            else placedHole.transform.position = hit.position;
        }
    }

    void TryPlaceRef(Touch touch)
    {
        Vector2 touchPos = touch.position;

        if (raycastManager.Raycast(touchPos, hits, TrackableType.PlaneWithinPolygon))
        {
            var hit = hits[0].pose;
            
            if (placedRef == null)
            {
                placedRef = Instantiate(refPrefab, hit.position, Quaternion.identity);
                placedRef.GetComponent<ObjOverlap>().mainControll = gameObject;
            }
            else placedRef.transform.position = hit.position;
        }
    }

    void TryPlaceBall(Touch touch)
    {
        Vector2 touchPos = touch.position;

        if (raycastManager.Raycast(touchPos, hits, TrackableType.PlaneWithinPolygon))
        {
            var hit = hits[0].pose;

            if (placedBall == null)
            {
                placedBall = Instantiate(ballPrefab, hit.position, Quaternion.identity);
                placedBall.transform.position += new Vector3(0, 0.01f, 0);
                placedBall.GetComponent<ObjOverlap>().mainControll = gameObject;
            }
            else
            {
                placedBall.transform.position = hit.position;
                placedBall.transform.position += new Vector3(0, 0.01f, 0);
            }
        }
    }

    void TryPlaceWall(Touch touch)
    {
        Vector2 touchPos = touch.position;
        Ray ray = cameraAR.ScreenPointToRay(touchPos);
        RaycastHit hitInfo;
        Physics.Raycast(ray, out hitInfo);

        if (hitInfo.collider.gameObject.CompareTag("Wall"))
        {
            if (hitInfo.normal == Vector3.up)
            {
                GameObject newObject = Instantiate(hitInfo.collider.gameObject);
                newObject.transform.position = newObject.transform.position + new Vector3(0, 0.1f, 0);
            }
        }
        else if (raycastManager.Raycast(touchPos, hits, TrackableType.PlaneWithinPolygon))
        {
            var hit = hits[0].pose;
            GameObject newObject = Instantiate(wallPrefab, hit.position , Quaternion.identity);
            newObject.GetComponent<ObjOverlap>().mainControll = gameObject;
            newObject.transform.position += new Vector3(0, 0.05f, 0);
        }
    }

    void TryPlaceObs(Touch touch, GameObject objPrefab)
    {
        Vector2 touchPos = touch.position;
        if (raycastManager.Raycast(touchPos, hits, TrackableType.PlaneWithinPolygon))
        {
            var hit = hits[0].pose;
            GameObject newObject = Instantiate(objPrefab, hit.position, Quaternion.identity);
            newObject.GetComponent<ObjOverlap>().mainControll = gameObject;
            newObject.transform.position += new Vector3(0, objPrefab.transform.localScale.y/2, 0);
        }
    }

    void TryPlaceWood(Touch touch)
    {
        Vector2 touchPos = touch.position;
        if (raycastManager.Raycast(touchPos, hits, TrackableType.PlaneWithinPolygon))
        {
            var hit = hits[0].pose;
            GameObject newObject = Instantiate(woodPrefab, hit.position, Quaternion.identity);
            newObject.GetComponent<ObjOverlap>().mainControll = gameObject;
            newObject.transform.position += new Vector3(0, 0.025f, 0);

        }
    }
    //======================Place Methods Ends Here==============================
    //===========================================================================
   

    //used in button scripts
    public void setBack()
    {
        isHole = false;
        isBall = false;
        isRef = false;
        isFen = false;
        isStone = false;
        isWood = false;
        isWall = false;
    }

    //Activate translation and rotation
    public void checkSelect()
    {
        if (isSelecting)
        {
            selectedButton.SetActive(true);
        }
        else
        {
            selectedButton.SetActive(false);
            
        }
    }

    //delete object
    public void deleteObject()
    {
        deleteTextObject.SetActive(true); // Enable the text so it shows
    }

    void checkSelect(Touch touch)
    {
        Ray ray = cameraAR.ScreenPointToRay(touch.position);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            
            if (hitInfo.collider.gameObject.CompareTag("Wall")|| hitInfo.collider.gameObject.CompareTag("Stone")|| hitInfo.collider.gameObject.CompareTag("Wood")|| hitInfo.collider.gameObject.CompareTag("Fence") || hitInfo.collider.gameObject.CompareTag("Ref"))
            {
                Debug.Log("Selected!");
                selectedObject = hitInfo.collider.transform.gameObject;
                isSelecting = true;
                originalMaterial = selectedObject.GetComponent<MeshRenderer>().material;
                selectedObject.GetComponent<MeshRenderer>().material = highlight;
                if (!hitInfo.collider.gameObject.CompareTag("Ref"))
                {
                    selectedObject.GetComponent<ObjOverlap>().canMove = true;
                }
                
            }
            else if (raycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon) && !bottomRight.Contains(touch.position))
            {
                setBack();
                EventSystem.current.SetSelectedGameObject(null);
                if (!selectedObject.gameObject.CompareTag("Ref"))
                {
                    selectedObject.GetComponent<ObjOverlap>().canMove = true;
                }
                selectedObject.GetComponent<MeshRenderer>().material = originalMaterial;
                selectedObject = null;
                isSelecting = false;
                TranslateButton.GetComponent<TranslateButton>().isPressed = false;
                RotateButton.GetComponent<RotateButton>().isPressed = false;
            }
            else
            {

            }
        }

    }

    public void resetSelect()
    {
        if (selectedObject != null)
        {
            selectedObject.GetComponent<MeshRenderer>().material = originalMaterial;
            selectedObject = null;
            isSelecting = false;
            originalMaterial = null;
            TranslateButton.GetComponent<TranslateButton>().isPressed = false;
            RotateButton.GetComponent<RotateButton>().isPressed = false;
            //selectedButton.SetActive(false);
        }
    }

}

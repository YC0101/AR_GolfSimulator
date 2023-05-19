using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjOverlap : MonoBehaviour
{

    
    public bool canMove = true;
    public LayerMask m_LayerMask;
    Vector3 prevDir = new Vector3(0, 0, 0);
    public float maxTime = 30.0f;
    Vector3 tmp = new Vector3(0, 0, 0);

    public bool isSelect = false;

    //Text Feedback
    public GameObject mainControll;
    [SerializeField]
    //private GameObject deleteTextObject;

    void Start()
    {
        tmp = transform.gameObject.GetComponent<BoxCollider>().size;
    }

    void FixedUpdate()
    {
        if (canMove)
        {
            MyCollisions();
        }
        if (maxTime <= 0)
        {
            Debug.Log("isDeleted");
            mainControll.GetComponent<PlaceObj>().deleteObject();
            Destroy(gameObject);
        }
        //MyCollisions();
    }

    void MyCollisions()
    {
        //Use the OverlapBox to detect if there are any other colliders within this box area.
        //Use the GameObject's centre, half the size (as a radius) and rotation. This creates an invisible box around your GameObject.
        //--Collider[] hitColliders = Physics.OverlapBox(gameObject.transform.position, transform.localScale / 2, Quaternion.identity, m_LayerMask);
        //Collider[] hitColliders = Physics.OverlapBox(gameObject.transform.position, gameObject.GetComponent<BoxCollider>().size / 2, Quaternion.identity, m_LayerMask);
        Collider[] hitColliders = Physics.OverlapBox(gameObject.transform.position, gameObject.transform.lossyScale / 2, Quaternion.identity, m_LayerMask);
        if (hitColliders.Length > 1) 
        {
            Debug.Log("Collide");
            float minPenetrationDistance = float.MaxValue;
            Vector3 nonOverlappingPosition = gameObject.transform.position;

            foreach (Collider collider in hitColliders)
            {
                if (collider != gameObject.GetComponent<Collider>())
                {
                    Vector3 direction;
                    float distance;

                    Physics.ComputePenetration(gameObject.GetComponent<Collider>(), gameObject.transform.position, gameObject.transform.rotation,
                        collider, collider.transform.position, collider.transform.rotation, out direction, out distance);



                    Vector3.Normalize(direction);
                    float limit = Vector3.Dot(new Vector3(1.0f, 1.0f, 1.0f), direction);
                    Vector3 modDir = direction;
                    if (direction.y < 0)
                    {
                        maxTime = -1;
                    }
                    if (direction.y > 0)
                    {
                        direction.z = direction.y;
                        direction.y = 0;
                    }
                    if (direction + prevDir == new Vector3(0, 0, 0) && limit < 10.0f)
                    {
                        //Debug.Log("Conflict");
                        modDir = new Vector3(direction.z, direction.y, direction.x);
                        //Debug.Log("Original:"+ direction);
                    }
                    else if (limit < 10.0f)
                    {
                        prevDir = direction;
                    }

                    if (distance < minPenetrationDistance && limit < 10.0f && maxTime >= 0)
                    {
                        float temp = Mathf.Abs(Vector3.Dot(tmp, modDir));

                        //Debug.Log("modified:" + modDir);
                        minPenetrationDistance = distance;
                        Debug.Log("check: " + 0.002f * temp);
                        nonOverlappingPosition = gameObject.transform.position + modDir * (distance + 0.002f * temp);
                        maxTime--;
                    }
                }
                gameObject.transform.position = nonOverlappingPosition;
            }

            transform.gameObject.GetComponent<BoxCollider>().size = new Vector3(0.998f * tmp.x, 0.998f * tmp.y, 0.998f * tmp.z);
        }
        else
        {
            canMove = false;
            Debug.Log("ObjOverlap canMove = false");
            transform.gameObject.GetComponent<BoxCollider>().size = new Vector3(0.998f * tmp.x, 0.998f * tmp.y, 0.998f * tmp.z);
        }

    }


}

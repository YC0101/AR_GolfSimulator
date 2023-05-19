using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deletInfo : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
        StartCoroutine(Wait());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }
}

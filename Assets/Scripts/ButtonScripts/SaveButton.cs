using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.IO;

public class SaveButton : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]
    private GameObject[] allObj;


    [SerializeField]
    private int layerNum;


    // Start is called before the first frame update
    void Start()
    {
        layerNum = LayerMask.NameToLayer("PlaceObjects");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Save is clicked" + Application.persistentDataPath);
        allObj = FindAllObj();
        if(allObj != null)
        {
            string[,] data = new string[allObj.Length, 7];

            for (int i = 0; i < allObj.Length; i++)
            {
                data[i, 0] = allObj[i].name;
                data[i, 1] = allObj[i].transform.position.ToString();
                //data[i, 2] = allObj[i].transform.position.y.ToString();
                //data[i, 3] = allObj[i].transform.position.z.ToString();
                data[i, 2] = allObj[i].transform.localScale.ToString();
                data[i, 3] = allObj[i].transform.rotation.ToString();
            }
            toCSV(data, "PlacedObjectsData");
            Debug.Log("file saved to:" + Application.persistentDataPath);
        }
        EventSystem.current.SetSelectedGameObject(null);
    }

    private GameObject[] FindAllObj()
    {
        var goArray = FindObjectsOfType(typeof(GameObject)) as GameObject[];
        var goList = new System.Collections.Generic.List<GameObject>();
        for (int i = 0; i < goArray.Length; i++)
        {
            if (goArray[i].layer == layerNum)
            {
                goList.Add(goArray[i]);
            }
        }
        if (goList.Count == 0)
        {
            return null;
        }
        return goList.ToArray();
    }

    private void toCSV(string[,] data, string filename)
    {
        StreamWriter sw = new StreamWriter(Application.persistentDataPath + "/" + filename + ".csv");

        for (int i = 0; i < data.GetLength(0); i++)
        {
            string row = "";
            for (int j = 0; j < data.GetLength(1); j++)
            {
                row += data[i, j] + ",";
            }
            sw.WriteLine(row);
        }
        sw.Close();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


struct Data
{
    public int m_nLevel;
    public Vector3 m_nPos;
    
    public void printData()
    {
        Debug.Log("Level : " + m_nLevel);
        Debug.Log("Position : " + m_nPos);
    }
};

public class JsonManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Data data = new Data();
        data.m_nLevel = 12;
        data.m_nPos = new Vector3(3.4f, 5.6f, 7.8f);

        string str = JsonUtility.ToJson(data);

        //Debug.Log("ToJson : " + str);

        Data data2 = JsonUtility.FromJson<Data>(str);
        data2.printData();


        Data data3 = new Data();
        data3.m_nLevel = 99;
        data3.m_nPos = new Vector3(8.1f, 9.2f, 7.2f);

        Debug.Log("TestPath : "+Application.dataPath + "/JsonManager.json");
        //File.WriteAllText(Application.dataPath + "/JsonManager.json", JsonUtility.ToJson(data3));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

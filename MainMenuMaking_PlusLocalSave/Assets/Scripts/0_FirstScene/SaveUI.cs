using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveUI : MonoBehaviour
{
    public SaveLoad saveType;
    public string savePath;
    public int pathLen;
    public void OnClickSaveBtn()
    {
        switch(saveType)
        {
            case SaveLoad.Save1:
                if(pathLen == savePath.Length)
                {
                    Debug.Log("add log1");
                    savePath += "\\01";
                }
                break;
            case SaveLoad.Save2:
                if (pathLen == savePath.Length)
                {
                    Debug.Log("add log2");
                    savePath += "\\02";
                }
                break;
            case SaveLoad.Save3:
                if (pathLen == savePath.Length)
                {
                    Debug.Log("add log3");
                    savePath += "\\03";
                }
                break;
            case SaveLoad.Save4:
                if (pathLen == savePath.Length)
                {
                    Debug.Log("add log4");
                    savePath += "\\04";
                }
                break;
            case SaveLoad.Save5:
                if (pathLen == savePath.Length)
                {
                    Debug.Log("add log");
                    savePath += "\\05";
                }
                break;
        }
        Debug.Log("path : "+savePath);
    }

    // Start is called before the first frame update
    void Start()
    {
        savePath = Application.dataPath + "\\Saves";
        pathLen = savePath.Length;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

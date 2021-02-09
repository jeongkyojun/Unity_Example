using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class SaveUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject firstSceneGameManager;
    MenuManagingScript menuEntity;
    public SaveLoad saveType;
    public int pathLen;

    Image childPanelImage;
    Vector3 defaultSize;
    Color defaultColor;

    public void OnClickSaveBtn()
    {
        switch(saveType)
        {
            case SaveLoad.Save1:
                if(pathLen == menuEntity.pE.savePath.Length)
                {
                    Debug.Log("add log1");
                    menuEntity.pE.savePath += "\\01";
                }
                break;
            case SaveLoad.Save2:
                if (pathLen == menuEntity.pE.savePath.Length)
                {
                    Debug.Log("add log2");
                    menuEntity.pE.savePath += "\\02";
                }
                break;
            case SaveLoad.Save3:
                if (pathLen == menuEntity.pE.savePath.Length)
                {
                    Debug.Log("add log3");
                    menuEntity.pE.savePath += "\\03";
                }
                break;
            case SaveLoad.Save4:
                if (pathLen == menuEntity.pE.savePath.Length)
                {
                    Debug.Log("add log4");
                    menuEntity.pE.savePath += "\\04";
                }
                break;
            case SaveLoad.Save5:
                if (pathLen == menuEntity.pE.savePath.Length)
                {
                    Debug.Log("add log");
                    menuEntity.pE.savePath += "\\05";
                }
                break;
        }
        Debug.Log("path : "+menuEntity.pE.savePath);
        Debug.Log("새게임");
        LoadManaging(menuEntity.pE.savePath);
        SceneManager.LoadScene("1_Loading");
    }

    // Start is called before the first frame update
    void Start()
    {
        menuEntity = firstSceneGameManager.GetComponent<MenuManagingScript>();
        menuEntity.pE.savePath = Application.dataPath + "\\Saves";
        pathLen = menuEntity.pE.savePath.Length;
        
        defaultSize = transform.localScale;
        childPanelImage = transform.GetComponentInChildren<Image>();
        defaultColor = childPanelImage.color;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LoadManaging(string savePath)
    {
        if (!Directory.Exists(savePath))
        {
            Debug.Log("새로운 게임을 만들기 위해 파일 경로를 만듭니다.");
            Directory.CreateDirectory(savePath);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //buttonScale.localScale = defaultScale * 1.2f;
        transform.localScale = defaultSize * 1.1f;
        childPanelImage.color = defaultColor * 2f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //buttonScale.localScale = defaultScale;
        transform.localScale = defaultSize;
        childPanelImage.color = defaultColor;
    }

    #region save&load
    // 제네릭 함수로 변환
    public static void LoadData<T>(ref T data, string filePath, string fileName)
    {
        Debug.Log("데이터를 로드합니다.");
        string readJson;
        T readData;
        try
        {
            Debug.Log(filePath + fileName);
            readJson = File.ReadAllText(filePath + fileName); // 데이터를 읽어서 readJson에 넣는다.
            readData = JsonUtility.FromJson<T>(readJson); // readJson string을 구조체로 변환한다.
                                                          //readData = JsonConvert.DeserializeObject<T>(readJson);
            Debug.Log("데이터를 읽었습니다. 진행사항을 불러옵니다.");

            data = readData; // 읽은 데이터를 데이터에 집어넣는다.
        }
        catch (Exception e)
        {
            //Debug.Log("error :: " + e);
            MenuEntity defaultMenu = new MenuEntity();
            Debug.Log("경로에 파일이 없습니다. 빈 파일을 생성합니다.");
            defaultMenu.Volume = 0f;
            SaveData(ref defaultMenu, filePath, fileName);
        }
    }

    public static void SaveData<T>(ref T data, string filePath, string fileName)
    {
        Debug.Log("저장을 실행합니다.");
        File.WriteAllText(filePath + fileName, JsonUtility.ToJson(data));
        //File.WriteAllText(filePath, JsonConvert.SerializeObject(data));
        Debug.Log("저장이 완료되었습니다.");
        Debug.Log("저장값 : " + data);
    }
    #endregion
}

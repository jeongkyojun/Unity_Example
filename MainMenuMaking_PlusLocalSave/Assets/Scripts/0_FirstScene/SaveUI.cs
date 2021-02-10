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

    public GameObject ImageSet;
    Image saveImage;
    Image childPanelImage;
    Text childText;
    Vector3 defaultSize;
    Color defaultColor;
    bool isLoad;
    GameEntity gE = new GameEntity();

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

        saveImage = ImageSet.GetComponent<Image>();

        defaultSize = transform.localScale;
        childPanelImage = transform.GetComponentInChildren<Image>();
        childText = transform.GetComponentInChildren<Text>();
        childText.text = "helloworld";
        defaultColor = childPanelImage.color;
        
        switch (saveType)
        {
            case SaveLoad.Save1:
                if (pathLen == menuEntity.pE.savePath.Length)
                {
                    Debug.Log("add log1");
                    isLoad = LoadData<GameEntity>(ref gE, menuEntity.pE.savePath + "\\01\\", "playerSave.json", childText,saveImage);
                }
                break;
            case SaveLoad.Save2:
                if (pathLen == menuEntity.pE.savePath.Length)
                {
                    Debug.Log("add log2");
                    isLoad = LoadData<GameEntity>(ref gE, menuEntity.pE.savePath + "\\02\\", "playerSave.json", childText, saveImage);
                }
                break;
            case SaveLoad.Save3:
                if (pathLen == menuEntity.pE.savePath.Length)
                {
                    Debug.Log("add log3");
                    isLoad = LoadData<GameEntity>(ref gE, menuEntity.pE.savePath + "\\03\\", "playerSave.json", childText, saveImage);
                }
                break;
            case SaveLoad.Save4:
                if (pathLen == menuEntity.pE.savePath.Length)
                {
                    Debug.Log("add log4");
                    menuEntity.pE.savePath += "\\04";
                    isLoad = LoadData<GameEntity>(ref gE, menuEntity.pE.savePath + "\\04\\", "playerSave.json", childText, saveImage);
                }
                break;
            case SaveLoad.Save5:
                if (pathLen == menuEntity.pE.savePath.Length)
                {
                    Debug.Log("add log");
                    isLoad = LoadData<GameEntity>(ref gE, menuEntity.pE.savePath + "\\05\\", "playerSave.json",childText, saveImage);
                }
                break;
        }
        if(isLoad)
        {
            childText.text = "level : " + gE.level.ToString() +
                "\nhp : " + gE.hp.ToString() + " / " + gE.maxHp.ToString() +
                "\nplayTime : " + gE.play_h.ToString() + " : " + gE.play_m.ToString() + " : " +gE.play_s.ToString();
            saveImage.color = Color.cyan;
        }
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
    public static bool LoadData<T>(ref T data, string filePath, string fileName,Text childText,Image Img)
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
            return true;
        }
        catch (Exception e)
        {
            childText.text = "새 게임 시작하기";
            Img.color = new Color(0, 0, 0, 0);
            return false;
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

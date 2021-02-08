using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

[System.Serializable]
public struct MenuEntity
{
    public float Volume;
};

public struct PathEntity
{
    public string savePath;
}

public class MenuManagingScript : MonoBehaviour
{
    string MenuEntityPath;
    string fileName = "\\OptionSave.json";
    public MenuEntity mE = new MenuEntity();
    public PathEntity pE = new PathEntity();

    public Slider volumeSlider;
    public Text volumeText;
    AudioSource volume;


    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        volume = GetComponent<AudioSource>();

        MenuEntityPath = Application.dataPath + "\\Saves";
        mE.Volume = volumeSlider.value;

        if (!Directory.Exists(MenuEntityPath))
        {
            Debug.Log("파일경로가 없습니다. 경로를 새로 만듭니다.");
            Directory.CreateDirectory(MenuEntityPath);
        }
        LoadData<MenuEntity>(ref mE, MenuEntityPath, fileName);

        Debug.Log("불러온 값 : " + mE.Volume);

        volumeSlider.value = mE.Volume;
        Debug.Log("불러온 값 : " + volumeSlider.value);
        volume.volume = volumeSlider.value;
        volumeText.text = ((int)(volumeSlider.value * 100)).ToString();
        volumeSlider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        //volumeSlider.onValueChanged.AddListener(delegate { SaveData<MenuEntity>(ref mE, MenuEntityPath, fileName); });
        Debug.Log("AddListener End");
    }

    public void ValueChangeCheck()
    {
        Debug.Log(volumeSlider.value);
        mE.Volume = volumeSlider.value;
        volume.volume = volumeSlider.value;
        volumeText.text = ((int)(volumeSlider.value * 100)).ToString();
        SaveData<MenuEntity>(ref mE, MenuEntityPath, fileName);
    }

    #region save&load
    // 제네릭 함수로 변환
    public static void LoadData<T>(ref T data,string filePath,string fileName)
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

    public static void SaveData<T>(ref T data, string filePath,string fileName)
    {
        Debug.Log("저장을 실행합니다.");
        File.WriteAllText(filePath+fileName, JsonUtility.ToJson(data));
        //File.WriteAllText(filePath, JsonConvert.SerializeObject(data));
        Debug.Log("저장이 완료되었습니다.");
        Debug.Log("저장값 : " + data);
    }
    #endregion
}

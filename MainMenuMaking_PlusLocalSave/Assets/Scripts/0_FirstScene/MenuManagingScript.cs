using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using Newtonsoft.Json;

[System.Serializable]
public struct MenuEntity
{
    public float Volume;
};

public struct PathEntity
{
    public string savePath;
}

[System.Serializable]
public struct InputEntity
{
    public Dictionary<KeyAction, KeyCode> keySet; // 기능과 KeyCode 연결
}

public class MenuManagingScript : MonoBehaviour
{
    string MenuEntityPath;
    string OptionfileName = "\\OptionSave.json";
    string KeyfileName = "\\KeySave.json";
    public MenuEntity mE = new MenuEntity();
    public PathEntity pE = new PathEntity();
    public InputEntity Ie = new InputEntity();

    public Slider volumeSlider;
    public Text volumeText;
    AudioSource Mainvolume;


    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        InitInputEntity(ref Ie);
        Mainvolume = GetComponent<AudioSource>();

        MenuEntityPath = Application.dataPath + "\\Saves";
        mE.Volume = volumeSlider.value;

        if (!Directory.Exists(MenuEntityPath))
        {
            Debug.Log("파일경로가 없습니다. 경로를 새로 만듭니다.");
            Directory.CreateDirectory(MenuEntityPath);
        }
        LoadData<MenuEntity>(ref mE, MenuEntityPath, OptionfileName);
        LoadData<InputEntity>(ref Ie, MenuEntityPath, KeyfileName);
        volumeSlider.value = mE.Volume;
        Mainvolume.volume = volumeSlider.value;
        volumeText.text = ((int)(volumeSlider.value * 100)).ToString();
        volumeSlider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });

    }

    private void Update()
    {
    }

    public void ValueChangeCheck() // 슬라이더 바 관련 저장 변화
    {
        Debug.Log(volumeSlider.value);
        mE.Volume = volumeSlider.value;
        Mainvolume.volume = volumeSlider.value;
        volumeText.text = ((int)(volumeSlider.value * 100)).ToString();
        SaveData<MenuEntity>(ref mE, MenuEntityPath, OptionfileName);
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
            //readData = JsonUtility.FromJson<T>(readJson); // readJson string을 구조체로 변환한다.
            readData = JsonConvert.DeserializeObject<T>(readJson);
            Debug.Log("데이터를 읽었습니다. 진행사항을 불러옵니다.");

            data = readData; // 읽은 데이터를 데이터에 집어넣는다.
        }
        catch (Exception e)
        {
            Debug.Log("error :: " + e);
            Debug.Log("경로에 파일이 없습니다. 초기 파일을 생성합니다.");
            SaveData(ref data, filePath, fileName);
        }
    }

    public static void SaveData<T>(ref T data, string filePath, string fileName)
    {
        Debug.Log("저장을 실행합니다.");
        //File.WriteAllText(filePath + fileName, JsonUtility.ToJson(data));
        File.WriteAllText(filePath + fileName, JsonConvert.SerializeObject(data));
        Debug.Log("저장이 완료되었습니다.");
        Debug.Log("저장값 : " + data);
    }
    #endregion


    public void InitInputEntity(ref InputEntity Ie)
    {
        Dictionary<KeyAction, KeyCode> keySet = new Dictionary<KeyAction, KeyCode>(); // 기능과 KeyCode 연결
        KeyAction keyact;
        /*
         * key값 : 게임내의 각 기능 , value : 해당 기능과 매핑된 키코드 또는 KeyCode.None;
         *  public enum KeyAction
         *  {
         *      Up,
         *      Down,
         *      Left,
         *      Right,

         *      Jump,
         *      Dash,
         *
         *      KeyCount
         *  };
         */
        KeyCode[] key = new KeyCode[] { KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.Space, KeyCode.Z };
        // 상하좌우키 입력
        for(int i=0;i<(int)KeyAction.KeyCount;i++)
        {
            keySet.Add((KeyAction)i, key[i]);
        }
        Ie.keySet=keySet;
    }



    public void keyChange(ref InputEntity Ie, KeyCode key, string func)
    {
        // 기존 기능의 키코드 초기화
        
    }
}

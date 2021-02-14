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

[System.Serializable]
public struct InputEntity
{
    public Dictionary<KeyCode, bool> isKeyOn;    // KeyCode가 할당 되어있는지를 확인
    public Dictionary<KeyCode, string> keySet;    // KeyCode와 기능 연결
    public Dictionary<string, KeyCode> keySetRev; // 기능과 KeyCode 연결
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
    AudioSource volume;


    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        InitInputEntity(ref Ie);
        volume = GetComponent<AudioSource>();

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
        volume.volume = volumeSlider.value;
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
        volume.volume = volumeSlider.value;
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
            readData = JsonUtility.FromJson<T>(readJson); // readJson string을 구조체로 변환한다.
                                                          //readData = JsonConvert.DeserializeObject<T>(readJson);
            Debug.Log("데이터를 읽었습니다. 진행사항을 불러옵니다.");

            data = readData; // 읽은 데이터를 데이터에 집어넣는다.
        }
        catch (Exception e)
        {
            Debug.Log("error :: " + e);
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
        Debug.Log("저장이 완료되었습니다.");
        Debug.Log("저장값 : " + data);
    }
    #endregion


    public void InitInputEntity(ref InputEntity Ie)
    {
        Dictionary<KeyCode, bool> isKeyOn = new Dictionary<KeyCode, bool>();    // KeyCode가 할당 되어있는지를 확인
        /*
         * 키코드가 매핑되어 있는지를 확인
         */
        Dictionary<KeyCode,string> keySet = new Dictionary<KeyCode, string>(); // 기능과 KeyCode 연결
        /*
         * key값 : 각 키코드 , value : 키코드와 매핑된 기능, 또는 "None";
         */
        Dictionary<string, KeyCode> keySetRev = new Dictionary<string, KeyCode>(); // 기능과 KeyCode 연결
        /*
         * key값 : 게임내의 각 기능 , value : 해당 기능과 매핑된 키코드 또는 KeyCode.None;
         */

        keySet.Add(KeyCode.None, " - ");
        
        // 상하좌우키 입력
        isKeyOn.Add(KeyCode.LeftArrow, true);
        isKeyOn.Add(KeyCode.RightArrow, true);
        isKeyOn.Add(KeyCode.UpArrow, true);
        isKeyOn.Add(KeyCode.DownArrow, true);

        keySet.Add(KeyCode.LeftArrow, "leftMove");
        keySet.Add(KeyCode.RightArrow, "rightMove");
        keySet.Add(KeyCode.UpArrow,"upMove");
        keySet.Add(KeyCode.DownArrow, "downMove");

        keySetRev.Add("leftMove",KeyCode.LeftArrow );
        keySetRev.Add("rightMove",KeyCode.RightArrow);
        keySetRev.Add("upMove", KeyCode.UpArrow);
        keySetRev.Add("downMove",KeyCode.DownArrow);

        //점프키 입력
        isKeyOn.Add(KeyCode.Space, true);
        isKeyOn.Add(KeyCode.Z, true);

        keySet.Add(KeyCode.Space, "jump");
        keySet.Add(KeyCode.Z, "dash");

        keySetRev.Add("jump", KeyCode.Space);
        keySetRev.Add("dash", KeyCode.Z);

        Ie.keySet = keySet;
        Ie.keySetRev = keySetRev;
        Ie.isKeyOn = isKeyOn;
    }



    public void keyChange(ref InputEntity Ie, KeyCode key, string func)
    {
        // 기존 기능의 키코드 초기화
        if (Ie.keySetRev[func] != KeyCode.None)
        {
            Ie.isKeyOn[Ie.keySetRev[func]] = false;
            Ie.keySet[Ie.keySetRev[func]] = "None";
        }

        try
        {    
            if(Ie.isKeyOn[key])// 이미 해당 키가 다른 기능을 매핑한 상태라면
            {
                Ie.keySetRev[Ie.keySet[key]] = KeyCode.None; // 해당 키가 이전에 가지고 있던 기능의 키코드를 지운다.                
            }
            Ie.keySet[key] = func;
            Ie.keySetRev[func] = key;// 해당 키코드엔 새로운 기능을 이어준다.
        }
        catch(Exception e)
        {
            // 오류가 있다는 것은, 아직 추가가 안되어있다는 뜻이다.
            Ie.isKeyOn.Add(key, true); // 연결되어있는 키값을 추가해준다.
            Ie.keySet.Add(key, func);

            if (Ie.isKeyOn[key])// 이미 해당 키가 다른 기능을 매핑한 상태라면
            {
                Ie.isKeyOn[Ie.keySetRev[Ie.keySet[key]]] = false;
                Ie.keySetRev[Ie.keySet[key]] = KeyCode.None; // 해당 키가 이전에 가지고 있던 기능의 키코드를 지운다.
            }
            Ie.keySet[key] = func;
            Ie.keySetRev[func] = key;// 해당 키코드엔 새로운 기능을 이어준다.
        }
    }
}

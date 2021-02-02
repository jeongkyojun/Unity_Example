using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using Newtonsoft.Json;

[Serializable]
struct playerData
{
    public Vector3 StartPosition;
    public float FloatSave;
    public string StringSave;
    public List<int> ListIntSave;
    public List<List<int>> ListListIntSave;
    public Dictionary<int, string> DicSave;
    public Hashtable hashSave;
};

public class PlayerMove : MonoBehaviour
{
    Rigidbody playerRigidbody;
    InputManager IM;

    playerData data = new playerData();

    string filePath;

    public float speed = 5f;
    public float StartTime;
    
    // Start is called before the first frame update
    void Start()
    {
        // file 경로 설정
        filePath = Application.dataPath + "\\Saves\\PlayerSave.json";

        playerRigidbody = GetComponent<Rigidbody>();
        IM = GetComponent<InputManager>();

        StartTime = Time.time;

        InputData(ref data);

        LoadData(ref data, filePath);
    }

    // Update is called once per frame
    void Update()
    {
        InputManager();
        //Move();
    }

    #region 입력 관련

    

    void InputManager()
    {
        

        

    }
    #endregion

    #region 이동관련
    void Move()
    {
        Vector3 move_Pos = Vector3.right * IM.c.move * speed;

        playerRigidbody.velocity = move_Pos;
    }

    void Jump()
    {

    }
    #endregion

    #region Save&Load

    void InputData(ref playerData data)
    {
        List<int> listtmp = new List<int>();
        List<List<int>> listlisttmp = new List<List<int>>();
        Dictionary<int, string> dictmp = new Dictionary<int, string>();
        Hashtable hashtmp = new Hashtable();

        
        data.StartPosition = transform.position;
        data.FloatSave = 0.5f;
        data.StringSave = "SaveTest";
        data.ListIntSave = listtmp;
        data.ListIntSave.Add(14);

        data.ListListIntSave = listlisttmp;
        data.ListListIntSave.Add(listtmp);

        data.DicSave = dictmp;
        data.DicSave.Add(1, "saveTest");

        data.hashSave = hashtmp;
        data.hashSave["hash"]=1;
    }

    // 제네릭 함수로 변환
    public static void LoadData<T>(ref T data,string filePath)
    {
        Debug.Log("데이터를 로드합니다.");
        string readJson;
        T readData;
        if (Directory.Exists(filePath))
        {
            readJson = File.ReadAllText(filePath); // 데이터를 읽어서 readJson에 넣는다.
            //readData = JsonUtility.FromJson<T>(readJson); // readJson string을 구조체로 변환한다.
            readData = JsonConvert.DeserializeObject<T>(readJson);
            Debug.Log("데이터를 읽었습니다. 진행사항을 불러옵니다.");

            data = readData; // 읽은 데이터를 데이터에 집어넣는다.
        }
        //catch (Exception e)
        else
        {
            //Debug.Log("error :: " + e);
            Debug.Log("경로에 파일이 없습니다. 빈 파일을 생성합니다.");
            SaveData(ref data, filePath);
        }
    }

    public static void SaveData<T>(ref T data, string filePath)
    {
        Debug.Log("저장을 실행합니다.");
        //File.WriteAllText(filePath, JsonUtility.ToJson(data));

        File.WriteAllText(filePath, JsonConvert.SerializeObject(data));
        Debug.Log("저장이 완료되었습니다.");
    }

    #endregion
}

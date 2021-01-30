using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

struct playerData
{
    public Vector3 StartPosition;
    public float PlayTime;
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
        playerRigidbody = GetComponent<Rigidbody>();
        IM = GetComponent<InputManager>();

        StartTime = Time.time;
        filePath = Application.dataPath + "\\Saves\\PlayerSave.json";
        LoadData();
    }

    // Update is called once per frame
    void Update()
    {
        InputManager();
        Move();
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
    void LoadData()
    {
        Debug.Log("readJson and data 선언");
        string readJson;
        playerData readData;
        try
        {
            //Debug.Log("try문 실행, readJson으로 filePath 읽어오기");
            //Debug.Log("FilePath : " + filePath);
            
            readJson = File.ReadAllText(filePath);
            readData = JsonUtility.FromJson<playerData>(readJson);
            
            //Debug.Log("불러오기 성공");

            //Debug.Log("load space : " + readData.KeyName["jump"]);

            data = readData;
            //data.KeyName = readData.KeyName;
            
            transform.position = data.StartPosition; // 현재 위치를 저장된 위치로 이동
            StartTime = Time.time; // 시간 기준을 불러온 직후로 설정
        }
        catch (Exception e)
        {
            Debug.Log("error :: " + e);
            Debug.Log("경로에 파일이 없습니다. 빈 파일을 생성합니다.");
            data.StartPosition = transform.position;
            data.PlayTime = 0;
            SaveData(data);
        }
    }

    void SaveData(playerData playerdata)
    {
        data.PlayTime += Time.time - StartTime;
        StartTime = Time.time;
        Debug.Log("현재 진행 사항을 저장합니다.");
        File.WriteAllText(filePath, JsonUtility.ToJson(playerdata));
        Debug.Log(JsonUtility.ToJson(playerdata));
        Debug.Log("저장이 완료되었습니다.");
    }

    #endregion
}

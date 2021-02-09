using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.IO;
using Newtonsoft.Json;

public struct GameEntity
{
    public int level;
    public int maxHp;
    public int hp;
    public Vector3 playerPosition;
    public int play_h;
    public int play_m;
    public int play_s;
    public int start_hour;
    public int start_min;
    public int start_sec;
};
public class GameManager : MonoBehaviour
{
    public GameObject player;
    public TextMeshProUGUI levelText,hpText,timeText;
    MenuManagingScript menuScript;
    public GameEntity gE = new GameEntity();
    public string PlayerFileName = "\\playerSave.json";

    public float second=0f;
    // Start is called before the first frame update
    void Start()
    {
        menuScript = FindObjectOfType<MenuManagingScript>();
        Debug.Log(menuScript.mE.Volume);
        Debug.Log(menuScript.pE.savePath);
        InitEntity(ref gE,player);
        LoadData<GameEntity>(ref gE, menuScript.pE.savePath, PlayerFileName);
    }

    // Update is called once per frame
    void Update()
    {
        second += Time.deltaTime;
        if (second >= 1)
        {
            gE.play_s ++;
            second = 0f;
        }
        if(gE.play_s>60)
        { 
            gE.play_m++;
            gE.play_s -= 60;
        }
        if(gE.play_m>60)
        {
            gE.play_h++;
            gE.play_m -= 60;
        }

        timeText.text = "PlayTime : "+gE.play_h.ToString()
            +" : "+gE.play_m.ToString()
            +" : "+gE.play_s.ToString();
        levelText.text = "level : "+gE.level.ToString();
        hpText.text = "hp : "+gE.hp.ToString()+" / "+gE.maxHp.ToString();
    }

    static void InitEntity(ref GameEntity gameEntity,GameObject player)
    {
        gameEntity.level = 1;
        gameEntity.maxHp = 10;
        gameEntity.hp = 10;
        gameEntity.playerPosition = player.transform.position;
        gameEntity.start_hour = DateTime.Now.Hour;
        gameEntity.start_min = DateTime.Now.Minute;
        gameEntity.start_sec = DateTime.Now.Second;
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
            Debug.Log("경로에 파일이 없습니다. 빈 파일을 생성합니다.");
            SaveData(ref data, filePath, fileName);
        }
    }

    public static void SaveData<T>(ref T data, string filePath, string fileName)
    {
        Debug.Log("저장을 실행합니다.");
        File.WriteAllText(filePath + fileName, JsonUtility.ToJson(data));
        //File.WriteAllText(filePath+fileName, JsonConvert.SerializeObject(data));
        Debug.Log("저장이 완료되었습니다.");
        Debug.Log("저장값 : " + data);
    }
    #endregion
}

using UnityEngine;
using System.IO;
using System;

public class SaveLoadScript
{
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

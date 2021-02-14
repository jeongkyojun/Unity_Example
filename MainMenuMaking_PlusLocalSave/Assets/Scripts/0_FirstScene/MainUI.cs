using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 메인, 옵션, 볼륨, 로드
// 메인 - 옵션, 볼륨, 로드
public enum BtnType
{
    New,    // 시작하기
    Load,   // 불러오기
    Option, // 옵션
    Sound,  // 소리
    Back,   // 뒤로가기
    End,    // 종료
    KeySet, // 키 셋팅
    BtnTypCnt // 버튼 타입 숫자
}

public enum SaveLoad
{
    Save1,
    Save2,
    Save3,
    Save4,
    Save5,
    SaveLoadCnt // 세이브 칸 숫자
}

public class MainUI : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

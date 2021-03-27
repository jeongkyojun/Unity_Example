using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Enviornment
{
    Normal = 0,
    Water = 1,
    Mountain = 2,
}

public enum Object
{
    Forest = 0,
    Rock = 1,

}

public enum Monster
{
    //숲
    Slime, // 슬라임
    Goblin, // 고블린
    Owk, // 오크
    
    //불
    FenFire, // 도깨비불
    Imp, // 임프
    Burndit, // 불탄산적

    //얼음
    FrostGiant,//서리거인


    //바람
    Vulture, // 대머리수리
    Ghost,  // 유령


    //그외
    Bandit, // 산적떼  

    ElementGiant, // 원소거인
}

public class Environment : MonoBehaviour
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

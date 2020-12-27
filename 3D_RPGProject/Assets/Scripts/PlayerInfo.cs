using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    [Header("이동관련 속성")]
    public float MoveSpeed = 2.0f;// 걷는 속도
    public float RunSpeed = 3.5f;// 뛰는 속도
    public float DirectionRotateSpeed = 100.0f;// 회전 속도
    public float BodyRotateSpeed = 2.0f;// 몸체 회전 속도
    [Range(0.01f, 5.0f)]
    public float VelocityChangeSpeed = 0.1f;//속도 변화량

    [Header("속도 및 방향 관련")]
    public Vector3 CurrentVelocity = Vector3.zero; // 현재 속도
    public Vector3 MoveDirection = Vector3.zero; //이동 방향

    [Header("캐릭터 컨트롤러")]
    public CharacterController myCharacterController = null; // 캐릭터 컨트롤러
    public CollisionFlags collisionFlags = CollisionFlags.None; // 콜리젼 플래그

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

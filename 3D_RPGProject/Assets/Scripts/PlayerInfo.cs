using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    [Header("이동관련 속성")]
    [Tooltip("기본이동속도")]
    public float MoveSpeed = 2.0f;// 걷는 속도
    [Tooltip("달리기이동속도")]
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
        // 컴포넌트 연결
        myCharacterController = GetComponent<CharacterController>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 이동관련함수
    /// </summary>
    void Move()
    {
        //MainCamera 게임 오브젝트의 트랜스폼 컴포넌트
        Transform CameraTransform = Camera.main.transform;
        // 카메라가 바라보는 방향이 월드상에서는 어떤 방향인지 얻는다.
        Vector3 forward = CameraTransform.TransformDirection(Vector3.forward);
        forward.y = 0.0f;
        Vector3 right = new Vector3(forward.z, 0.0f, -forward.x);

        float vertical = Input.GetAxis("vertical"); // 위, 아래, w, s (-1~ 1)
        float horizontal = Input.GetAxis("Horizontal"); // 키보드의 좌, 우, a, d (-1~1)

        //우리가 이동하고자 하는 방향
        Vector3 targetDirection = horizontal * right + vertical * forward;
    }

    void Jump()
    {

    }
}

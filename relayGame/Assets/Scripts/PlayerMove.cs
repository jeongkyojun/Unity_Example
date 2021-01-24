using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // 상태 관련 변수
    float speed = 8f;
    float jumpPower = 10f;
    float pushSpeed = 3f;
    float nowspeed;

    // 입력 관련 변수
    float move;
    bool jump=false, push=false; // 점프상태 확인, 물체 밀거나 당기는것 확인
    bool jumpKey = false;
    // 방향 기준 변수
    Vector3 right = Vector3.right;
    Vector3 up = Vector3.up;

    // 행동 상태 관련 변수
    bool isJump = false, isPush = false; // 점프중인지, 미는 중인지 확인
    bool isGrounded = false;

    int jumpCnt = 0, maxJumpCnt = 1; // 최대 점프 횟수

    // 리지드 바디 값
    Rigidbody playerRigidbody;

    // Start is called before the first frame update
    void Start()
    {
        // 리지드바디 겟 컴퍼넌트
        playerRigidbody = GetComponent<Rigidbody>();
        nowspeed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        InputManaging();

        Move();

        Jump();
    }

    void InputManaging()
    {
        //입력 감지 함수

        move = Input.GetAxis("Horizontal"); // 좌, 우 화살표

        jump = Input.GetKeyDown("space");


        if (Input.GetKeyDown("space")) // 좌측 시프트 키
            jumpKey = true;
        if (Input.GetKeyUp("space"))
            jumpKey = false;

        if (Input.GetKeyDown("left shift")) // 좌측 시프트 키
            push = true;
        if (Input.GetKeyUp("left shift"))
            push = false;
    }

    void Move()
    {
        Vector3 movedir = right * nowspeed * move;

        // 움직일 경우, 플레이어 리지드 바디 의 x축 속도를 이동 속도로 부여
        playerRigidbody.velocity = new Vector3(movedir.x,playerRigidbody.velocity.y,0);
    }

    void Jump()
    {
        Vector3 jumpdir = up * jumpPower;

        if (jump && jumpCnt<maxJumpCnt) // 스페이스를 눌렀고, 점프카운트가 최대 가능횟수를 넘지 않았을 때 
        {
            jumpCnt++; // 점프카운트 1 증가

            playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, jumpdir.y, 0); // y축 속도 부여
            isJump = true; // 점프상태 활성화
        }

        if(isJump && jumpKey) // 점프중이고, 점프키를 지속적으로 누르고 있는 중 일때
        {
            playerRigidbody.velocity += jumpdir / 4 * Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.contacts[0].normal.y > 0.7)
        {
            jumpCnt = 0;
            isJump = false;
            isGrounded = true;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.contacts[0].normal.y < 0.7&& collision.collider.tag == "pushObj")
        {
            if (push)
            {
                isPush = true;
                nowspeed = pushSpeed;
            }
            else
            {
                isPush = false;
                nowspeed = speed;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if(collision.collider.tag =="pushObj")
        {
            isPush = false;
            nowspeed = speed;
            Debug.Log("out!");
        }
    }
}

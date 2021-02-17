using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    MenuManagingScript menuScript;


    Rigidbody playerRigidbody;

    bool MoveLock = false, startLock = true;
    bool isGrounded = false;

    bool isMove,isLeft,isRight,isJump;
    
    float coyote_jump=0;
    int maxjumpCnt = 1, jumpCnt = 0;

    float maxspeed = 3f, jumpPower = 5f;
    float minlen = 10f;
    // Start is called before the first frame update
    void Start()
    {
        menuScript = FindObjectOfType<MenuManagingScript>();

        playerRigidbody = GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void Update()
    {
        IsPlayerGrounded(); // 땅에 닿았는지 여부 파악
        OnPlayerInputManagement(); // 입력 변수 확인
        if (!startLock)
        {
            OnPlayerMove(); // 플레이어 좌 우 움직임
            OnPlayerJump();
        }
    }

    private void OnPlayerInputManagement()
    {
        isLeft = Input.GetKey(menuScript.Ie.keySet[KeyAction.Left]);
        isRight = Input.GetKey(menuScript.Ie.keySet[KeyAction.Right]);

        if (Input.GetKey(menuScript.Ie.keySet[KeyAction.Jump]))
        {
            Debug.Log("jump On!");
            isJump = true;
            coyote_jump = Time.time;
        }
        else
        {
            if(Time.time - coyote_jump >= 0.1) // 코요테타임 : 0.1초
            {
                isJump = false;
            }
        }

        if (isLeft || isRight)
            isMove = true;
        if (!isLeft && isRight)
            isMove = false;
    }
    private void OnPlayerMove()
    {
        if(isMove)//좌 우 버튼을 눌렀을때
        {
            if(!MoveLock) // 특정한 조건을 수행하고 있지 않는다면 
            {
                if (isLeft)
                    Debug.Log("Left");
                if (isRight)
                    Debug.Log("Right");
                if (playerRigidbody.velocity.x < maxspeed && playerRigidbody.velocity.x> -1*maxspeed)
                    playerRigidbody.velocity += ((isLeft?-1:0)+(isRight?1:0)) * Vector3.right * maxspeed / 4;
            }
        }
        else
        {
            if (playerRigidbody.velocity.x >= 0.1 || playerRigidbody.velocity.x <= -0.1)
                playerRigidbody.velocity /= 3f;
        }
    }
    private void OnPlayerJump()
    {
        if(isJump&&jumpCnt<maxjumpCnt)
        {
            playerRigidbody.velocity += Vector3.up * jumpPower;
            jumpCnt++;
        }
    }

    private void IsPlayerGrounded()
    {
        RaycastHit ray;

        if(Physics.Raycast(transform.position,-Vector3.up,out ray,minlen))
        {
            if (minlen > ray.distance)
                minlen = ray.distance+0.1f;

            isGrounded = true;
            jumpCnt = 0;
        }
        else
        {
            isGrounded = false;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        startLock = false;
    }
}

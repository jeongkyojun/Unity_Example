using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playermove : MonoBehaviour
{
    private Rigidbody2D rigidbodyPlayer;
    private PlayerInput playerInput;



    private bool isGround = false; // 땅에 닿아있는가
    private bool isJump = false;   // 점프 중인가
    private bool isDash = false;   // 달리는 중인가
    private bool isMove = true; // 이동가능한 상태인가


    private float LeftRight=1; // 오른쪽 시야 = 1, 왼쪽 시야  = -1
    private float dashTime;

    private int collisionEnterCnt = 0;
    //[Header("이동 관련")]
    //[Tooltip("이동 속도")]
    private float speed = 5f; // 이동속도

    private float dashSpeed = 10f;
    private int dashCnt = 0; // 대시 횟수

    private float jumpPower = 10f; // 점프 정도
    private int jumpCnt = 0; // 점프횟수

    // Start is called before the first frame update
    void Start()
    {
        // GetComponent 사용
        rigidbodyPlayer = GetComponent<Rigidbody2D>(); // 리지드바디 받기
        playerInput = GetComponent<PlayerInput>(); // 플레이어입력 받기
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(rigidbody2D.velocity.x + " , " + rigidbody2D.velocity.y+"."+isGround+" , "+ isJump);
        SetupDirection(); // 보는 방향 설정
        Move(); // 이동(+ 대쉬)
        Jump(); // 점프
        Dash(); // 대쉬
    }

    void LateUpdate()
    {
        //Debug.Log("Position : " + transform.position);
    }

    void Dash()
    {
        if(Input.GetKeyDown(KeyCode.Z)&&dashCnt < 4)
        {
            dashTime = Time.time;
            isDash = true;
            isMove = false;
            dashCnt++;
        }

        if (rigidbodyPlayer.velocity.x == 0)
        {
            isMove = true;
            dashCnt = 0;
        }
    }


    void Jump()
    {
        if(jumpCnt <2 && Input.GetKeyDown(KeyCode.Space))
        {
            rigidbodyPlayer.velocity = new Vector2(rigidbodyPlayer.velocity.x, jumpPower);
            jumpCnt++;
        }
    }

    void Move()
    {
        Vector2 right = new Vector2(1f, 0.0f);
        Vector2 up = new Vector2(0.0f, 1f);
        if (isDash) // 대시일때 -> 속도를 부여한다.
        {
            Debug.Log("Run!");
            isDash = false;

            //급정거 기능 ( 대쉬 중이고, 진행방향과 반대방향을 보면서 대시하는 경우 )
            if (!isMove && LeftRight * rigidbodyPlayer.velocity.x < 0)
            {
                Debug.Log("Stop!!!!!!!!!!!!!!!!!!");
                //급정거 애니메이션 설정
                rigidbodyPlayer.velocity += new Vector2(-rigidbodyPlayer.velocity.x,0f); // 속도조절
            }

            else
            {
                Vector2 vec = LeftRight * right * dashSpeed;
                rigidbodyPlayer.velocity += vec;
                if (rigidbodyPlayer.velocity.x > dashSpeed * 2f)
                    rigidbodyPlayer.velocity = new Vector2(dashSpeed * 2f, rigidbodyPlayer.velocity.y); // 속도조절
            }
        }
        else if (isMove) // 대시가 아니고, 이동상태일때 -> 이동한다.
        {
            Vector3 vec = playerInput.rotate * right * Time.deltaTime * speed;
            transform.position += vec;
        }
    }


    void OnCollisionExit2D()
    {
        collisionEnterCnt--;

        if (collisionEnterCnt == 0) // 접촉 면이 없을 경우
        {
            isGround = false;
        }
        Debug.Log("Exit");
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        collisionEnterCnt++;
        Debug.Log("OnGround");
        Debug.Log(other.contacts[0].normal);
        if (other.contacts[0].normal.y >= 0.7)
        {
            isGround = true;
            jumpCnt = 0;
        }

        if (collisionEnterCnt!=0&&!isGround)
        {
            // 바닥에 닿지 않았는데 어딘가에 닿아있다 = 벽에 닿아있다
        }
    }

    void SetupDirection()
    {
        if (playerInput.rotate < 0)
            LeftRight = -1;
        else if (playerInput.rotate > 0)
            LeftRight = 1;
    }
}

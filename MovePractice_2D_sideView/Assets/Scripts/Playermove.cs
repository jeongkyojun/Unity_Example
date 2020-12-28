using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playermove : MonoBehaviour
{
    private Rigidbody2D rigidbody2D;

    private PlayerInput playerInput;

    private bool isDash, isGround, isJump;

    private float RecentJumptime,LeftRight;

    [Header("이동 관련")]
    [Tooltip("이동 속도")]
    public float speed;
    [Tooltip("점프 힘")]
    public float jumpPower;
    // Start is called before the first frame update
    void Start()
    {
        speed = 5f;
        jumpPower = 5f;
        LeftRight = 1;
        isGround = true;
        isJump = false;
        isDash = false;
        rigidbody2D = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(rigidbody2D.velocity.x + " , " + rigidbody2D.velocity.y+"."+isGround+" , "+ isJump);
        if (playerInput.rotate < 0)
            LeftRight = -1;
        else if (playerInput.rotate > 0)
            LeftRight = 1;
        
        Move();
        Jump();
    }

    void Jump()
    {
        if (isJump && Time.time - RecentJumptime > 1)
        {
            Debug.Log("JumpOff");
            isGround = false;
            isJump = false;
        }

        if (Input.GetKey(KeyCode.Space) && isGround)
        {
            if (!isJump)
            {
                RecentJumptime = Time.time;
                isJump = true;
                //Debug.Log(Input.GetAxis("Jump"));
                rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, jumpPower);
            }
            else
            {
                rigidbody2D.velocity += (new Vector2(0, 10 * Time.deltaTime));
            }
        }
    }

    void Move()
    {
        Vector2 right = new Vector2(1f, 0.0f);
        Vector2 up = new Vector2(0.0f, 1f);
        Vector3 vec = playerInput.rotate * right*Time.deltaTime*speed;
        transform.position += vec;
    }

    void OnTriggerEnter2D()
    {
        Debug.Log("OnGround");
        isGround = true;
        isJump = false;
    }
}

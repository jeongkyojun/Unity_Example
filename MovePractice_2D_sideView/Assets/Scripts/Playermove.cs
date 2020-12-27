using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playermove : MonoBehaviour
{
    private Rigidbody2D rigidbody2D;

    private PlayerInput playerInput;

    private float speed = 10f,jumpPower = 10f,RecentJumptime;

    private bool isGround,isJump,isDash;

    private int LeftRight;

    // Start is called before the first frame update
    void Start()
    {
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
        Dash();
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
        /*
        if (transform.position.x + playerInput.rotate * Time.deltaTime * speed < 100 && transform.position.x + playerInput.rotate * Time.deltaTime * speed > -100)
            transform.Translate(new Vector2(playerInput.rotate * Time.deltaTime * speed, 0));
        else
        {
            set_return();
        }
        */
        if (playerInput.rotate!=0)
           rigidbody2D.velocity = new Vector2(speed*playerInput.rotate, 0);
        else
        {
            rigidbody2D.velocity = new Vector2(0f,rigidbody2D.velocity.y);
        }
    }

    void Dash()
    {
        float inputTime;
        if (Input.GetKey(KeyCode.Z))
        {
            inputTime = Time.time;
            transform.Translate(new Vector2(speed * 2.5f * Time.deltaTime*LeftRight,0));
        }

    }


    void set_return()
    {
        if (transform.position.x + playerInput.rotate * Time.deltaTime * speed > 100)
            transform.position = new Vector2(-99, transform.position.y);
        else if (transform.position.x + playerInput.rotate * Time.deltaTime * speed < -100)
            transform.position = new Vector2(99, transform.position.y);
    }

    void OnTriggerEnter2D()
    {
        Debug.Log("OnGround");
        isGround = true;
        isJump = false;
    }
}

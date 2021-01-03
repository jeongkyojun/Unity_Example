using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    [Header("이동관련")]
    public float speed = 5f;//이동속도
    public float jumpPower = 5f;

    Rigidbody playerRigidbody;
    SpriteRenderer playerSprite;

    private bool isGrounded = true;
    private int jumpCnt = 2;
    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        playerSprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
    }

    void Move()
    {
            float move = Input.GetAxis("Horizontal");
            Vector3 right = new Vector3(1, 0, 0);
            Vector3 vec = right * speed * Time.deltaTime * move;

            transform.position += vec;
    }

    void Jump()
    {
        if(Input.GetKeyDown(KeyCode.Space)&&jumpCnt<2)
        {
            jumpCnt++;
            isGrounded = false;
            playerRigidbody.velocity = new Vector3(0f, jumpPower, 0f);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.contacts[0].normal.y>0.7)
        {
            Debug.Log("OnGrounded!");
            isGrounded = true;
            jumpCnt = 0;
        }
        if(collision.contacts[0].normal.y>=0&&collision.contacts[0].normal.y<0.7)
        {
            Debug.Log("wall jump!");
            jumpCnt=1;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    [Header("이동관련")]
    public float speed = 5f;//이동속도
    public float jumpPower = 5f;
    public float Gravity = 1;

    Rigidbody playerRigidbody;
    SpriteRenderer playerSprite;

    private float gravitySpeed = 9.8f;
    private bool isGrab = false;
    private bool isGrounded = false;
    private int jumpCnt = 2;

    GameObject LopeHandle;
    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        playerSprite = GetComponent<SpriteRenderer>();
        playerRigidbody.UseGravity = false;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
    }

    void Move()
    {
        Debug.Log("Move");
        float move = Input.GetAxis("Horizontal");
        Vector3 right = new Vector3(1, 0, 0);
        Vector3 vec = right * speed * Time.deltaTime * move;

        transform.position += vec;
    }

    void Jump()
    {
        Debug.Log("Jump");
        if(Input.GetKeyDown(KeyCode.Space)&&jumpCnt<2)
        {
            jumpCnt++;
            isGrounded = false;
            playerRigidbody.velocity = Vector3.up * jumpPower;
        }

        if (!isGrounded)
        {
            playerRigidbody.velocity -= Vector3.up* gravitySpeed * Time.deltaTime * Gravity;
        }
    }

    void Grab()
    {
        if(isGrab)
        {
            playerRigidbody.velocity = new Vector3(0f,0f,0f);
            transform.position = new Vector3(LopeHandle.transform.position.x,LopeHandle.transform.position.y - 0.3f,0f);
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

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("OntriggerEnter : " + other.tag);
        if(other.tag == "LopeHandle")
        {
            isGrab = true;
            LopeHandle = other.gameObject;
            Debug.Log(LopeHandle.transform.position);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMove : MonoBehaviour
{
    Rigidbody rigidbody;
    SpriteRenderer spriteRenderer;
    public int StatusColor = 1;//1 : 빨강, 2 : 주황, 3 : 파랑
    public float speed = 3f;
    public float jumpPower = 4f;

    private bool isJumpPress = false;
    private bool onGround = false;
    private bool onEnterCollision = false;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = new Color(255, 0, 0,255);
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
        Vector3 right = new Vector3(1f, 0f, 0f);

        Vector3 vec = right * move*speed*Time.deltaTime;

        transform.position += vec;
    }

    void Jump()
    {
        if(onGround&&Input.GetKeyDown(KeyCode.Space))
        {
            isJumpPress = true;
            onGround = false;
            rigidbody.velocity += new Vector3(0, jumpPower, 0);
        }

        if(Input.GetKeyUp(KeyCode.Space))
        {
            isJumpPress = false;
        }
        else if(isJumpPress)
        {
            rigidbody.velocity += new Vector3(0, jumpPower / 2 * Time.deltaTime,0);
        }
    }

    void OnCollisionEnter(Collision collider)
    {
        if (!onEnterCollision) // 1회만 부딫힘 판정을 받기위해 설정
        {
            Debug.Log("touch!");
            onEnterCollision = true;
            if (collider.collider.CompareTag("OrangeBtn")) // 접촉 태그가 주황버튼일 경우
            {
                Debug.Log("Orange");
                StatusColor = 2;
                spriteRenderer.color = new Color(1,0.5f,0f,1); // 주황색
            }
            if (collider.collider.CompareTag("BlueBtn")) // 접촉 태그가 파란버튼일 경우
            {
                Debug.Log("Blue");
                StatusColor = 3;
                spriteRenderer.color = new Color(0, 0, 1, 1); // 파랑색
            }
            Vector3 contactVec = Vector3.zero;
        }

        if (collider.contacts[0].point.y<transform.position.y)
        {
            // 접촉 부분이 원의 중심보다 작을 경우 바닥에 닿은 것으로 취급
            onGround = true; 
        }
    }

    void OnCollisionExit()
    {
        onEnterCollision = false;
    }
}

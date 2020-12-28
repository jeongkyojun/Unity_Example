using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMove : MonoBehaviour
{
    Vector3 recent_velocity; // 최근 속력 확인
    Rigidbody rigidbody;

    private bool isSpace;//스페이스바 누르는지 여부
    private bool isJump; //점프 상태 확인
    private bool isTriggerEnter; // OnTriggerEnter 트리거 발동 여부 확인


    private float jumpInputTime; // 점프키를 최근에 누른 시간

    private int LookDirection; // 보는 방향

    #region 이동 관련 변수
    [Header("이동 관련")]
    [Tooltip("이동 속도")]
    public float speed;
    
    [Tooltip("각도")]
    [Range(30.0f,60.0f)]
    public float rotate;
    
    [Tooltip("각도 변화 속도")]
    public float rotateMoveSpeed;
    
    [Tooltip("각도 증감 방향")]
    public float rotateDirection;

    [Tooltip("점프 거리")]
    public float jumpPower;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();

        // 상태 변수 초기화
        
        //bool
        isJump = false;
        isTriggerEnter = false;

        //int
        LookDirection = 1;
        rotateDirection = 1;

        // 이동 거리및 속력 초기화

        //float(speed)
        rotateMoveSpeed = 0.1f;
        speed = 3f;

        //float(angle)
        rotate = 30f;

        //float(power)
        jumpPower = 10f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isJump)
        {
            Move();
            GetRot();
        }
        if(!isTriggerEnter)
        {
            recent_velocity = rigidbody.velocity;
        }
        Jump();
        
    }
    void Move()
    {
        float right = Input.GetAxis("Horizontal");

        if (right < 0)
            LookDirection = -1;
        if (right > 0)
            LookDirection = 1;

        Vector3 Hor = new Vector3(1f, 0f, 0f);
        Vector3 vec = Hor * right* speed*Time.deltaTime;
        transform.position += vec;
    }
    void GetRot()
    { 
        float up = Input.GetAxis("Vertical");
        if (rotate >= 60.0f)
            rotateDirection = -1;
        if (rotate <= 30.0f)
            rotateDirection = 1;
        if(up !=0)
        {
            rotate += rotateMoveSpeed * rotateDirection;
            Debug.Log("rotation : " + rotate);
        }
    }
    
    void Jump()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("space Input");
            jumpInputTime = Time.time;
            isJump = true;
        }
        if(Input.GetKeyUp(KeyCode.Space))
        {
            float keyinputTime = Time.time - jumpInputTime;
            if (keyinputTime > 1)
                keyinputTime = 1f;
            Debug.Log("space output ( Time : "+keyinputTime+" ) , rotate : "+rotate);
            float power = jumpPower * keyinputTime;
            double angle = rotate * Math.PI / 180;
            Debug.Log("power : " + power+" , move ( "+ LookDirection * power * (float)Math.Cos(angle)+" , "+ power * (float)Math.Sin(angle)+" ) -> "+Math.Tan(angle));
            // 사인 : 높이/빗변 , 코사인 : 밑변 / 빗변
            rigidbody.velocity += new Vector3(LookDirection * power * (float)Math.Cos(angle), power * (float)Math.Sin(angle),0);
            isJump = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!isTriggerEnter)
        {
            Vector3 contactPosition = Vector3.zero;
            Debug.Log("position : " + transform.position);
            foreach (ContactPoint contact in collision.contacts)
                contactPosition += contact.point;
            contactPosition /= collision.contacts.Length;
            Debug.Log("positionAvg : " + contactPosition);
            isTriggerEnter = true;


            if (Math.Abs(contactPosition.y - transform.position.y) > Math.Abs(contactPosition.x - transform.position.x))
            {
                if (contactPosition.y < transform.position.y)
                {
                    Debug.Log("UP");
                    recent_velocity.y *= -1f;
                }
                else if (contactPosition.y > transform.position.y)
                {
                    Debug.Log("DOWN");
                    recent_velocity.y *= -1f;
                }
            }
            else
            {
                if (contactPosition.x < transform.position.x)
                {
                    Debug.Log("Left");
                    recent_velocity.x *= -1f;
                }
                else if (contactPosition.x > transform.position.x)
                {
                    Debug.Log("Right");
                    recent_velocity.x *= -1f;
                }
            }
            recent_velocity /= 2;
            rigidbody.velocity = Vector3.zero;
            rigidbody.velocity += recent_velocity;
        }
    }
    void OnTriggerExit()
    {
        Debug.Log("TriggerExit");
        isTriggerEnter = false;
    }
}

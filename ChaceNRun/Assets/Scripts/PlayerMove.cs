using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed = 4f;
    public float jumpPower = 5f;

    private int jumpCount = 0;

    private bool isGrounded = false;
    private bool isDead = false;

    private Vector3 right = new Vector3(1, 0, 0);

    private float health = 100f;

    Rigidbody rigidbody;
    //Animator animator;
    //AudioSource playerAudio;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        //animator = GetComponent<Animator>();
        //playerAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
            return;
        Move();
        Jump();
    }

    void Move()
    {
        float move = Input.GetAxis("Horizontal");
        Vector3 vec = right * move * speed * Time.deltaTime;
        transform.position += vec;
    }

    void Jump()
    {
        if(Input.GetKeyDown(KeyCode.Space) && jumpCount < 2)
        {
            jumpCount++;
            rigidbody.velocity += new Vector3(0f, jumpPower, 0f);
        }

        if(Input.GetKeyUp(KeyCode.Space))
        {

        }
    }

    void Die()
    {
        //animator.SetTrigger("Die"); // Die 애니메이션 실행
        //playerAudio.clip = deathClip; // 오디오 클립 변경
        //playerAudio.Play(); // 사망 효과음 재생

        rigidbody.velocity = Vector3.zero;

        isDead = true;

        GameManager.instance.OnPlayerDead();

    }

    void OnTriggerEnter(Collider other)
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Hit! ( "+collision.collider.tag+" )");
        if (collision.contacts[0].normal.y > 0.7f)
        {
            isGrounded = true;
            jumpCount = 0;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }

    void Stop(float stopTime)
    {

    }

    public void OnDamage(float damage,Vector3 hitPoint, Vector3 hitnormal)
    {
        /*
          
        if(!isDead)
        {
            playerAudio.PlayOneShot(hitClip); // 사망 효과음 재생
        }

        */
        health -= damage;
        if(health<0 && !isDead)
        {
            Debug.Log("dead!");
            Die();
        }
    }
}

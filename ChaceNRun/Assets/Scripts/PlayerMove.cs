using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed = 4f;
    public float jumpPower = 3f;

    private int jumpCount = 0;

    private bool isGrounded = false;
    private bool isDead = false;

    private Vector3 right = new Vector3(1, 0, 0);

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
        }

        if(Input.GetKeyUp(KeyCode.Space))
        {

        }
    }

    void OnTriggerEnter(Collider other)
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
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
}

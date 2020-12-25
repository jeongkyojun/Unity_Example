using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playermove : MonoBehaviour
{
    private Rigidbody2D rigidbody2D;

    private PlayerInput playerInput;

    private float speed = 3f,jumpPower = 5f;

    private bool isGround;
    // Start is called before the first frame update
    void Start()
    {
        isGround = true;
        rigidbody2D = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
    }

    void Jump()
    {
        if (Input.GetKey(KeyCode.Space) && isGround)
        {
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, jumpPower);
        }
    }
    void Move()
    {
        if (transform.position.x + playerInput.rotate * Time.deltaTime * speed < 100 && transform.position.x + playerInput.rotate * Time.deltaTime * speed > -100)
            transform.position = new Vector2(transform.position.x + playerInput.rotate * Time.deltaTime * speed, transform.position.y);
        else
        {
            set_return();
        }
    }


    void set_return()
    {
        if (transform.position.x + playerInput.rotate * Time.deltaTime * speed < 100)
            transform.position = new Vector2(99, transform.position.y);
        else if (transform.position.x + playerInput.rotate * Time.deltaTime * speed > -100)
            transform.position = new Vector2(-99, transform.position.y);
    }

    void OnTriggerEnter2D()
    {
        Debug.Log("true");
        isGround = true;
    }
    void OnTriggerExit2D()
    {
        Debug.Log("false");
        isGround = false;
    }
}

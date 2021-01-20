using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    PlayerInput playerInput;
    PlayerModel playerModel;

    Vector3 up = Vector3.up;
    Vector3 right = Vector3.right;

    Rigidbody playerRigidBody;

    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerModel = GetComponent<PlayerModel>();
       
        playerRigidBody = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        if (!playerInput.MoveLock)
        {
            Move();
        }
        Jump();
    }

    void Move()
    {
        Vector3 movePosition = right * playerModel.speed * playerInput.moveA * Time.deltaTime;

        playerRigidBody.MovePosition(playerRigidBody.position + movePosition);
    }
       
    void Jump()
    {
        if (playerInput.jumpD && playerModel.jumpCnt<2)
        {
            Debug.Log("Jump!");
            playerModel.recentJumpTime = Time.time;
            playerModel.jumpCnt++;
            playerModel.isJump = true;
            Vector3 jumpPosition = up * playerModel.jumpPower;
            playerRigidBody.velocity = new Vector3(playerRigidBody.velocity.x, jumpPosition.y, 0);
        }

        if (playerInput.jumpU)
            playerModel.isJump = false;

        if(playerModel.isJump && (Time.time - playerModel.recentJumpTime < 1))
        {
            playerRigidBody.velocity += up * Time.deltaTime * playerModel.jumpPower / 2;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if(other.contacts[0].normal.y > 0.7)
        {
            Debug.Log("Grounded!");
            playerModel.isGrounded = true;
            playerModel.jumpCnt = 0;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Dead")
        {
            //Destroy(gameObject);
        }
    }
}

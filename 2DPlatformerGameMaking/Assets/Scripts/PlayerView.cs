using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{

    //애니메이션 및 이동, 물리 설정

    PlayerModel playerModel;
    PlayerInput playerInput;
    Rigidbody playerRigidbody;

    // Start is called before the first frame update
    void Start()
    {
        playerModel = GetComponent<PlayerModel>();
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
    }

    public void Move()
    {
        Vector3 right = Vector3.right;
        Vector3 movement = right * playerInput.move * Time.deltaTime * playerModel.speed;
        playerRigidbody.MovePosition(playerRigidbody.position + movement);
    }

    public void Jump()
    {
        Vector3 up = Vector3.up;
        if(playerInput.jump&&playerModel.jumpCnt<2)
        {
            playerModel.jumpCnt++;
            playerRigidbody.velocity += up * playerModel.jumpPower;
        }
    }
}

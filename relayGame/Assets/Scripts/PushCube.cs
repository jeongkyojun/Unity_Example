using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PushCube : MonoBehaviour
{
    bool isMove = false,isSet = false;

    Rigidbody blockRigidbody;

    float moveSpeed = 3f , move;
    // Start is called before the first frame update
    void Start()
    {
        blockRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("left shift"))
            isMove = true;
        if (Input.GetKeyUp("left shift"))
            isMove = false;

        move = Input.GetAxis("Horizontal");

        if (isSet)
            Move();
    }

    private void Move()
    {
        Vector3 movedir = Vector3.right * moveSpeed * move;

        // 움직일 경우, 플레이어 리지드 바디 의 x축 속도를 이동 속도로 부여
        blockRigidbody.velocity = new Vector3(movedir.x, 0, 0);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (Math.Abs(collision.contacts[0].normal.x) > 0.7)
        {
            if (collision.collider.tag == "Player")
            {
                if (isMove)
                    isSet = true;
                else
                    isSet = false;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.tag == "Player")
            isSet = false;
    }
}

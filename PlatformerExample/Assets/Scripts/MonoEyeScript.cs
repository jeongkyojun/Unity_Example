using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MonoEyeScript : MonoBehaviour
{
    public GameObject eyes;
    Vector3 firstPosition;
    Vector3 playerPosition;

    Ray ray;
    RaycastHit hit;

    float attackTime;
    float attackingTime;
    bool find_player = false;

    bool isActive = false;
    bool isAttack = false;
    float direction = -1f;

    float speed = 10f;
    // Start is called before the first frame update
    void Start()
    {
        firstPosition = eyes.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(isActive)
        {
            Move(playerPosition);
            if (find_player)
            {
                if (Time.time - attackTime > 3)
                {
                    if(!isAttack)
                    {
                        attackingTime = Time.time;
                        isAttack = true;
                        Debug.Log("Attack!");
                    }
                    Attack();
                }
            }
        }
        else
        {
            Move(firstPosition);
        }
    }

    void Move(Vector3 playerPosition)
    {
        if (playerPosition.x < eyes.transform.position.x)
            direction = -1;
        else if (playerPosition.x > eyes.transform.position.x)
            direction = 1;
        else
            direction = 0;

        eyes.transform.position += Vector3.right * direction * speed * Time.deltaTime;

        if (!find_player)
        {
            // find 조건 만족이 아직 안됨, 나중에 수정 필요!
            if ((direction > 0 && eyes.transform.position.x > playerPosition.x) || (direction < 0 && eyes.transform.position.x < playerPosition.x))
            {
                Debug.Log("Find");
                attackTime = Time.time;
                find_player = true;
            }
        }
    }

    void Attack()
    {

        Physics.Raycast(eyes.transform.position, -1 * Vector3.up, out hit , 20f);
        Debug.DrawRay(eyes.transform.position, -1 * Vector3.up * hit.distance, Color.blue);

        if (Time.time - attackingTime > 0.5)
        {
            isAttack = false;
            attackTime = Time.time;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            isActive = true;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
            playerPosition = other.gameObject.transform.position;
    }

    void OnTriggerExit()
    {
        isActive = false;
        find_player = false;
        isAttack = false;
    }
}

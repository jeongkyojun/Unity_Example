using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoEyeScript : MonoBehaviour
{
    public GameObject eyes;
    Vector3 firstPosition;
    Vector3 playerPosition;

    float attackTime;
    bool find_player = false;

    bool isActive = false;
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
                    Attack();
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
        //Debug.Log(eyes.transform.position.x);
        if (!find_player)
        {
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
        attackTime = Time.time;
        Debug.Log("Attack!");
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
    }
}

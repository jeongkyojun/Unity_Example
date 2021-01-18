using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapCaboom : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isSetup = false;
    public bool isStop = false;
    float fallingSpeed = 15f;
    float speed = 5f;
    Vector3 firstPos;
    void Start()
    {
        firstPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(isSetup && !isStop)
        {
            Move();
        }
        else
        {
            Move(firstPos);
        }
    }

    void Move()
    {
        Vector3 set = Vector3.up * -1 * fallingSpeed * Time.deltaTime;

        transform.position += set;
    }

    void Move(Vector3 firstPos)
    {
        if(firstPos.y > transform.position.y)
        {
            transform.position += Vector3.up * speed * Time.deltaTime;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if(other.collider.tag != "Player")
        {
            isStop = true;
        }
    }
}

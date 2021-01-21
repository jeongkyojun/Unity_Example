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

    Rigidbody rigid;
    void Start()
    {
        rigid = GetComponent<Rigidbody>(); // rigid
        firstPos = transform.position; // 처음 위치
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
        else
        {
            isStop = false;
            isSetup = false;
            //gameObject.tag = "Untagged";
        }
    }

    void OnCollisionEnter(Collision other)
    {
        Debug.Log(other.collider.tag);
        if(other.collider.tag != "Player")
        {
            rigid.velocity = Vector3.zero;
            isStop = true;
            gameObject.tag = "Untagged";
        }
    }
}

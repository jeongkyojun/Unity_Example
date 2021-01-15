using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeDownScript : MonoBehaviour
{
    Vector3 firstPosition;
    bool isDown;
    float speed = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        firstPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isDown)
            Move(firstPosition);
    }

    void Move(Vector3 firstPosition)
    {
        if (transform.position.y < firstPosition.y)
            transform.position += Vector3.up * speed * Time.deltaTime;
    }

    void OnCollisionEnter(Collision other)
    {
        if(other.collider.tag == "Player")
        {
            Debug.Log(other.contacts[0].normal.y);
            if(other.contacts[0].normal.y < -0.7f)
            {
                isDown = true;
            }
        }
    }

    void OnCollisionStay(Collision other)
    {
        if(isDown && other.collider.tag == "Player")
        {
            transform.position -= Vector3.up * speed * Time.deltaTime;
        }
    }

    void OnCollisionExit(Collision other)
    {
        isDown = false;
        
    }
}

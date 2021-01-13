using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoStraight : MonoBehaviour
{
    Vector3 startPosition;

    float speed = 10f;
    float boundary;
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        boundary = startPosition.x - 40f;

        Debug.Log("boundary : " + boundary + " , startPosition : " + startPosition);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < boundary)
        {
            transform.position = startPosition;
        }

        transform.position -= Vector3.right * speed * Time.deltaTime;
    }
}

using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CubeMove : MonoBehaviour
{
    public GameObject center;
    float centerDistance; // 길이
    float degree;
    float speed = 0.1f;// 1당 2초에 한바퀴
    float Pi = (float)Math.PI;
    Vector3 centerPosition;

    // Start is called before the first frame update
    void Start()
    {
        centerPosition = center.transform.position;
        centerDistance = (float)Math.Sqrt(Math.Pow(transform.position.x - centerPosition.x, 2) + Math.Pow(transform.position.y - centerPosition.y, 2));
        degree = (float)Math.Asin((transform.position.y - centerPosition.y) / centerDistance);

        Debug.Log("Distance : " + centerDistance + " & Degree : " + degree);
    }

    // Update is called once per frame
    void Update()
    {
        centerPosition = center.transform.position;

        degree += Pi*speed*Time.deltaTime;

        if(degree>360)
        {
            degree -= 360;
        }
        if(degree<0)
        {
            degree += 360;
        }

        transform.position = centerPosition+new Vector3((float)Math.Cos(degree), (float)Math.Sin(degree), 0)*centerDistance;
    }
}

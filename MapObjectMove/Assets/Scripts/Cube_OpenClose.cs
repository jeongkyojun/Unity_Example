using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube_OpenClose : MonoBehaviour
{
    public GameObject cube1, cube2, cube3, cube4; // ++, -+ , -- , +-

    float speed = 1f, distance = 0f, max_distance = 1f;

    float direction = 1f;

    Vector3 moveX, moveY;
    // Start is called before the first frame update
    void Start()
    {
        moveX = Vector3.right; 
        moveY = Vector3.up;
    }

    // Update is called once per frame
    void Update()
    {
        if (distance > max_distance)
            direction = -1f;

        if (distance < 0)
            direction = 1f;

        cube1.transform.position += (moveX + moveY) * speed * Time.deltaTime * direction;
        cube2.transform.position += (-moveX + moveY) * speed * Time.deltaTime * direction;
        cube3.transform.position -= (moveX + moveY) * speed * Time.deltaTime * direction;
        cube4.transform.position += (moveX - moveY) * speed * Time.deltaTime * direction;

        distance += direction * speed * Time.deltaTime;
    }
}

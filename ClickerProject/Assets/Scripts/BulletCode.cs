using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BulletCode : MonoBehaviour
{
    Vector3 dir = Vector3.zero;

    float speed = 10f;
    float dirLen= 0f;

    float startTime;

    Plane GroupPlane = new Plane(Vector3.forward, Vector3.zero);

    Renderer bulletColor;

    // Start is called before the first frame update
    void Start()
    {
        bulletColor = GetComponent<Renderer>();

        bulletColor.material.color = GetColor(gameObject.tag);


        startTime = Time.time;

        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        float rayLength; // ray의 길이를 받는 선
        if (GroupPlane.Raycast(cameraRay, out rayLength)) // z축이 0인 가상의 평면과 교차하는경우
        {
            Vector3 shootingdir = cameraRay.GetPoint(rayLength); // 끝 점을 저장받는다.
            dir = shootingdir - transform.position;
            dirLen = Mathf.Sqrt(Mathf.Pow(dir.x, 2) + Mathf.Pow(dir.y, 2));
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += dir * speed * Time.deltaTime / dirLen;

        if (Time.time - startTime > 5f)
            Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag!="Player")
        {
            collision.collider.tag = gameObject.tag;
            Destroy(gameObject);
        }
    }

    Color GetColor(string tag)
    {
        switch(tag)
        {
            case "Red":
                return Color.red;
            case "Blue":
                return Color.blue;
            case "Yellow":
                return Color.yellow;
            case "Green":
                return Color.green;
            case "Black":
                return Color.black;
            default:
                return Color.white;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class normalBulletCode : MonoBehaviour
{
    Vector3 dir = Vector3.zero;

    //int hitnum=0, maxnum = 1;

    float speed = 10f;
    float dirLen = 0f;

    float startTime;

    Plane GroupPlane = new Plane(Vector3.forward, Vector3.zero);

    Renderer bulletColor;

    // Start is called before the first frame update
    void Start()
    {
        bulletColor = GetComponent<Renderer>();

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
        // 벡터 길이에 따라 속도가 변하지 않게 하기 위해 dirLen을 구해 나눠서 1로 초기화해준다.
        transform.position += dir * speed * Time.deltaTime / dirLen; 
    }

    private void OnCollisionEnter(Collision collision)
    {

        Debug.Log(collision.contacts[0].normal);
        Debug.Log(dir); // dir은 Vector3 자료형 (이동하는 방향)
        float sinT = collision.contacts[0].normal.y;
        float cosT = collision.contacts[0].normal.x;

        float sin2T = 2 * sinT * cosT;
        float cos2T = (cosT * cosT) - (sinT * sinT);

        // 2t-a -> cos(2t-a-PI), sin(2t-a-PI) cosPI = -1, sinPI = 0
        // 즉, cos(2t-a) * -1  , sin(2t-a) * -1 이 된다.

        dir = new Vector3((cos2T * dir.x) + (sin2T * dir.y),
            (sin2T * dir.x) - (cos2T * dir.y),
            0) * -1;

        Debug.Log(dir);
        dirLen = Mathf.Sqrt(Mathf.Pow(dir.x, 2) + Mathf.Pow(dir.y, 2)); // 방향에 따른 속도 측정 기준 dirLen 은 float 자료형
    }
}

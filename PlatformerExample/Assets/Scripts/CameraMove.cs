using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraMove : MonoBehaviour
{
    bool isLeft; // 좌측버튼 인식
    bool isRight; // 우측버튼 인식
    bool isUp; // 위 버튼 인식
    bool isDown; // 아래 버튼 인식

    bool isHorizon; // 좌우 시야 활성화 키 1

    public GameObject player; // 플레이어 연결

    float leftTime, rightTime, upTime, downTime; // 화살표 버튼 누른 시간

    float distance = 10f; // 최대 이동 거리

    //float speed = 8f;

    // Start is called before the first frame update
    void Start()
    {
        // 모두 false로 초기화
        isLeft = false;
        isRight = false;
        isUp = false;
        isDown = false;
        isHorizon = false;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(Input.GetKeyDown("x"))
        {
            isHorizon = true;
        }
        if(Input.GetKeyUp("x"))
        {
            isHorizon = false;
        }

        // 버튼을 누른 경우 true로 변환
        if (Input.GetKeyDown("down")){
            isDown = true;
            downTime = Time.time;
        }
        if (Input.GetKeyDown("left")){
            isLeft = true;
            leftTime = Time.time;
        }
        if (Input.GetKeyDown("up")){
            isUp = true;
            upTime = Time.time;
        }
        if (Input.GetKeyDown("right")){
            isRight = true;
            rightTime = Time.time;
        }

        // 버튼을 뗀 경우 false로 변환
        if (Input.GetKeyUp("down")){
            isDown = false;
        }
        if (Input.GetKeyUp("left")){
            isLeft = false;
        }
        if (Input.GetKeyUp("up")){
            isUp = false;
        }
        if (Input.GetKeyUp("right")){
            isRight = false;
        }

        // Y축 이동
        if (!isUp && isDown && Time.time - downTime > 1)
        {
            MoveY(player.transform.position - Vector3.up * distance);
        }

        if (!isDown && isUp && Time.time - upTime > 1)
        {
            MoveY(player.transform.position + Vector3.up * distance);
        }

        // X축 이동
        if (isHorizon) // x를 함께 누르고 있어야 인식된다.
        {
            if (!isRight && isLeft && Time.time - leftTime > 1)
            {
                MoveX(player.transform.position - Vector3.right * distance);
            }

            if (!isLeft && isRight && Time.time - rightTime > 1)
            {
                MoveX(player.transform.position + Vector3.right * distance);
            }
        }

        if (!isDown && !isUp)
        {
            MoveY(player.transform.position);
        }

        if(!isHorizon || (!isRight && !isLeft))
        {
            MoveX(player.transform.position);
        }

    }

    void MoveY(Vector3 point)
    {
        if (Math.Abs(point.y - transform.position.y) > 0.1f)
            transform.position += Vector3.up * (point.y - transform.position.y) * Time.deltaTime;
    }

    void MoveX(Vector3 point)
    {
        if (Math.Abs(point.x - transform.position.x) > 0.1f)
            transform.position += Vector3.right * (point.x - transform.position.x) * Time.deltaTime;
    }
}

using UnityEngine;
using System;
using System.Collections.Generic;
using static UnityEngine.Time;

public class MovePlayer : MonoBehaviour
{
    // 입력 관련
    int shootingBtnNum = 0;
    string moveBtnName = "Horizontal";
    string jumpBtnName = "up";

    bool mouseBtn0 = false;
    bool jump = false;
        
    bool jumpCont;

    float move;


    float speed = 1f;
    float rotate_speed = 60f;

    // 좌표를 얻기 위한 가상의 평면 생성
    Plane GroupPlane = new Plane(Vector3.forward, Vector3.zero);
    
    private Rigidbody playerRigidbody;  // rigidbody 설정
    private LineRenderer shootingPath;
    Vector3[] line_2 = new Vector3[2];
    //public GameObject particle;

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>(); // rigidbody 할당
        shootingPath = GetComponent<LineRenderer>();
        jumpCont = Input.GetKeyDown(jumpBtnName);
    }

    // Update is called once per frame
    void Update()
    {
        InputManager();
        Shooting();
        Move();
    }
    private void InputManager()
    {
        if (Input.GetMouseButtonDown(shootingBtnNum))
            mouseBtn0 = true;
        if (Input.GetMouseButtonUp(shootingBtnNum))
            mouseBtn0 = false;

        if (jumpCont)
            jump = true;

        if (Input.GetKeyUp(jumpBtnName))
            jump = false;

        move = Input.GetAxis(moveBtnName);

    }

    private void Shooting()
    {
        if (mouseBtn0)
        {
            // 카메라 시점에서 마우스에 닿는 좌표와 이어지는 선 수신 (픽셀 기준)
            // z축을 일직선으로 쏘는 광선 cameraRay를 생성
            Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            float rayLength; // ray의 길이를 받는 선
            if (GroupPlane.Raycast(cameraRay, out rayLength)) // z축이 0인 가상의 평면과 교차하는경우
            {
                Vector3 vec = cameraRay.GetPoint(rayLength);

                line_2[0] = transform.position;
                line_2[1] = vec;
                shootingPath.SetPositions(line_2); // 궤적을 그린다.
            }
        }
        else
        {
            line_2[0] = transform.position;
            line_2[1] = transform.position;
            shootingPath.SetPositions(line_2); // 아닌경우, 궤적을 없앤다.
        }
    }
    private void Move()
    {
        Vector3 movedir = move * Vector3.right * speed;

        playerRigidbody.velocity = movedir + playerRigidbody.velocity.y * Vector3.up;
    }

    private void Rotate(Vector3 vec)
    {
        double degree = (Math.Atan(vec.z / vec.x));
        Debug.Log(degree);
        degree = degree / 3; // degree값을 범위에 맞게 변환

        if (vec.x < 0)
        {
            degree += 1.5f;
        }
        else
        {
            
            degree += 0.5f;
        }

        Quaternion target = Quaternion.Euler(90,0, (float)degree*180+180);
        transform.rotation = Quaternion.Slerp(transform.rotation, target,1); // transform.rotation의 값을 target으로 바꾼다.
        //회전시킬거면 3D로 하자. LookAt이 편하다. 아니면 z축을 정면으로 만들던가.
    }
}

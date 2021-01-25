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
    bool mouseBtn0up = false;
    bool jump = false;// 점프키를 누르고 있는가를 확인
    bool bulletRight = false;
    bool bulletLeft = false;
     
    bool jumpCont; // 점프키를 꾹 누르고 있는가를 확인

    float move;

    float speed = 1f;
    float rotate_speed = 60f;

    // 좌표를 얻기 위한 가상의 평면 생성
    Plane GroupPlane = new Plane(Vector3.forward, Vector3.zero);
    
    private Rigidbody playerRigidbody;  // rigidbody 설정
    private LineRenderer shootingPath;
    Vector3[] line_2 = new Vector3[2];
    Vector3 shootingdir;

    RaycastHit target;


    public int[] bulletBelt = new int[5]; // 0:1:2:3:4
    public int choicebullet = 0, maxbulletnum = 5;

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>(); // rigidbody 할당
        shootingPath = GetComponent<LineRenderer>();

        choicebullet = 0;

        for(int i=0;i<maxbulletnum;i++)
        {
            bulletBelt[i] = 5;
        }
    }

    // Update is called once per frame
    void Update()
    {
        InputManager();
        Shooting();
        Move();
        bulletChange();
    }
    private void InputManager()
    {
        jumpCont = Input.GetKeyDown(jumpBtnName);
        mouseBtn0up = Input.GetMouseButtonUp(shootingBtnNum);

        if (Input.GetMouseButtonDown(shootingBtnNum))
            mouseBtn0 = true;
        if (mouseBtn0up)
            mouseBtn0 = false;

        if (jumpCont)
            jump = true;

        if (Input.GetKeyUp(jumpBtnName))
            jump = false;

        move = Input.GetAxis(moveBtnName);

        bulletLeft = Input.GetKeyDown("q");
        bulletRight = Input.GetKeyDown("e");
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
                shootingdir = cameraRay.GetPoint(rayLength);

                line_2[0] = transform.position;
                line_2[1] = shootingdir;
                shootingPath.SetPositions(line_2); // 궤적을 그린다.
            }
        }
        else if(mouseBtn0up)
        {
            if (bulletBelt[choicebullet] > 0)
            {
                if (Physics.Raycast(transform.position, shootingdir - transform.position, out target, Mathf.Infinity))
                {

                    Debug.Log("Hit! " + target.point);
                    Debug.Log(target.collider.tag);
                    switch (choicebullet)
                    {
                        case 0:
                            target.collider.tag = "Red";
                            break;
                        case 1:
                            target.collider.tag = "Blue";
                            break;
                        case 2:
                            target.collider.tag = "Yellow";
                            break;
                        case 3:
                            target.collider.tag = "Green";
                            break;
                        case 4:
                            target.collider.tag = "Black";
                            break;
                        default:
                            Debug.Log("Bulletchoice Error!");
                            target.collider.tag = "Untagged";
                            break;
                    }
                    Debug.Log(" -> " + target.collider.tag);
                }

                else
                {
                    Debug.Log("Nothing Hit");
                }
                bulletBelt[choicebullet]--;
            }
            else
            {
                Debug.Log("I don't have Bullet");
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

    private void bulletChange()
    {
        if(bulletLeft)
        {
            choicebullet--;
            if (choicebullet < 0)
            {
                choicebullet += maxbulletnum;
            }
            Debug.Log(choicebullet+" Left");
        }
        if(bulletRight)
        {
            choicebullet++;
            if(choicebullet>=maxbulletnum)
            {
                choicebullet -= maxbulletnum;
            }
            Debug.Log(choicebullet + " Right");
        }
    }

    
}

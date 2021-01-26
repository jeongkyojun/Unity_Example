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

    public GameObject bulletFactory; // bullet 프리팹을 지정하는 오브젝트

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>(); // rigidbody 할당
        shootingPath = GetComponent<LineRenderer>(); // lineRenderer 할당

        choicebullet = 0; // 총알 선택 기본값(0) 설정

        for(int i=0;i<maxbulletnum;i++)
        {
            bulletBelt[i] = 5;      // 총알 개수 설정 초기화
        }
    }

    // Update is called once per frame
    void Update()
    {
        InputManager(); // 입력 관련
        Shooting(); // 쏘는 행위 관련
        Move(); // 움직임 관련
        bulletChange(); // 총알 변환 관련
    }

    private void InputManager()
    {
        // 입력을 받는 기능 집합
        jumpCont = Input.GetKeyDown(jumpBtnName); // 점프키를 눌렀을 때
        mouseBtn0up = Input.GetMouseButtonUp(shootingBtnNum); // 마우스 왼쪽키를 눌렀을 때

        if (Input.GetMouseButtonDown(shootingBtnNum)) // 마우스키를 누르는 동안 mouseBtn0는 true
            mouseBtn0 = true;
        if (mouseBtn0up)// 떼면 false
            mouseBtn0 = false;

        if (jumpCont) // 점프키를 누르는 동안 true
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
                shootingdir = cameraRay.GetPoint(rayLength); // 끝 점을 저장받는다.

                line_2[0] = transform.position; // 선의 맨 처음은 자신의 위치
                line_2[1] = shootingdir; // 선의 맨 끝은 목적지
                shootingPath.SetPositions(line_2); // 궤적을 그린다.
            }
        }
        else if(mouseBtn0up) // 마우스를 눌렀다 뗐을때, 발사된다.
        {
            if (bulletBelt[choicebullet] > 0) // 총알이 1개 이상일때 - 발사된다.
            {
                GameObject bullet = Instantiate(bulletFactory);

                float angle = Mathf.Atan((shootingdir -  transform.position).y / (shootingdir-transform.position).x);

                if ((shootingdir - transform.position).x < 0)
                    angle += Mathf.PI;

                Debug.Log((shootingdir - transform.position)+" , "+angle);

                switch (choicebullet) // 내가 선택한 총알에 따라 타겟의 태그 변화
                {
                    case 0:
                        bullet.tag = "Red";
                        break;
                    case 1:
                        bullet.tag = "Blue";
                        break;
                    case 2:
                        bullet.tag = "Yellow";
                        break;
                    case 3:
                        bullet.tag = "Green";
                        break;
                    case 4:
                        bullet.tag = "Black";
                        break;
                    default:
                        Debug.Log("Bulletchoice Error!");
                        bullet.tag = "Untagged";
                        break;
                }

                bullet.transform.position = 1.5f*(new Vector3(Mathf.Cos(angle),Mathf.Sin(angle),0)) + transform.position;

                /*
                // 레이캐스트를 사용, 타겟에 맞았을 경우
                if (Physics.Raycast(transform.position, shootingdir - transform.position, out target, Mathf.Infinity))
                {
                    Debug.Log("Hit! " + target.point); // 타겟 위치 출력
                    Debug.Log(target.collider.tag); // 타겟 태그 출력
                    switch (choicebullet) // 내가 선택한 총알에 따라 타겟의 태그 변화
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
                    Debug.Log(" -> " + target.collider.tag); // 변화된 태그 출력
                }
                else
                {
                    Debug.Log("Nothing Hit"); // 안맞은 경우, 로그만 출력
                }
                */
                bulletBelt[choicebullet]--; // 쏜 경우, 총알을 하나 줄인다.
            }
            else
            {
                Debug.Log("I don't have Bullet"); // 총알이 없다
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

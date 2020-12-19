using UnityEngine;
using System;
using static UnityEngine.Time;

public class MovePlayer : MonoBehaviour
{
    private Rigidbody playerRigidbody;  // rigidbody 설정
    private PlayerInput playerInput;
    float speed = 5f;
    float rotate_speed = 60f;

    public GameObject particle;

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>(); // rigidbody 할당
        playerInput = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        Plane GroupPlane = new Plane(Vector3.up, Vector3.zero);

        float rayLength;

        if (GroupPlane.Raycast(cameraRay, out rayLength))
        {
            Vector3 vec = cameraRay.GetPoint(rayLength);
           // Debug.Log(vec.x + "," + vec.y + "," + vec.z);
            //Move(vec);
            Rotate(vec);
        }   

    }

    private void Move(Vector3 vec)
    {
        /*
        Vector3 moveDistance =
            playerInput.move * transform.up * speed * Time.deltaTime;

        playerRigidbody.MovePosition(playerRigidbody.position + moveDistance);
        //transform.position = new Vector3(transform.position.x + playerInput.move * deltaTime, transform.position.y , transform.position.z);
        */
        transform.position = new Vector3(
            transform.position.x + (vec.x-transform.position.x) * Time.deltaTime * speed,
            transform.position.y,
            transform.position.z + (vec.z-transform.position.z) * Time.deltaTime * speed);
    }

    private void Rotate(Vector3 vec)
    {
        double degree = (Math.Atan(vec.z / vec.x));
        /*
        float turn = playerInput.rotate *rotate_speed * Time.deltaTime;
        //리지드바디를 이용해 게임 오브젝트 회전 변경
        playerRigidbody.rotation = playerRigidbody.rotation * Quaternion.Euler(0f,0f,-turn);
        */
        Debug.Log(degree);
        degree = degree / 3; // degree값을 범위에 맞게 변환
        degree *= -1;
        if (vec.x < 0)
        {
            degree += 1.5f;
        }
        else
        {
            
            degree += 0.5f;
        }
        // 0 ~ 0.5 , 0.5 ~ 1 , 1 ~ 1.5, 1.5 ~ 2
        // 0 ~ 0.5 , 0.5 ~ 1 , 0 ~ 0.5, 0.5 ~ 1 


        Quaternion target = Quaternion.Euler(90,0, (float)degree*180);
        transform.rotation = Quaternion.Slerp(transform.rotation, target, 360);
    }
}

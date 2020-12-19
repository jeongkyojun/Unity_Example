using UnityEngine;
using System;
using static UnityEngine.Time;

public class MovePlayer : MonoBehaviour
{
    private Rigidbody playerRigidbody;  // rigidbody 설정
    private PlayerInput playerInput;
    float speed = 1f;
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
            Move(vec);
            Rotate(vec);
        }   

    }

    private void Move(Vector3 vec)
    {
        transform.position = new Vector3(
            transform.position.x + (vec.x-transform.position.x) * Time.deltaTime * speed,
            transform.position.y,
            transform.position.z + (vec.z-transform.position.z) * Time.deltaTime * speed);
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

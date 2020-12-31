using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    private Rigidbody playerRigidbody;  // rigidbody 설정
    private PlayerInput playerInput;
    float speed = 1f,direction,timeX,timeZ,Xn,Zn; // 속도
    bool keyX, keyZ;
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>(); // rigidbody 할당
        playerInput = GetComponent<PlayerInput>();
        keyX = false;
        keyZ = false;
        Xn = 1f;
        Zn = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        if (!keyX || timeX > 1f + Xn)
        {
            if (playerInput.move < 0)
                direction = -1;
            else if (playerInput.move > 0)
                direction = 1;
            else
                direction = 0;
            transform.position = new Vector3(transform.position.x + direction * speed, 0, transform.position.z);

            if (direction != 0)
                timeX = 0;
            if(Xn>0.3f)
                Xn = Xn-0.1f;
        }
        if(!keyZ || timeZ > 1f + Zn)
        {
            if (playerInput.rotate < 0)
                direction = -1;
            else if (playerInput.rotate > 0)
                direction = 1;
            else
                direction = 0;
            transform.position = new Vector3(transform.position.x, 0, transform.position.z - direction * speed);
            
            if(direction!=0)
                timeZ = 0;
            if (Zn > 0.3f)
                Zn = Zn-0.1f;
        }

        // 계속 누르고 있는지 확인
        if (playerInput.move != 0 )
        {
            keyX = true;
        }
        else
        {
            keyX = false;
            Xn = 0f;
        }

        //계속 누르고 있는지 확인
        if (playerInput.rotate != 0 )
        {
            keyZ = true;
        }
        else
        {
            keyZ = false;
            Zn = 0f;
        }

        timeX += Time.deltaTime;
        timeZ += Time.deltaTime;
    }
}

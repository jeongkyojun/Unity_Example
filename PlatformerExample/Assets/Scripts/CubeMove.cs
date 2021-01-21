using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMove : MonoBehaviour
{
    bool isMove;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionStay(Collision other)
    {
        if(other.collider.tag == "player")
        {
            if (isMove)
            {
                /*
                 * 플레이어의 키입력에 따라 움직인다.
                 */
            }
            else
            {
                if (Input.GetKey("shift"))
                {
                    isMove = true;
                }
                else
                    isMove = false;
            }
        }
    }

    void OnCollisionExit()
    {
        isMove = false;
    }
}

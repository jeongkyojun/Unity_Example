using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public string InputAxisMove = "Horizontal";
    public string InputAxisJump = "Jump";
    public string InputAxisFire1 = "Fire1";
    public string InputAxisFire2 = "Fire2";
    public string InputAxisFire3 = "Fire3";

    public float moveA, jumpA, dashA;

    public bool leftD,rightD, jumpD, dashD, leftU,rightU, jumpU, dashU;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        moveA = Input.GetAxis(InputAxisMove);

        leftD = Input.GetKeyDown("left");
        rightD = Input.GetKeyDown("right");

        jumpD = Input.GetKeyDown("space");
        dashD = Input.GetKeyDown("z");

        leftU = Input.GetKeyUp("left");
        rightU = Input.GetKeyUp("right");

        jumpU = Input.GetKeyUp("space");
        dashU = Input.GetKeyUp("z");
        if(jumpD)
        {
            Debug.Log("Input Space key");
        }
        if(jumpU)
        {
            Debug.Log("Output Space key");
        }
    }
}

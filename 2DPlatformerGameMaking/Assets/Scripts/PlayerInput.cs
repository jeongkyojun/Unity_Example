using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public string InputGetHorizontal = "Horizontal";


    public float move;
    public bool jump;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        move = Input.GetAxis(InputGetHorizontal);
        jump = Input.GetKeyDown(KeyCode.Space);
    }
}

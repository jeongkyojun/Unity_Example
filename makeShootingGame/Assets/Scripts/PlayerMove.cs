using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private int cnt = 0;
    private float pastTime=0;
    private float speed = 5f;
    PlayerInput playerInput;
    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        cnt = 0;
        pastTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(playerInput.rotate * Time.deltaTime*speed, playerInput.move * Time.deltaTime*speed, 0));
    }
}

using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerModel : MonoBehaviour
{

    public float speed = 5f;
    public float jumpPower = 10f;

    public int jumpCnt = 0;

    public event Action onDeath; // 사망시 발동할 이벤트

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}

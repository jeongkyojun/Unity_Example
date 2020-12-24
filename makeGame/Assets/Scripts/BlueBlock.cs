using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueBlock : MonoBehaviour
{
    //3초 존재, 1초 비존재
    private float PastTime;
    private bool isActive;
    private float x, y, z;
    private float ActiveNum = 3f, MaxNum = 4f,cnt;
    // Start is called before the first frame update
    void Start()
    {
        cnt = 0;
        MaxNum = 4;
        ActiveNum = 2;
        PastTime = 0f;
        isActive = true;
        x = transform.position.x;
        y = transform.position.y;
        z = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - PastTime >= 1)
        {
            PastTime = Time.time;
            if (isActive)
            {
                transform.position = new Vector3(x, y, z);
                isActive = false;
            }
            else if(cnt > ActiveNum/MaxNum)
            {
                transform.position = new Vector3(x, y, 21);
                isActive = true;
            }
            if (++cnt == MaxNum)
                cnt = 0;
        }
    }
}

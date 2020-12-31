using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMove : MonoBehaviour
{
    public float dir;
    // Start is called before the first frame update
    void Start()
    {
        dir = 1;
    }

    // Update is called once per frame
    void Update()
    {
        //-9.5 ~ -7
        if (transform.position.x > -7)
            dir = -1;
        if (transform.position.x < -9.5)
            dir = 1;
        transform.position += new Vector3(0.02f*dir, 0f,0f);
    }
}

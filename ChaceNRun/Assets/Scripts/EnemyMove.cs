using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public float moveSpeed = 3f;
    public Transform cube;

    int num;
    float recentTime;
    // Start is called before the first frame update
    void Start()
    {
        cube.GetComponent<MeshRenderer>().material.color = new Color(1, 0, 0, 1);

        recentTime = 0.0f;
        num = 0;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(moveSpeed*Time.deltaTime, 0, 0);
        if (Time.time - recentTime >= 1)
        {
            recentTime = Time.time;
            switch (num)
            {
                case 0:
                    cube.GetComponent<MeshRenderer>().material.color = new Color(1, 0, 0, 1);
                    break;
                case 1:
                    cube.GetComponent<MeshRenderer>().material.color = new Color(0, 1, 0, 1);
                    break;
                case 2:
                    cube.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 1, 1);
                    break;
                case 3:
                    cube.GetComponent<MeshRenderer>().material.color = new Color(1, 1, 0, 1);
                    break;
            }
            num++;
            num = num % 4;
        }
    }
}

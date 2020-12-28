using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Vector3 PlayerTransform;
    public GameObject Player;
    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        int playerTransformX = ((int)Player.transform.position.x+5) / 10;
        int playerTransformY = ((int)Player.transform.position.y+5) / 10;
        Debug.Log(playerTransformX+" , "+playerTransformY);
        Debug.Log(new Vector3(10 * playerTransformX, 10 * playerTransformY, -10));
        transform.position = new Vector3(10*playerTransformX, 10*playerTransformY, -10);
    }
}

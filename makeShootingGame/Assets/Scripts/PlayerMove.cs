using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float move = Input.GetAxis("Horizontal");
        float updown = Input.GetAxis("Vertical");

        Vector3 dir = Vector3.right * move + Vector3.up * updown;

        transform.Translate(dir * speed * Time.deltaTime);

        //다른 방법 1
        //transform.Translate(new Vector3(move, updown, 0) * speed * Time.deltaTime);

        //다른 방법 2
        //transform.Translate(new Vector3(move*speed*Time.deltaTime, updown * speed * Time.deltaTime, 0));
    }
}

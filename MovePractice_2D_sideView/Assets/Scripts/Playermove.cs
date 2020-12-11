using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playermove : MonoBehaviour
{
    private Rigidbody2D rigidbody2D;

    private PlayerInput playerInput;

    private float speed = 3f;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.x + playerInput.rotate * Time.deltaTime * speed<8.5 && transform.position.x + playerInput.rotate * Time.deltaTime * speed > -8.5)
            transform.position = new Vector2(transform.position.x + playerInput.rotate * Time.deltaTime * speed, transform.position.y);
        
        if(Input.GetKey(KeyCode.Space))
        {
            rigidbody2D.AddForce(new Vector2(0, 3));
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followPlayer : MonoBehaviour
{
    PlayerInput playerInput;
    float speed =3f;
    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerInput.rotate!=0)
            transform.position = new Vector3(transform.position.x+playerInput.rotate*Time.deltaTime*speed, 0,-10);
    }
}

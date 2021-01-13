using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPoints : MonoBehaviour
{
    GameObject MoveObject;
    Vector3 ObjectVector;
    Vector3 ObjectSub;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision other)
    {
        MoveObject = other.gameObject;
        ObjectVector = other.gameObject.transform.position;

        ObjectSub = transform.position - ObjectVector;
    }

    void OnCollisionStay(Collision other)
    {
        if(other.gameObject == MoveObject)
        {
            transform.position = other.gameObject.transform.position + ObjectSub;
        }

    }
}

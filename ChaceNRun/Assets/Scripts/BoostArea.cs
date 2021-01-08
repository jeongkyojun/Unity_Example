using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostArea : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("trigger"+other.tag);

        if(other.tag == "ChasingEnemy")
        {
            Debug.Log("OntriggerEnter");
        }
    }


    void OnCollisionEnter(Collision other)
    {
        Debug.Log("collision" + other.collider.tag);

        if (other.collider.tag == "ChasingEnemy")
        {
            Debug.Log("OnCollisionEnter");
        }
    }
}

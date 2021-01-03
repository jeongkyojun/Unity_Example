using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public float moveSpeed = 3f;
    public Transform cube;
    public float damage = 1000000f;


    private bool isHit = true;

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

    }


    void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter");
        if (isHit)
        {
            if (other.tag == "Player")
            {
                
                PlayerMove playerMove = other.GetComponent<PlayerMove>();
                Debug.Log("offset : " + other.contactOffset);
                if (playerMove!=null)
                {
                    //Vector3 hitPoint = other.contactOffset;
                    //Vector3 hitNormal = other.contactOffset - other.transform.position;
                    //playerMove.OnDamage(damage, hitPoint, hitNormal);
                }
            }
            //isHit = false;
        }
    }
    void OnCollisionEnter(Collision other)
    {
        Debug.Log("OnCollisionEnter");
        if (isHit)
        {
            if (other.collider.tag =="Player" )
            {
                

                
            }
            //isHit = false;
        }
    }
    void OnTriggerExit(Collider collision)
    {
        Debug.Log("OnTriggerExit");
        isHit = true;
    }

    void OnCollisionExit(Collision collision)
    {
        Debug.Log("OnCollisionExit");
        isHit = true;
    }
}

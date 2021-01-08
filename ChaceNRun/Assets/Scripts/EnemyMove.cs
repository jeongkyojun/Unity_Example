using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public GameObject Lookingsight;
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
        Debug.Log("OnTriggerEnter(Enemy)");
        //Debug.Log(other.tag);
    }
    void OnCollisionEnter(Collision other)
    {
        Debug.Log("OnCollisionEnter(Enemy)");
        //Debug.Log(other.collider.tag);
    }
    void OnTriggerExit(Collider collision)
    { 
       
    }

    void OnCollisionExit(Collision collision)
    {

    }
}

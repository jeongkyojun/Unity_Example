using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapFind : MonoBehaviour
{
    public GameObject trap;
    TrapCaboom trapCaboom;
    // Start is called before the first frame update
    void Start()
    {
        trapCaboom = trap.GetComponent<TrapCaboom>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("findPlayer");
        trapCaboom.isSetup = true;
    }
}

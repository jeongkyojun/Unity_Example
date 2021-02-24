using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManaging : MonoBehaviour
{
    public int X, Y;
    public GameObject cam;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        cam.transform.position = transform.position + -7*Vector3.forward;
    }
}

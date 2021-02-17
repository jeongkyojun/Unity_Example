using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    // dummy Data

    MenuManagingScript menuScript;
    Dictionary<KeyAction, KeyCode> Input;

    // Start is called before the first frame update
    void Start()
    {
        menuScript = FindObjectOfType<MenuManagingScript>();
        Input = menuScript.Ie.keySet;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

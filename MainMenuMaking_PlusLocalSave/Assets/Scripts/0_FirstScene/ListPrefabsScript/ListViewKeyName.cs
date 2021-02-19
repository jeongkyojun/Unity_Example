using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListViewKeyName : MonoBehaviour
{
    public KeyAction keyType;
    public Text keyName;

    // Start is called before the first frame update
    void Start()
    {
        keyName = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        keyName.text = keyType.ToString();
    }
}

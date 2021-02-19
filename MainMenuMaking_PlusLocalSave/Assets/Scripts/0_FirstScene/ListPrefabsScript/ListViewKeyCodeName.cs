using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListViewKeyCodeName : MonoBehaviour
{
    public KeyAction keyType;
    public Text keyCodeName;
    public GameObject GM;
    MenuManagingScript GmManaging;
    // Start is called before the first frame update
    void Start()
    {
        keyCodeName = GetComponent<Text>();
        GmManaging = FindObjectOfType<MenuManagingScript>();
    }

    // Update is called once per frame
    void Update()
    {
        keyCodeName.text = GmManaging.Ie.keySet[keyType].ToString();
    }

    public void OnclickListBtn()
    {
        Debug.Log("Click KeyName : "+GmManaging.Ie.keySet[keyType].ToString());    
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject MainMenuManager;
    MenuManagingScript menuScript;
    // Start is called before the first frame update
    void Start()
    {
        MainMenuManager = GameObject.Find("FirstSceneManager");
        menuScript = MainMenuManager.GetComponent<MenuManagingScript>();
        Debug.Log(menuScript.mE.Volume);
        Debug.Log(menuScript.pE.savePath);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

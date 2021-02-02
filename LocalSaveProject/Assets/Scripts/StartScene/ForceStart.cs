using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceStart : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void InitScene()
    {
        Debug.Log("[SceneTitle] Init Scene-----------------");
        if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.CompareTo("StartScene")!=0)
        {
            Debug.Log("SceneChange");
            UnityEngine.SceneManagement.SceneManager.LoadScene("StartScene");
        }
    }
    private void Start()
    {
    }
}

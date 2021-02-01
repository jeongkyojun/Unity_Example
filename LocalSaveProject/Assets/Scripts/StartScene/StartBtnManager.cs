using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartBtnManager : MonoBehaviour
{
    public void OnClickStartBtn()
    {
        Debug.Log("StartBtn Click!");
        SceneManager.LoadScene("GameScene");
    }
}

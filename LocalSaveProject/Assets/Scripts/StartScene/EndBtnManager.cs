using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndBtnManager : MonoBehaviour
{
    public void OnClickEndBtn()
    {
        Debug.Log("Click EndBtn");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit()
#endif
    }
}

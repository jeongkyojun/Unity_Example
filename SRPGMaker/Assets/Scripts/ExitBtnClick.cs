using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ExitBtnClick : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Vector3 defaultSize;
    // Start is called before the first frame update
    void Start()
    {
        defaultSize = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnExitBtnClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // 유니티 에디터인 경우
#else
                Application.Quit();// 일반 빌드일 경우
#endif
    }

    public void OnPointerEnter(PointerEventData data)
    {
        transform.localScale = defaultSize * 1.1f;
    }
    public void OnPointerExit(PointerEventData data)
    {
        transform.localScale = defaultSize;
    }
}

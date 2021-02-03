using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BtnType : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    public ButtonType currentType;
    //public Transform buttonScale;
    Vector3 defaultScale;
    public CanvasGroup mainGroup;
    public CanvasGroup optionGroup;
    
    bool isSound = true;

    private void Start()
    {
        //defaultScale = buttonScale.localScale;
        defaultScale = transform.localScale;
    }
    public void OnBtnClick()
    {
        switch (currentType)
        {
            case ButtonType.New:
                Debug.Log("새게임");
                ScreenLoader.LoadSceneHandle("Play", 0);
                break;
            case ButtonType.Continue:
                Debug.Log("이어하기");
                ScreenLoader.LoadSceneHandle("Play", 1);
                break;
            case ButtonType.Option:
                Debug.Log("설정");
                CanvasGroupOn(optionGroup);
                CanvasGroupOff(mainGroup);
                break;
            case ButtonType.Sound:
                Debug.Log("소리");
                if (isSound)
                {
                    Debug.Log("사운드 off");
                }
                else
                {
                    Debug.Log("사운드 on");
                }
                isSound = !isSound;
                break;
            case ButtonType.Back:
                Debug.Log("뒤로가기");
                CanvasGroupOn(mainGroup);
                CanvasGroupOff(optionGroup);
                break;
            case ButtonType.Quit:
                Debug.Log("끝내기");
#if UNITYENGINE
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
                break;
        }
    }

    public void CanvasGroupOn(CanvasGroup cg)
    {
        cg.alpha = 1;
        cg.interactable = true;
        cg.blocksRaycasts = true;
    }
    public void CanvasGroupOff(CanvasGroup cg)
    {
        cg.alpha = 0;
        cg.interactable = false;
        cg.blocksRaycasts = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //buttonScale.localScale = defaultScale * 1.2f;
        transform.localScale = defaultScale * 1.2f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //buttonScale.localScale = defaultScale;
        transform.localScale = defaultScale;
    }
}

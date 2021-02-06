using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

public class BtnUI : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    public GameObject firstSceneGameManager;
    MenuManagingScript menuEntity;
    
    public TextMeshProUGUI btnText;

    public BtnType currentType;

    public CanvasGroup mainGroup;
    public CanvasGroup optionGroup;
    public CanvasGroup loadGroup;
    public CanvasGroup volumeGroup;

    Vector3 defaultSize; // 기본 버튼 사이즈 저장

    // Start is called before the first frame update
    void Start()
    {
        
        defaultSize = transform.localScale;
        switch (currentType)
        {
            case BtnType.New:
                break;
            case BtnType.Load:
                break;
            case BtnType.Option:
                break;
            case BtnType.Sound:
                break;
            case BtnType.BackToMain:
                break;
            case BtnType.BackToOption:
                break;
            case BtnType.End:
                break;
        }
    }

    public void OnClickBtnManager()
    {
        switch (currentType)
        {
            case BtnType.New:
                Debug.Log("새게임");
                SceneManager.LoadScene("1_Loading");
                break;
            case BtnType.Load:
                Debug.Log("이어하기");
                CanvasGroupOn(loadGroup);
                CanvasGroupOff(mainGroup);
                //ScreenLoader.LoadSceneHandle("Play", 1);
                break;
            case BtnType.Option:
                Debug.Log("설정");
                CanvasGroupOn(optionGroup);
                CanvasGroupOff(mainGroup);
                break;
            case BtnType.Sound:
                Debug.Log("소리");
                CanvasGroupOff(optionGroup);
                CanvasGroupOn(volumeGroup);
                break;
            case BtnType.BackToMain:
                Debug.Log("메인메뉴로 가기");
                CanvasGroupOn(mainGroup);
                CanvasGroupOff(optionGroup);
                break;
            case BtnType.BackToOption:
                Debug.Log("옵션으로 돌아가기");
                CanvasGroupOff(volumeGroup);
                CanvasGroupOn(optionGroup);
                break;
            case BtnType.End:
                Debug.Log("끝내기");
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false; // 유니티 에디터인 경우
#else
                Application.Quit();// 일반 빌드일 경우
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
        transform.localScale = defaultSize * 1.2f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //buttonScale.localScale = defaultScale;
        transform.localScale = defaultSize;
    }

}

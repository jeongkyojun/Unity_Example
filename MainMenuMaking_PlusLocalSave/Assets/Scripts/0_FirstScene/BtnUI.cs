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

    public GameObject MenuManager;
    MenuManager MenuManagerScript;

    public TextMeshProUGUI btnText;

    public BtnType currentType;

    Vector3 defaultSize; // 기본 버튼 사이즈 저장

    // Start is called before the first frame update
    void Start()
    {
        MenuManagerScript = MenuManager.GetComponent<MenuManager>();
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
            case BtnType.Back:
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
                Debug.Log("세이브 파일 선택");
                //현재 캔버스 그룹을 off 한다.
                CanvasGroupOff(MenuManagerScript.GroupArr[MenuManagerScript.ArrPoint]);
                MenuManagerScript.GroupArr[++MenuManagerScript.ArrPoint] = MenuManagerScript.LoadGroup;
                CanvasGroupOn(MenuManagerScript.LoadGroup);
                break;
            case BtnType.Load:
                Debug.Log("이어하기");
                
                //ScreenLoader.LoadSceneHandle("Play", 1);
                break;
            case BtnType.Option:
                Debug.Log("설정");
                CanvasGroupOff(MenuManagerScript.GroupArr[MenuManagerScript.ArrPoint]);
                MenuManagerScript.GroupArr[++MenuManagerScript.ArrPoint] = MenuManagerScript.OptionGroup;
                CanvasGroupOn(MenuManagerScript.OptionGroup);
                break;
            case BtnType.Sound:
                Debug.Log("소리");
                CanvasGroupOff(MenuManagerScript.GroupArr[MenuManagerScript.ArrPoint]);
                MenuManagerScript.GroupArr[++MenuManagerScript.ArrPoint] = MenuManagerScript.VolumeGroup;
                CanvasGroupOn(MenuManagerScript.VolumeGroup);
                break;
            case BtnType.Back:
                Debug.Log("이전메뉴로 이동");
                CanvasGroupOff(MenuManagerScript.GroupArr[MenuManagerScript.ArrPoint]);
                CanvasGroupOn(MenuManagerScript.GroupArr[--MenuManagerScript.ArrPoint]);
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
        transform.localScale = defaultSize * 1.2f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = defaultSize;
    }

}

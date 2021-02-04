using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class BtnUI : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    public GameObject firstSceneGameManager;
    AudioSource menuaudio;
    public TextMeshProUGUI btnText;

    public BtnType currentType;

    public CanvasGroup mainGroup;
    public CanvasGroup optionGroup;

    Vector3 defaultSize; // 기본 버튼 사이즈 저장
    bool isSound=true;
    int SoundMode = 0;
    float SoundVolume = 0.25f;
    // Start is called before the first frame update
    void Start()
    {
        defaultSize = transform.localScale;
        menuaudio = firstSceneGameManager.GetComponent<AudioSource>();
    }

    public void OnClickBtnManager()
    {
        switch (currentType)
        {
            case BtnType.New:
                Debug.Log("새게임");
                //ScreenLoader.LoadSceneHandle("Play", 0);
                break;
            case BtnType.Load:
                Debug.Log("이어하기");
                //ScreenLoader.LoadSceneHandle("Play", 1);
                break;
            case BtnType.Option:
                Debug.Log("설정");
                CanvasGroupOn(optionGroup);
                CanvasGroupOff(mainGroup);
                break;
            case BtnType.Sound:
                Debug.Log("소리");
                /*
                if (isSound)
                {
                    Debug.Log("사운드 off");
                    menuaudio.Stop();
                    btnText.text = "Sound on".ToString();
                }
                else
                {
                    Debug.Log("사운드 on");
                    menuaudio.Play();
                    btnText.text = "Sound off".ToString();
                }
                */
                //isSound = !isSound;
                
                SoundMode++;
                if (SoundMode == 5)
                    SoundMode -= 5;
                menuaudio.volume = 1 - SoundMode * SoundVolume;
                btnText.text = "Sound : "+menuaudio.volume.ToString();

                break;
            case BtnType.BackToMain:
                Debug.Log("메인메뉴로 가기");
                CanvasGroupOn(mainGroup);
                CanvasGroupOff(optionGroup);
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

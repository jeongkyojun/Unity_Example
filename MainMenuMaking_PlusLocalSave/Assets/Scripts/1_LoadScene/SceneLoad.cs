using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoad : MonoBehaviour
{
    public Slider progressbar;
    public Text loadtext;
    
    private void Start()
    {
        // 코루틴 실행
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        yield return null;
        // 비동기 실행
        // 비동기 씬을 불러온다.
        AsyncOperation operation = SceneManager.LoadSceneAsync("2_Play");
        //AsyncOperation 관련 함수
        //isDone : boolean으로 작업의 완료 유무를 반환
        //progress : float로 작업의 진행 정도를 0,1 로 표시(0이면 진행중, 1이면 진행완료)
        //allowSceneActivation : true면 로딩이 완료되면 바로 신을 넘김
        //                       false면 progress가 0.9f에서 멈춤 (이때 다시 true를 해야 불러온 씬으로 넘어갈 수 있다.)


        operation.allowSceneActivation = false;
        // 비동기 operation 이 완료되면 isDone에서 true를 반환
        while(!operation.isDone)
        {
            yield return null;
            if (progressbar.value < 0.9f)
            {
                progressbar.value = Mathf.MoveTowards(progressbar.value, 0.9f, Time.deltaTime);
            }
            else if(operation.progress>=0.9f)
            {
                progressbar.value = Mathf.MoveTowards(progressbar.value, 1f, Time.deltaTime);
            }
            
            if (progressbar.value>=1f)
            {
                loadtext.text = "press spacebar";   
            }

            if (progressbar.value>=1f&&operation.progress>=0.9f&&Input.GetKeyDown("space"))
            {
                Debug.Log("press space");
                operation.allowSceneActivation = true;
            }

        }
    }

    
}

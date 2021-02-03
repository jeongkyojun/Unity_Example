using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScreenLoader : MonoBehaviour
{
    public Slider progressbar;
    public Text loadtext;
    public static string loadScene;
    public static int loadType;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadScene());
    }

    public static void LoadSceneHandle(string _name, int _loadType)
    {
        loadScene = _name;
        loadType = _loadType;
        SceneManager.LoadScene("Loading");
    }    

    IEnumerator LoadScene()
    {
        yield return null;
        AsyncOperation operation = SceneManager.LoadSceneAsync(loadScene);
        operation.allowSceneActivation = false;

        while(!operation.isDone)
        {
            yield return null;
            if (loadType == 0)
            {
                Debug.Log("새게임");
                /*
                 * 모든 데이터를 초기값으로 적용하고 시작한다.
                 */
            }
            else if (loadType == 1)
            {
                Debug.Log("불러오기");
                /*
                 * 저장된 데이터의 값을 불러온 뒤 시작한다.
                 */
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}

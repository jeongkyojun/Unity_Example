using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject gameOverUI; // 게임오버 문자 출력
    public GameObject player;   // 플레이어 게임 오브젝트
    public GameObject Enemy;    // 적 게임오브젝트

    public Text timeText;
    public Text scoreText;

    private float surviveTime; // 생존시간
    private bool isGameOver; //게임오버 상태


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            surviveTime = Time.time;
            isGameOver = false;
        }
        else
        {
            Debug.LogWarning("씬에 두개 이상의 게임 매니저가 존재");
            Destroy(gameObject);
        }
    }
    // Update is called once per frame
    void Update()
    {
        Debug.Log(player.GetComponent<PlayerMove>().isGrounded);
        if (!isGameOver)
        {
            int timecut = (int)((Time.time - surviveTime) * 100);
            float textTime = timecut / 100f;
            timeText.text = "Time : " + textTime;

        }
        else
        {
            if(Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene("Main");
            }
        }

        
    }

    public void OnPlayerDead()
    {
        isGameOver = true;
        gameOverUI.SetActive(true);
    }

    
}

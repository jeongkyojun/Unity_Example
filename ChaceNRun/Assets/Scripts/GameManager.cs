using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public GameObject gameOverText;
    public GameObject player;
    public GameObject Enemy;
    public Text timeText;
    public Text scoreText;

    private float surviveTime; // 생존시간
    private bool isGameOver; //게임오버 상태


    // Start is called before the first frame update
    void Start()
    {
        surviveTime = Time.time;
        isGameOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        int timecut = (int)((Time.time - surviveTime) * 100);
        float textTime = timecut / 100f;
        timeText.text = "Time : " + textTime;

    }

    public void EndGame()
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    // 싱글톤 접근용 프로퍼티
    public static GameManager gameManager
    {
        get
        {
            // 싱글톤 변수에 오브젝트가 할당되지 않았다면
            if (_gameManager == null)
            {
                // 씬에서 GameManager 오브젝트를 찾아서 할당받는다.
                _gameManager = FindObjectOfType(typeof(GameManager)) as GameManager;
            }
            return _gameManager;
        }
    }
    private static GameManager _gameManager;

    public event Action TurnEnd; // 턴 종료시 발생하는 이벤트

    private int turn = 0; // 현재 턴 상태
    public bool isGameOver { get; private set; } // 게임 오버 상태 확인

    private void Awake() // 활성화 상태가 아니어도 된다.
    {
        // 싱글톤 오브젝트가 된 다른 GameManager오브젝트가 있다면
        if (_gameManager != this)
        {
            // 자신을 파괴
            Destroy(gameObject);
        }
    }

    void Start() // 활성화 상태일때만 된다.
    {
        //1. 받은 정보를 통해서 새로운 맵을 생성한다.

        //2. 몬스터들을 생성한 뒤, 위치에 맞게 배치한다.

    }

    // Update is called once per frame
    void Update()
    {

    }

    // 턴 갱신
    public void AddTurn()
    {
        turn++;
    }

    public void EndGame()
    {
        // 게임 오버 상태를 참으로 변경
        isGameOver = true;

        // 게임오버 UI 활성화 (미구현)
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    GameManager GMSetting;
    LivingEntity ControllerInfo;
    bool isLeft;
    bool isRight;
    bool isUp;
    bool isDown;

    Vector3 up, down, left, right;
    // Start is called before the first frame update
    void Start()
    {
        ControllerInfo = GetComponent<LivingEntity>(); // 같이 포함된 LivingEntity 탐색
        GMSetting = FindObjectOfType<GameManager>(); // 게임매니저 탐색
        SettingDirection();
    }

    // Update is called once per frame
    void Update()
    {
        if(ControllerInfo.playAble)
        {
            /*
             * 게임 매니저 정보 갱신
             * 
             * 위치 이동 설정
             */
            InputManager();
        }
    }

    void SettingDirection()
    {
        up = Vector3.up;
        down = Vector3.down;
        left = Vector3.left;
        right = Vector3.right;
    }

    // 상하좌우 이동설정
    void InputManager()
    {
        if(Input.GetKeyDown("up"))
        {
            Move(1, 0, 0, 0);
        }
        if(Input.GetKeyDown("down"))
        {
            Move(0, 1, 0, 0);
        }
        if(Input.GetKeyDown("left"))
        {
            Move(0, 0, 0, 1);
        }
        if(Input.GetKeyDown("right"))
        {
            Move(0,0,1,0);
        }
    }

    // 이동 함수 적용
    void Move(int up=0, int down=0, int right=0, int left=0)
    {
        // position 이동 및 저장된 위치조정
        transform.position += (this.up*up + this.down*down) * (GMSetting.TileSize.x) + (this.left*left + this.right*right) * (GMSetting.TileSize.y);
        GMSetting.Entities[GMSetting.Turn].Position += (up - down) * Vector2Int.up + (right - left) * Vector2Int.right;
    }
}

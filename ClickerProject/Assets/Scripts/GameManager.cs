using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    MovePlayer movePlayer;

    public Text howmanyBullet;
    public Image bulletColor;

    // Start is called before the first frame update
    void Start()
    {
        movePlayer = player.GetComponent<MovePlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            bulletColor.color = bulletBeltColor(movePlayer.choicebullet);
            howmanyBullet.text = movePlayer.bulletBelt[movePlayer.choicebullet].ToString();
        }
        catch(Exception e)
        {
            Debug.Log("error::"+e);
            movePlayer = GetComponent<MovePlayer>();
        }
        
    }

    public Color bulletBeltColor(int num)
    {
        switch (num)
        {
            case 0:
                return Color.red;
            case 1:
                return Color.blue;
            case 2:
                return Color.yellow;
            case 3:
                return Color.green;
            case 4:
                return Color.black;
            default:
                return Color.white;
        }
    }
}

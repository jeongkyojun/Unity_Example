using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public Text healthText;
    public Text TimeText;

    PlayerMove playerMove;
    // Start is called before the first frame update
    void Start()
    {
        playerMove = player.GetComponent<PlayerMove>();

    }

    // Update is called once per frame
    void Update()
    {
        int time = (int)(Time.time * 100);
        float timereturn = (float)time/100;

        TimeText.text = "Time : "+timereturn.ToString();
    }
}

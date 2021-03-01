using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnScript : MonoBehaviour
{
    GameManager GM;
    Button TurnEndBtn;
    // Start is called before the first frame update
    void Start()
    {
        GM = FindObjectOfType<GameManager>();
        TurnEndBtn = GetComponent<Button>();
    }


    public void OnClickTurnEndBtn()
    {
        GM.turn = 2;
        TurnEndBtn.interactable = false;
    }
}

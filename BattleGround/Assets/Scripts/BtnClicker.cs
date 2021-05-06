using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnClicker : MonoBehaviour
{
    GameManager GM;
    // Start is called before the first frame update
    void Start()
    {
        GM = FindObjectOfType<GameManager>();
    }

    public void OnClickTurnEnd()
    {
        GM.ChangeTurn();
    }
}

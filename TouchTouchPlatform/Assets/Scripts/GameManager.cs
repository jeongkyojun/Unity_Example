using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject Player;
    public GameObject OrangeBlock;
    public GameObject BlueBlock;

    bool OrangeActive;
    bool BlueActive;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(Player.GetComponent<PlayerMove>().StatusColor == 1)
        {
            OrangeActive = true;
            BlueActive = true;
        }
        else if(Player.GetComponent<PlayerMove>().StatusColor == 2)
        {
            OrangeActive = false;
            BlueActive = true;
        }
        else
        {
            OrangeActive = true;
            BlueActive = false;
        }

        OrangeBlock.SetActive(OrangeActive);
        BlueBlock.SetActive(BlueActive);
    }
}

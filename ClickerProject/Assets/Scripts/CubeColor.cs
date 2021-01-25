using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeColor : MonoBehaviour
{
    Renderer cubeColor;
    public GameObject GM;
    GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        cubeColor = GetComponent<Renderer>();
        gameManager = GM.GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        cubeColor.material.color = gameManager.bulletBeltColor(tagToNum(gameObject.tag));
    }

    int tagToNum(string tag)
    {
        switch (tag)
        {
            case "Red":
                return 0;
            case "Blue":
                return 1;
            case "Yellow":
                return 2;
            case "Green":
                return 3;
            case "Black":
                return 4;
            default:
                return -1;
        }
    }
}

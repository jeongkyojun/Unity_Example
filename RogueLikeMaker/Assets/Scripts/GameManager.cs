using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Tiles
{
    Pos[,] set;
}

public struct Pos
{

}

public class GameManager : MonoBehaviour
{
    Tiles Map;
    // Start is called before the first frame update
    void Start()
    {
        Map = new Tiles();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

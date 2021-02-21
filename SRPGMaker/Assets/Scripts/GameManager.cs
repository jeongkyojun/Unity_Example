using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct TileInfo
{
    public position[,] pos;
};
public struct position
{
    public bool isSet;
    public bool isTrue;
    public Color TileColor;
    public Vector3 TilePos;
};

public class GameManager : MonoBehaviour
{
    public GameObject normalPrefab;
    public GameObject cityPrefab;
    
    TileInfo tileInfo = new TileInfo();
    // Start is called before the first frame update
    void Start()
    {
        GenerateMap(ref tileInfo);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateMap(ref TileInfo tileInfo)
    {
        int RowSize = 23;
        int ColSize = 23;
        float firstPosRow = 0;
        float firstPosCol = 0;

        float firstLineRow = 0.75f;
        float firstLineCol = 1f;

        float secondLineCol = 0.5f;

        position[,] Mappos = new position[RowSize,ColSize];
        for(int i=0;i<RowSize;i++)
        {
            for(int j=0;j<ColSize;j++)
            {
                if ((i % 2 == 0) && (j == ColSize - 1))
                    Mappos[i, j].isTrue = false;
                else
                {
                    Debug.Log(i + " , " + j);
                    Mappos[i, j].isTrue = true;
                    Mappos[i, j].TileColor = new Color(255, 255, 255, 100);
                    Mappos[i, j].TilePos = new Vector3(firstPosCol + (j * firstLineCol) + ((i % 2 == 0) ? secondLineCol : 0),// 0 + 1*n + (0.75 or 0)
                        firstPosRow + (i * firstLineRow),// 0 + 0.75 * m
                        0);
                    Debug.Log("position : " + Mappos[i, j].TilePos);
                    GameObject Tile = Instantiate(normalPrefab);
                    Tile.transform.position = Mappos[i, j].TilePos;
                    SpriteRenderer TileSprite = Tile.GetComponent<SpriteRenderer>();
                    TileSprite.color = Mappos[i, j].TileColor;
                }
                
            }
        }

        tileInfo.pos = Mappos;
    }
}

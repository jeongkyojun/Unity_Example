using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct TileInfo
{
    public position[,] pos;
};
public struct position
{
    public int row, col;
    public bool isSet;
    public bool isTrue;
    public Color TileColor;
    public Vector3 TilePos;
};

public class GameManager : MonoBehaviour
{
    public GameObject normalPrefab;
    public GameObject cityPrefab;
    GameObject[,] gameArr;
    TileInfo tileInfo = new TileInfo();
    // Start is called before the first frame update
    void Start()
    {
        GenerateMap(ref tileInfo,ref gameArr);
    }

    void GenerateMap(ref TileInfo tileInfo,ref GameObject[,] gameArr)
    {
        int RowSize = 70;
        int ColSize = 70;
        float firstPosRow = 0;
        float firstPosCol = 0;

        float firstLineRow = 0.75f;
        float firstLineCol = 1f;

        float secondLineCol = 0.5f;

        float isred=1, isblue=1, isgreen=1;

        position[,] Mappos = new position[RowSize,ColSize];
        gameArr = new GameObject[RowSize, ColSize];
        for(int i=0;i<RowSize;i++)
        {
            for (int j = 0; j < ColSize; j++)
            {
                if (i / (RowSize / 3 + 1) == 0 || j / (RowSize / 3 + 1) == 0)
                    isred = 0;
                else
                    isred = 1;

                if (i / (RowSize / 3 + 1) == 1 || j / (RowSize / 3 + 1) == 1)
                    isblue = 0;
                else
                    isblue = 1;

                if (i / (RowSize / 3 + 1) == 2 || j / (RowSize / 3 + 1) == 2)
                    isgreen = 0;
                else
                    isgreen = 1;


                Debug.Log(i + " , " + j);

                // 맵 정보 구조체에 정보 저장
                Mappos[i, j].row = i;
                Mappos[i, j].col = j;
                Mappos[i, j].isTrue = true;
                Mappos[i, j].TileColor = new Color(isred, isgreen, isblue, 100);
                Mappos[i, j].TilePos = new Vector3(firstPosCol + (j * firstLineCol) + ((i % 2 == 0) ? secondLineCol : 0),// 0 + 1*n + (0.75 or 0)
                    firstPosRow + (i * firstLineRow),// 0 + 0.75 * m
                    0);
                Debug.Log("position : " + Mappos[i, j].TilePos);

                // 프리팹 생성 & 게임 오브젝트 배열에 저장
                gameArr[i, j] = Instantiate(normalPrefab);
                gameArr[i, j].transform.position = Mappos[i, j].TilePos;
                TileBtn tileBtn = gameArr[i, j].GetComponentInChildren<TileBtn>();
                tileBtn.row = i;
                tileBtn.col = j;
                tileBtn.Tilesprite.color = Mappos[i, j].TileColor;


            }
        }
        tileInfo.pos = Mappos;
    }
}

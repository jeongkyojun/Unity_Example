using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct TileEntity
{
    public Pos[,] Poses;
}

public struct Pos
{
    // 위치, 
    public Vector3 TilePosition;
    public int X, Y;
    public Color defaultColor;
    public Color selectColor;
};

public class GameManager : MonoBehaviour
{
    int MaxX = 80;
    int MaxY = 80;

    public GameObject normalMapTile;

    Vector3 up = Vector3.up;
    Vector3 right = Vector3.right;

    TileEntity TE;
    GameObject[,] TilesArr;
    TileInfo[,] TileInfoArr;

    Plane GroupPlane = new Plane(Vector3.zero, Vector3.forward);
    // Start is called before the first frame update
    void Start()
    {
        GenerateMap(ref TE, ref TilesArr, ref TileInfoArr);
    }

    // Update is called once per frame
    void Update()
    {
        SelectTile(TE,TilesArr,TileInfoArr);
    }

    void GenerateMap(ref TileEntity Tiles, ref GameObject[,] TileObjs, ref TileInfo[,] TileInfos)
    {
        Tiles.Poses = new Pos[MaxX,MaxY];
        GameObject[,] TileTmps = new GameObject[MaxX, MaxY];
        TileInfo[,] TileInfoTmps = new TileInfo[MaxX, MaxY];

        Color defaultColor = new Color(76, 128, 35, 255);
        Color SelectedColor = new Color(76, 128, 35, 155);

        float firstPosX = 0.5f;
        float firstPosY = 0.5f;

        for(int i=0;i<MaxY;i++)
        {
            for(int j=0;j<MaxX;j++)
            {
                Tiles.Poses[j, i].X = j;
                Tiles.Poses[j, i].Y = i;
                Tiles.Poses[j, i].TilePosition = right * (j+firstPosX) + up * (i+firstPosY);
                Tiles.Poses[j, i].defaultColor = defaultColor / 255f;
                Tiles.Poses[j, i].selectColor = SelectedColor / 255f;

                TileTmps[j, i] = Instantiate(normalMapTile);
                TileInfoTmps[j, i] = TileTmps[j, i].GetComponentInChildren<TileInfo>();

                TileTmps[j, i].transform.position = Tiles.Poses[j, i].TilePosition;
                TileInfoTmps[j, i].TileSprite.color = defaultColor / 255f;
            }
        }
        TileObjs = TileTmps;
        TileInfos = TileInfoTmps;
    }

    void SelectTile(TileEntity Tiles, GameObject[,] TileObjs, TileInfo[,] TileInfos)
    {

    }
}

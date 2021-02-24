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
    public bool isTrue; // 타일 통과 가능 여부
    public Vector3 TilePosition; // 타일위치
    public int X, Y;
    public int TileEnv; // 타일환경 0:불, 1:얼음, 2:바람, 3:땅
    public int Environment; // 세부환경 ex)늪, 얼음, 바위, 용암, 땅 등...
    public Color defaultColor;
    public Color selectColor;
};

public class GameManager : MonoBehaviour
{
    // 81 + 9 + 81 + 9 + 81 = 9 * (29)
    int MaxX = 261;
    int MaxY = 261; 

    public GameObject normalMapTile;
    public GameObject FireMapTile;
    public GameObject IceMapTile;
    public GameObject WindMapTile;
    public GameObject EarthMapTile;
    public GameObject VoidMapTile;
    int[] TileSet = new int[4];
    int maxnum = 0;

    public GameObject player;

    PlayerManaging playerInfo;

    Vector3 up = Vector3.up;
    Vector3 right = Vector3.right;
    Vector3 foward = Vector3.forward;

    TileEntity TE;
    GameObject[,] TilesArr;
    TileInfo[,] TileInfoArr;

    Plane GroupPlane = new Plane(Vector3.zero, Vector3.forward);
    // Start is called before the first frame update
    void Start()
    {
        playerInfo = FindObjectOfType<PlayerManaging>();

        // 각 속성 타일의 수를 정한다.
        int minRange = 3;
        int maxRange = 5;
        for(int i=0;i<4;i++)
        {
            TileSet[i] = Random.Range(minRange,maxRange);
        }
        GenerateMap(ref TE, ref TilesArr, ref TileInfoArr);
        while (true)
        {
            playerInfo.X = Random.Range(0, MaxX);
            playerInfo.Y = Random.Range(0, MaxY);
            if(TE.Poses[playerInfo.X,playerInfo.Y].isTrue)
                break;
        }
        player.transform.position = up * (playerInfo.Y + 0.5f) + right*(playerInfo.X + 0.5f)+foward*-3f;
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

        // 0 ~ 9(row/29) * 9 , 9*9 + 9 ~ 9*18 + 9
        // (maxX/29)* i * 10 ~ (maxX/29)*(i+1)*10
        // 9 * 0 ~ 9*9
        int Type;
        int select;
        for (int i=0;i<3;i++)
        {
            for(int j=0;j<3;j++)
            {
                // 속성을 정한다.
                while (true)
                {
                    select = Random.Range(0, 4);
                    if (TileSet[select] > 0)
                        break;
                }
                TileSet[select]--;
                Type = select;
                for(int k= (MaxX/29)*i*10;k<(MaxX/29)*(i+1)*9 + (MaxX/29)*i;k++)
                {
                    for (int l = (MaxX / 29) * j * 10; l < (MaxX / 29) * (j + 1) * 9 + (MaxX / 29) * j; l++)
                    {
                        switch(Type)
                        {
                            case 0:
                                TileTmps[l, k] = Instantiate(FireMapTile);
                                break;
                            case 1:
                                TileTmps[l, k] = Instantiate(IceMapTile);
                                break;
                            case 2:
                                TileTmps[l, k] = Instantiate(WindMapTile);
                                break;
                            case 3:
                                TileTmps[l, k] = Instantiate(EarthMapTile);
                                break;
                        }
                        TileInfoTmps[l, k] = TileTmps[l, k].GetComponentInChildren<TileInfo>();
                        Tiles.Poses[l, k].X = l;
                        Tiles.Poses[l, k].Y = k;
                        Tiles.Poses[l, k].TilePosition = right * (l + firstPosX) + up * (k + firstPosY);
                        Tiles.Poses[l, k].defaultColor = TileInfoTmps[l, k].TileSprite.color;
                        Tiles.Poses[l, k].selectColor = TileInfoTmps[l,k].TileSprite.color/2f;
                        Tiles.Poses[l, k].isTrue = true;

                        TileTmps[l, k].transform.position = Tiles.Poses[l, k].TilePosition;
                    }
                    if (j != 2)
                    {
                        for (int l = (MaxX / 29) * (j + 1) * 9 + (MaxX / 29) * j; l < (MaxX / 29) * (j + 1) * 10; l++)
                        {
                            TileTmps[l, k] = Instantiate(VoidMapTile);
                            TileInfoTmps[l, k] = TileTmps[l, k].GetComponentInChildren<TileInfo>();
                            Tiles.Poses[l, k].X = l;
                            Tiles.Poses[l, k].Y = k;
                            Tiles.Poses[l, k].TilePosition = right * (l + firstPosX) + up * (k + firstPosY);
                            Tiles.Poses[l, k].defaultColor = TileInfoTmps[l, k].TileSprite.color;
                            Tiles.Poses[l, k].selectColor = TileInfoTmps[l, k].TileSprite.color / 2f;
                            Tiles.Poses[l, k].isTrue = true;

                            TileTmps[l, k].transform.position = Tiles.Poses[l, k].TilePosition;
                        }
                    }
                }
                if (i != 2)
                {
                    for (int k = (MaxX / 29) * (i + 1) * 9 + (MaxX / 29) * i; k < (MaxX / 29) * (i + 1) * 10; k++)
                    {
                        for (int l = (MaxX / 29) * j * 10; l < (MaxX / 29) * (j + 1) * 9 + (MaxX / 29) * j; l++)
                        {
                            TileTmps[l, k] = Instantiate(VoidMapTile);
                            TileInfoTmps[l, k] = TileTmps[l, k].GetComponentInChildren<TileInfo>();
                            Tiles.Poses[l, k].X = l;
                            Tiles.Poses[l, k].Y = k;
                            Tiles.Poses[l, k].TilePosition = right * (l + firstPosX) + up * (k + firstPosY);
                            Tiles.Poses[l, k].defaultColor = TileInfoTmps[l, k].TileSprite.color;
                            Tiles.Poses[l, k].selectColor = TileInfoTmps[l, k].TileSprite.color / 2f;
                            Tiles.Poses[l, k].isTrue = true;

                            TileTmps[l, k].transform.position = Tiles.Poses[l, k].TilePosition;
                        }
                        if (j != 2)
                        {
                            for (int l = (MaxX / 29) * (j + 1) * 9 + (MaxX / 29) * j; l < (MaxX / 29) * (j + 1) * 10; l++)
                            {
                                TileTmps[l, k] = Instantiate(VoidMapTile);
                                TileInfoTmps[l, k] = TileTmps[l, k].GetComponentInChildren<TileInfo>();
                                Tiles.Poses[l, k].X = l;
                                Tiles.Poses[l, k].Y = k;
                                Tiles.Poses[l, k].TilePosition = right * (l + firstPosX) + up * (k + firstPosY);
                                Tiles.Poses[l, k].defaultColor = TileInfoTmps[l, k].TileSprite.color;
                                Tiles.Poses[l, k].selectColor = TileInfoTmps[l, k].TileSprite.color / 2f;
                                Tiles.Poses[l, k].isTrue = true;

                                TileTmps[l, k].transform.position = Tiles.Poses[l, k].TilePosition;
                            }
                        }
                    }
                }
            }
        }

        TileObjs = TileTmps;
        TileInfos = TileInfoTmps;
    }

    void SelectTile(TileEntity Tiles, GameObject[,] TileObjs, TileInfo[,] TileInfos)
    {

    }
}

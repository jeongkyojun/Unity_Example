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
    public bool isSet; // 이벤트 또는 몬스터 세팅 여부
    public bool isLooked; // 시야가 밝혀졌었는지 여부 -> 암흑상태 확인
    public bool isLooking; // 시야 안에 있는지 여부 -> 전장의 안개 실행

    public Vector3 TilePosition; // 타일위치
    public int X, Y;
    public int TileEnv; // 타일환경 0:불, 1:얼음, 2:바람, 3:땅
    public int Environment; // 세부환경 ex)늪, 얼음, 바위, 용암, 땅 등...
    public int TileStatus; // 타일 상태 여부 - 몬스터나 어떠한 이벤트가 있는지 확인

    public Color defaultColor;
};

public class GameManager : MonoBehaviour
{
    static int BigGrid = 27;
    static int boundary = 3;

    static int MaxX = BigGrid * 3 + boundary * 2;
    static int MaxY = BigGrid * 3 + boundary * 2;
    float TileXSize = 2;
    float TileYSize = 2;

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

    bool isMoved;
    int turn;

    Plane GroupPlane = new Plane(Vector3.zero, Vector3.forward);
    // Start is called before the first frame update
    void Start()
    {
        playerInfo = FindObjectOfType<PlayerManaging>();
        isMoved = false;
        turn = 1;
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
        player.transform.position = up * (TileYSize*playerInfo.Y + TileYSize/2) + right*(TileXSize*playerInfo.X + TileXSize/2)+foward*-3f;
    }

    // Update is called once per frame
    void Update()
    {
        if (turn == 1)
        {
            if (isMoved)
            {
               //MoveTo(player.transform.position,, player);
            }
        }
        else
        {

        }

    }

    void GenerateMap(ref TileEntity Tiles, ref GameObject[,] TileObjs, ref TileInfo[,] TileInfos)
    {
        Tiles.Poses = new Pos[MaxX,MaxY];
        GameObject[,] TileTmps = new GameObject[MaxX, MaxY];
        TileInfo[,] TileInfoTmps = new TileInfo[MaxX, MaxY];

        Color defaultColor = new Color(76, 128, 35, 255);
        Color SelectedColor = new Color(76, 128, 35, 155);

        float firstPosX = TileXSize/2;
        float firstPosY = TileYSize/2;

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
                for(int k= BigGrid*i+boundary*i;k<BigGrid*(i+1)+boundary*i;k++)
                {
                    for (int l = BigGrid * j + boundary * j; l < BigGrid * (j + 1) + boundary * j; l++)
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
                        Tiles.Poses[l, k].TileEnv = Type;
                        Tiles.Poses[l, k].TilePosition = right * (l*TileXSize + firstPosX) + up * (k*TileYSize+ firstPosY); // transform.position (위치) 설정
                        Tiles.Poses[l, k].defaultColor = TileInfoTmps[l, k].TileSprite.color; // 타일 칼라 저장
                        Tiles.Poses[l, k].isTrue = true;

                        TileTmps[l, k].transform.position = Tiles.Poses[l, k].TilePosition;
                    }
                    if (j != 2)
                    {
                        for (int l = BigGrid*(j+1)+boundary*j; l < BigGrid*(j+1)+boundary*(j+1); l++)
                        {
                            TileTmps[l, k] = Instantiate(VoidMapTile);
                            TileInfoTmps[l, k] = TileTmps[l, k].GetComponentInChildren<TileInfo>();
                            Tiles.Poses[l, k].X = l;
                            Tiles.Poses[l, k].Y = k;
                            Tiles.Poses[l, k].TilePosition = right * (l * TileXSize + firstPosX) + up * (k * TileYSize + firstPosY);
                            Tiles.Poses[l, k].defaultColor = TileInfoTmps[l, k].TileSprite.color;
                            Tiles.Poses[l, k].isTrue = false;

                            TileTmps[l, k].transform.position = Tiles.Poses[l, k].TilePosition;
                        }
                    }
                }
                if (i != 2)
                {
                    for (int k = BigGrid*(i+1)+boundary*i; k < BigGrid * (i + 1) + boundary * (i+1); k++)
                    {
                        for (int l = BigGrid * j + boundary * j; l < BigGrid * (j + 1) + boundary * j; l++)
                        {
                            TileTmps[l, k] = Instantiate(VoidMapTile);
                            TileInfoTmps[l, k] = TileTmps[l, k].GetComponentInChildren<TileInfo>();
                            Tiles.Poses[l, k].X = l;
                            Tiles.Poses[l, k].Y = k;
                            Tiles.Poses[l, k].TilePosition = right * (l * TileXSize + firstPosX) + up * (k * TileYSize + firstPosY);
                            Tiles.Poses[l, k].defaultColor = TileInfoTmps[l, k].TileSprite.color;
                            Tiles.Poses[l, k].isTrue = false;

                            TileTmps[l, k].transform.position = Tiles.Poses[l, k].TilePosition;
                        }
                        if (j != 2)
                        {
                            for (int l = BigGrid * (j + 1) + boundary * j; l < BigGrid * (j + 1) + boundary * (j + 1); l++)
                            {
                                TileTmps[l, k] = Instantiate(VoidMapTile);
                                TileInfoTmps[l, k] = TileTmps[l, k].GetComponentInChildren<TileInfo>();
                                Tiles.Poses[l, k].X = l;
                                Tiles.Poses[l, k].Y = k;
                                Tiles.Poses[l, k].TilePosition = right * (l * TileXSize + firstPosX) + up * (k * TileYSize + firstPosY);
                                Tiles.Poses[l, k].defaultColor = TileInfoTmps[l, k].TileSprite.color;
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

    void MoveTo(Vector3 StartPosition, Vector3 DestPosition,GameObject gameObject)
    {
        gameObject.transform.position += (DestPosition - StartPosition) * Time.deltaTime;
    }
}

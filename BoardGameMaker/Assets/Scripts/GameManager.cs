using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct TileEntity
{
    public Pos[,] Poses;
    public int[,] GridEnv;
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
    static int boundary = 10;
    static int boarder = 5;

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
    int[] TileSet = new int[5];
    int maxnum = 0;

    public GameObject player;

    PlayerManaging playerInfo;

    Vector3 up = Vector3.up;
    Vector3 right = Vector3.right;
    Vector3 foward = Vector3.forward;

    TileEntity TE;
    GameObject[,] TilesArr;
    TileInfo[,] TileInfoArr;

    bool isMoved, YMoved=false, XMoved=false;
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
        TileSet[4] = 2;
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
            else
            {

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
        Tiles.GridEnv = new int[3, 3];

        Color defaultColor = new Color(76, 128, 35, 255);
        Color SelectedColor = new Color(76, 128, 35, 155);

        float firstPosX = TileXSize/2;
        float firstPosY = TileYSize/2;

        // 0 ~ 9(row/29) * 9 , 9*9 + 9 ~ 9*18 + 9
        // (maxX/29)* i * 10 ~ (maxX/29)*(i+1)*10
        // 9 * 0 ~ 9*9
        int Type;
        int select;

        for(int i=0;i<3;i++)
        {
            for(int j=0;j<3;j++)
            {
                while (true)
                {
                    select = Random.Range(0, 5);
                    if (TileSet[select] > 0)
                        break;
                }
                TileSet[select]--;
                Tiles.GridEnv[i, j] = select;
            }
            Debug.Log(Tiles.GridEnv[i,0]+" , "+ Tiles.GridEnv[i, 1]+" , "+ Tiles.GridEnv[i, 2]);
        }

        for (int i=0;i<3;i++)
        {
            for(int j=0;j<3;j++)
            {
                for(int k= BigGrid*i+boundary*i;k<BigGrid*(i+1)+boundary*i;k++)
                {
                    for (int l = BigGrid * j + boundary * j; l < BigGrid * (j + 1) + boundary * j; l++)
                    {
                        switch(Tiles.GridEnv[i,j])
                        {
                            case 0:
                                TileTmps[l, k] = Instantiate(FireMapTile);
                                Tiles.Poses[l, k].isTrue = true;
                                break;
                            case 1:
                                TileTmps[l, k] = Instantiate(IceMapTile);
                                Tiles.Poses[l, k].isTrue = true;
                                break;
                            case 2:
                                TileTmps[l, k] = Instantiate(WindMapTile);
                                Tiles.Poses[l, k].isTrue = true;
                                break;
                            case 3:
                                TileTmps[l, k] = Instantiate(EarthMapTile);
                                Tiles.Poses[l, k].isTrue = true;
                                break;
                            case 4:
                                TileTmps[l, k] = Instantiate(VoidMapTile);
                                Tiles.Poses[l, k].isTrue = false;
                                break;
                        }
                        TileInfoTmps[l, k] = TileTmps[l, k].GetComponentInChildren<TileInfo>();
                        Tiles.Poses[l, k].X = l;
                        Tiles.Poses[l, k].Y = k;
                        Tiles.Poses[l, k].TileEnv = Tiles.GridEnv[j,i];
                        Tiles.Poses[l, k].TilePosition = right * (l*TileXSize + firstPosX) + up * (k*TileYSize+ firstPosY); // transform.position (위치) 설정
                        Tiles.Poses[l, k].defaultColor = TileInfoTmps[l, k].TileSprite.color; // 타일 칼라 저장
                        

                        TileTmps[l, k].transform.position = Tiles.Poses[l, k].TilePosition;
                    }

                    if (j != 2)
                    {
                        for (int l = BigGrid*(j+1)+boundary*j; l < BigGrid*(j+1)+boundary*(j+1); l++)
                        {
                            //좌/우 간 타일 비교
                            int boundaryNum = Random.Range(-1, boundary+1);
                            if(l<=BigGrid*(j+1)+boundary*j+boundaryNum)
                            {
                                switch (Tiles.GridEnv[i, j])
                                {
                                    case 0:
                                        TileTmps[l, k] = Instantiate(FireMapTile);
                                        Tiles.Poses[l, k].isTrue = true;
                                        break;
                                    case 1:
                                        TileTmps[l, k] = Instantiate(IceMapTile);
                                        Tiles.Poses[l, k].isTrue = true;
                                        break;
                                    case 2:
                                        TileTmps[l, k] = Instantiate(WindMapTile);
                                        Tiles.Poses[l, k].isTrue = true;
                                        break;
                                    case 3:
                                        TileTmps[l, k] = Instantiate(EarthMapTile);
                                        Tiles.Poses[l, k].isTrue = true;
                                        break;
                                    case 4:
                                        TileTmps[l, k] = Instantiate(VoidMapTile);
                                        Tiles.Poses[l, k].isTrue = false;
                                        break;
                                }
                            }
                            else
                            {
                                switch (Tiles.GridEnv[i, j+1])
                                {
                                    case 0:
                                        TileTmps[l, k] = Instantiate(FireMapTile);
                                        Tiles.Poses[l, k].isTrue = true;
                                        break;
                                    case 1:
                                        TileTmps[l, k] = Instantiate(IceMapTile);
                                        Tiles.Poses[l, k].isTrue = true;
                                        break;
                                    case 2:
                                        TileTmps[l, k] = Instantiate(WindMapTile);
                                        Tiles.Poses[l, k].isTrue = true;
                                        break;
                                    case 3:
                                        TileTmps[l, k] = Instantiate(EarthMapTile);
                                        Tiles.Poses[l, k].isTrue = true;
                                        break;
                                    case 4:
                                        TileTmps[l, k] = Instantiate(VoidMapTile);
                                        Tiles.Poses[l, k].isTrue = false;
                                        break;
                                }
                            }
                            TileInfoTmps[l, k] = TileTmps[l, k].GetComponentInChildren<TileInfo>();
                            Tiles.Poses[l, k].X = l;
                            Tiles.Poses[l, k].Y = k;
                            Tiles.Poses[l, k].TilePosition = right * (l * TileXSize + firstPosX) + up * (k * TileYSize + firstPosY);
                            Tiles.Poses[l, k].defaultColor = TileInfoTmps[l, k].TileSprite.color;
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
                            int boundaryNum = Random.Range(-1, boundary+1);
                            if (k <= BigGrid * (i + 1) + boundary * i  + boundaryNum)
                            {
                                switch (Tiles.GridEnv[i, j])
                                {
                                    case 0:
                                        TileTmps[l, k] = Instantiate(FireMapTile);
                                        Tiles.Poses[l, k].isTrue = true;
                                        break;
                                    case 1:
                                        TileTmps[l, k] = Instantiate(IceMapTile);
                                        Tiles.Poses[l, k].isTrue = true;
                                        break;
                                    case 2:
                                        TileTmps[l, k] = Instantiate(WindMapTile);
                                        Tiles.Poses[l, k].isTrue = true;
                                        break;
                                    case 3:
                                        TileTmps[l, k] = Instantiate(EarthMapTile);
                                        Tiles.Poses[l, k].isTrue = true;
                                        break;
                                    case 4:
                                        TileTmps[l, k] = Instantiate(VoidMapTile);
                                        Tiles.Poses[l, k].isTrue = false;
                                        break;
                                }
                            }
                            else
                            {
                                switch (Tiles.GridEnv[i+1, j])
                                {
                                    case 0:
                                        TileTmps[l, k] = Instantiate(FireMapTile);
                                        Tiles.Poses[l, k].isTrue = true;
                                        break;
                                    case 1:
                                        TileTmps[l, k] = Instantiate(IceMapTile);
                                        Tiles.Poses[l, k].isTrue = true;
                                        break;
                                    case 2:
                                        TileTmps[l, k] = Instantiate(WindMapTile);
                                        Tiles.Poses[l, k].isTrue = true;
                                        break;
                                    case 3:
                                        TileTmps[l, k] = Instantiate(EarthMapTile);
                                        Tiles.Poses[l, k].isTrue = true;
                                        break;
                                    case 4:
                                        TileTmps[l, k] = Instantiate(VoidMapTile);
                                        Tiles.Poses[l, k].isTrue = false;
                                        break;
                                }
                            }
                            TileInfoTmps[l, k] = TileTmps[l, k].GetComponentInChildren<TileInfo>();
                            Tiles.Poses[l, k].X = l;
                            Tiles.Poses[l, k].Y = k;
                            Tiles.Poses[l, k].TilePosition = right * (l * TileXSize + firstPosX) + up * (k * TileYSize + firstPosY);
                            Tiles.Poses[l, k].defaultColor = TileInfoTmps[l, k].TileSprite.color;
                            TileTmps[l, k].transform.position = Tiles.Poses[l, k].TilePosition;
                        }
                        if (j != 2)
                        {
                            for (int l = BigGrid * (j + 1) + boundary * j; l < BigGrid * (j + 1) + boundary * (j + 1); l++)
                            {
                                int boundaryX = Random.Range(-1, boundary+1);
                                int boundaryY = Random.Range(-1, boundary+1);
                                int x = j;
                                int y = i;
                                //세로 -> i 관련해서
                                if (k > BigGrid * (i + 1) + boundary * i + boundaryY)
                                {
                                    y++;
                                }
                                if(l <= BigGrid * (j + 1) + boundary * j + boundaryX)
                                {
                                    x++;
                                }
                                switch (Tiles.GridEnv[y, x])
                                {
                                    case 0:
                                        TileTmps[l, k] = Instantiate(FireMapTile);
                                        Tiles.Poses[l, k].isTrue = true;
                                        break;
                                    case 1:
                                        TileTmps[l, k] = Instantiate(IceMapTile);
                                        Tiles.Poses[l, k].isTrue = true;
                                        break;
                                    case 2:
                                        TileTmps[l, k] = Instantiate(WindMapTile);
                                        Tiles.Poses[l, k].isTrue = true;
                                        break;
                                    case 3:
                                        TileTmps[l, k] = Instantiate(EarthMapTile);
                                        Tiles.Poses[l, k].isTrue = true;
                                        break;
                                    case 4:
                                        TileTmps[l, k] = Instantiate(VoidMapTile);
                                        Tiles.Poses[l, k].isTrue = false;
                                        break;
                                }
                                TileInfoTmps[l, k] = TileTmps[l, k].GetComponentInChildren<TileInfo>();
                                Tiles.Poses[l, k].X = l;
                                Tiles.Poses[l, k].Y = k;
                                Tiles.Poses[l, k].TilePosition = right * (l * TileXSize + firstPosX) + up * (k * TileYSize + firstPosY);
                                Tiles.Poses[l, k].defaultColor = TileInfoTmps[l, k].TileSprite.color;

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

    void MoveTo(Vector3 StartPosition, Vector3 DestPosition, GameObject gameObject)
    {
        gameObject.transform.position += (DestPosition - StartPosition) * Time.deltaTime;
        if (DestPosition.y < StartPosition.y)
        {
            if (gameObject.transform.position.y < DestPosition.y)
            {
                gameObject.transform.position = Vector3.right * gameObject.transform.position.x + Vector3.up * DestPosition.y + Vector3.forward * gameObject.transform.position.z;
                YMoved = true;
            }
        }
        else
        {
            if (gameObject.transform.position.y > DestPosition.y)
            {
                gameObject.transform.position = Vector3.right * gameObject.transform.position.x + Vector3.up * DestPosition.y + Vector3.forward * gameObject.transform.position.z;
                YMoved = true;
            }
        }

        if(DestPosition.x < StartPosition.y)
        {
            if (gameObject.transform.position.x < DestPosition.x)
            {
                gameObject.transform.position = Vector3.right * DestPosition.x + Vector3.up * gameObject.transform.position.y + Vector3.forward * gameObject.transform.position.z;
                XMoved = true;
            }
        }
        else
        {
            if (gameObject.transform.position.x > DestPosition.x)
            {
                gameObject.transform.position = Vector3.right * DestPosition.x + Vector3.up * gameObject.transform.position.y + Vector3.forward * gameObject.transform.position.z;
                XMoved = true;
            }
        }
         
        if(XMoved && YMoved)
        {
            XMoved = false;
            YMoved = false;
            isMoved = false;
        }
    }
}

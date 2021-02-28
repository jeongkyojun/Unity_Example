using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct TileEntity
{
    public Pos[,] Poses;
    public bool[,] TileSetting;
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
    static int BigGrid = 9; // 기준이 되는 그리드의 크기
    static int boundary = 10; // 그리드의 경계너비
    static int boarder = 5; // 그리드 밖 경계(아직 미구현)
    static int GridNum = 10; // 1행/1열당 그리드의 개수

    static float setPercent = 90f;
    static float stopPercent = 100 - setPercent;

    static int MaxX = BigGrid * GridNum + boundary * (GridNum-1);
    static int MaxY = BigGrid * GridNum + boundary * (GridNum-1);
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
    public GameObject monster;

    PlayerManaging playerInfo;

    Vector3 up = Vector3.up;
    Vector3 right = Vector3.right;
    Vector3 foward = Vector3.forward;

    public TileEntity TE;
    GameObject[,] TilesArr;

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
        int minRange = (GridNum*GridNum)/4;
        int maxRange = (GridNum*GridNum)/4+2;
        for(int i=0;i<4;i++)
        {
            TileSet[i] = Random.Range(minRange,maxRange);
        }
        //TileSet[4] = GridNum*GridNum*2/9;
        TileSet[4] = 0;
        GenerateMap(ref TE, ref TilesArr);
        playerInfo.MaxX = MaxX;
        playerInfo.MaxY = MaxY;
        playerInfo.TileXSize = TileXSize;
        playerInfo.TileYSize = TileYSize;
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

    void GenerateMap(ref TileEntity Tiles, ref GameObject[,] TileObjs)
    {
        Tiles.Poses = new Pos[MaxX,MaxY];
        GameObject[,] TileTmps = new GameObject[MaxX, MaxY];
        Tiles.TileSetting = new bool[MaxX, MaxY];

        float firstPosX = TileXSize/2;
        float firstPosY = TileYSize/2;

        // 0 ~ 9(row/29) * 9 , 9*9 + 9 ~ 9*18 + 9
        // (maxX/29)* i * 10 ~ (maxX/29)*(i+1)*10
        // 9 * 0 ~ 9*9
        int SettingEnv,Select;
        int Xpos, Ypos;
        for (int i= 0;i<MaxY;i++)
        {
            for(int j=0;j<MaxX;j++)
            {
                Tiles.TileSetting[i, j] = false;
                Tiles.Poses[i, j].TileEnv= 4;
            }
        }
        
        for (int i=0;i<GridNum;i++)
        {
            for(int j=0;j<GridNum;j++)
            {
                /*
                while (true)
                {
                    SettingEnv = Random.Range(0, 5);
                    if (TileSet[SettingEnv] > 0)
                        break;
                }
                */
                Select = Random.Range(0, 101);

                // i 가 0에 가까울수록 얼음(1)타일의 확률이 늘어나며, i가 최대값에 가까울수록 불(0)타일의 확률이 늘어난다.
                // j 가 0에 가까울수록 고원(2)타일의 확률이 늘어나며, j가 최대값에 가까울수록 숲(3)타일의 확률이 늘어난다.
                if (Select<= 50 / (GridNum-1) * i) // 0 이면 Gridnum + -Gridnum / Gridnum * 25
                    SettingEnv = 0;
                else if (Select <=50)
                    SettingEnv = 1;
                else if (Select <= 50/(GridNum-1) * j + 50)
                    SettingEnv = 2;
                else
                    SettingEnv = 3;

                Ypos = Random.Range(BigGrid * i+boundary*i, BigGrid * (i + 1) + boundary * i);
                Xpos = Random.Range(BigGrid * j + boundary * j, BigGrid * (j + 1) + boundary * j);
                if (!Tiles.TileSetting[Ypos,Xpos])
                {
                    Debug.Log("[ "+j+" , "+i+" ] Env : " + SettingEnv);
                    GenerateTile(ref Tiles, setPercent, stopPercent, Xpos, Ypos, SettingEnv);
                    TileSet[SettingEnv]--;
                }
            }
        }
        for(int i=0;i<MaxY;i++)
        {
            for(int j=0;j<MaxX;j++)
            {
                if (Tiles.TileSetting[j, i])
                {
                    switch (Tiles.Poses[j, i].TileEnv)
                    {
                        case 0:
                            TileTmps[j, i] = Instantiate(FireMapTile);
                            Tiles.Poses[j, i].isTrue = true;
                            break;
                        case 1:
                            TileTmps[j, i] = Instantiate(IceMapTile);
                            Tiles.Poses[j, i].isTrue = true;
                            break;
                        case 2:
                            TileTmps[j, i] = Instantiate(WindMapTile);
                            Tiles.Poses[j, i].isTrue = true;
                            break;
                        case 3:
                            TileTmps[j, i] = Instantiate(EarthMapTile);
                            Tiles.Poses[j, i].isTrue = true;
                            break;
                        case 4:
                            TileTmps[j, i] = Instantiate(VoidMapTile);
                            Tiles.Poses[j, i].isTrue = false;
                            break;
                    }
                }
                else
                {
                    if (i > 0 && j > 0 && i < MaxY - 1 && j < MaxX - 1
                        && Tiles.TileSetting[j, i - 1] &&Tiles.TileSetting[j, i + 1] && Tiles.TileSetting[j - 1, i] && Tiles.TileSetting[j + 1, i]
                        &&Tiles.Poses[j,i-1].TileEnv==Tiles.Poses[j,i+1].TileEnv && Tiles.Poses[j,i-1].TileEnv==Tiles.Poses[j+1,i].TileEnv && Tiles.Poses[j, i - 1].TileEnv == Tiles.Poses[j - 1, i].TileEnv)
                    {
                        switch (Tiles.Poses[j-1, i].TileEnv)
                        {
                            case 0:
                                TileTmps[j, i] = Instantiate(FireMapTile);
                                Tiles.Poses[j, i].isTrue = true;
                                break;
                            case 1:
                                TileTmps[j, i] = Instantiate(IceMapTile);
                                Tiles.Poses[j, i].isTrue = true;
                                break;
                            case 2:
                                TileTmps[j, i] = Instantiate(WindMapTile);
                                Tiles.Poses[j, i].isTrue = true;
                                break;
                            case 3:
                                TileTmps[j, i] = Instantiate(EarthMapTile);
                                Tiles.Poses[j, i].isTrue = true;
                                break;
                            case 4:
                                TileTmps[j, i] = Instantiate(VoidMapTile);
                                Tiles.Poses[j, i].isTrue = false;
                                break;
                        }
                    }
                    else
                    {
                        TileTmps[j, i] = Instantiate(VoidMapTile);
                        Tiles.Poses[j, i].isTrue = false;
                    }
                }

                Tiles.Poses[j, i].X = j;
                Tiles.Poses[j, i].Y = i;
                Tiles.Poses[j, i].TilePosition = right * (j * TileXSize + firstPosX) + up * (i * TileYSize + firstPosY); // transform.position (위치) 설정

                TileTmps[j, i].transform.position = Tiles.Poses[j, i].TilePosition;
            }
        }
        TileObjs = TileTmps;
    }
    
    void GenerateTile(ref TileEntity tiles, float setPercent, float stopPercent, int Xpos, int Ypos, int SettingEnv)
    {
        // 타일 번호, 시작 지점, 퍼질 확률, 타일
        float changePercent = 0.1f;

        tiles.TileSetting[Ypos,Xpos] = true;
        tiles.Poses[Ypos, Xpos].TileEnv = SettingEnv;

        if(Xpos>0 && Random.Range(0,101)<setPercent && !tiles.TileSetting[Ypos,Xpos-1])
            GenerateTile(ref tiles, setPercent - changePercent, stopPercent + changePercent, Xpos - 1, Ypos, SettingEnv);
        if(Xpos<MaxX-1 && Random.Range(0,101)<setPercent && !tiles.TileSetting[Ypos, Xpos+1])
            GenerateTile(ref tiles, setPercent - changePercent, stopPercent + changePercent, Xpos + 1, Ypos,SettingEnv);
        if (Ypos > 0 && Random.Range(0, 101) < setPercent && !tiles.TileSetting[Ypos-1, Xpos])
            GenerateTile(ref tiles, setPercent - changePercent, stopPercent + changePercent, Xpos, Ypos-1,SettingEnv);
        if (Ypos < MaxY-1 && Random.Range(0, 101) < setPercent && !tiles.TileSetting[Ypos+1, Xpos])
            GenerateTile(ref tiles, setPercent - changePercent, stopPercent + changePercent, Xpos, Ypos+1,SettingEnv);
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

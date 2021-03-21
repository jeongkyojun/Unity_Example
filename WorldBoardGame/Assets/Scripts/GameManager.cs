using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public struct Tiles
{
    public int MapSeed;
    public Position[,] Poses;
    public int[,] TileNumber; // 타일마다의 대륙 번호
    public bool[,] TileSet; //타일 생성 여부

    public int[] isFire;   // 용암 개수 확인
    public int[] isWater;  // 물 개수 확인
    public int[] isCont;   // 대륙이 가지고 있는 땅 개수 확인
};

public struct Position
{
    public int x,y; // 가로, 세로
    public int tilenumber;// 타일 번호(공통인지 아닌지를 확인)

    public bool isGo; // 진입 가능 여부 확인
    
    public int water_info; // 0 이면 물, 1이면 용암, 2는 바다, 3은 땅
    
    public int TileEnv; // 환경 설정 0,1,2,3,4 로 구성, 4는 물
    public int Env; // 지형 ( 0:산 , 1+Tileenv : 해당 속성에 맞는 나무 숫자 (Tileenv는 최대 3까지), 이후 아직 안정해짐
                    // 만약, TileEnv = 4, 즉 물인 경우 0 : 바다, 1 : 호수, 2: 강, 3 : 용암, 4: 얼음 이다.
};

public struct point
{
    public int x;
    public int y;

    public int tilenumber;
    public int Env;
}

public class GameManager : MonoBehaviour
{
    // 맵 구성 관련 정보
    [Header("맵 관련 정보")]
    public int GridSize; // 기준이 되는 그리드의 크기
    public int border;   // 그리드 테두리의 너비

    // 세포 자동화 방식의 생존 및 사망 숫자
    [Header("셀룰러 오토마타 정보")]
    [Tooltip("맵 스케일링 당시 사용되는 숫자, 주위 블럭 중 땅의 개수가 이 숫자를 초과하여야 땅이 된다.")]
    public int TileSurviveNum;
    [Tooltip("초기 땅과 물을 구분할때 사용되는 확률")]
    [Range(0, 100)]
    public int randomfillPercent;
    [Tooltip("속성이 퍼져나갈때, 멈출지 퍼질지를 결정하는 확률")]
    [Range(0,100)]
    public int RandomOutPercent;
    [Tooltip("산이 생성될 확률")]
    [Range(0, 100)]
    public int RandomMountainPercent;
    [Tooltip("숲이 생성될 확률")]

    [Range(0, 100)]
    public int[] RandomForestPercent;
    [Tooltip("확률이 감소되는 정도")]
    public float DecreaseForestPercent;
    [Tooltip("강물이 퍼져나갈때, 갈라질지 진행할지를 결정하는 확률")]
    [Range(0, 100)]
    public int RandomSeparatePercent;

    [Header("반복횟수")]
    public int rotateNum;
    public int EnvNum;
    public int InputQueNumber;
    [Header("랜덤관련")]
    public int seed; // 랜덤 시드넘버

    [Header("타일 전체 범위")]
    public int MaxX;
    public int MaxY;

    [Header("타일 기본 사이즈")]
    public float tileXSize = 2;
    public float tileYSize = 2;

    [Header("이동 카운트 관련")]
    public TextMeshProUGUI MoveCntText;
    public int moveCnt;

    float firstPosX; // 첫 위치 지정
    float firstPosY; // 첫 위치 지정2

    // 등고선 관련
    [Header("지형 프리팹")]
    public GameObject[] high;
    [Header("환경 프리팹")]
    public GameObject[] env;

    // 변수
    [Header("동적 변수")]
    public Tiles TE = new Tiles();
    public GameObject[,] TilesArr; // 타일 오브젝트 저장
    GameObject[,] TileEnvArr;

    //기본 벡터
    Vector3 right = Vector3.right;
    Vector3 up = Vector3.up;
    Vector3 forward = Vector3.forward;

    //스크립트 관련 컴포넌트
    [Header("플레이어 관련")]
    public GameObject player;
    PlayerManaging playerScript;

    // Start is called before the first frame update
    void Start()
    {
        InitUI(5);

        MaxX = border * 2 + GridSize;
        MaxY = border * 2 + GridSize;

        seed = UnityEngine.Random.Range(00000000, 100000000);

        TilesArr = new GameObject[MaxX, MaxY];
        TileEnvArr = new GameObject[MaxX, MaxY];
        firstPosX = tileXSize / 2;
        firstPosY = tileYSize / 2;

        Debug.Log("seed : " + seed.ToString());
        UnityEngine.Random.InitState(seed);
        TE.MapSeed = seed;

        GenerateMap(ref TE); // 지형 생성 및 스케일링

        MakeMap(ref TE,ref TilesArr, ref TileEnvArr);

        PlayerSetting(ref TE);
    }

    // Update is called once per frame
    void Update()
    {
        if (moveCnt == 0)
        {
            InitUI(5);
        }
    }

    void InitUI(int GetCnt)
    {
        moveCnt = GetCnt;
        MoveCntText.text = "MoveCnt : " + moveCnt.ToString();
    }

    void PlayerSetting(ref Tiles tiles)
    {
        playerScript = FindObjectOfType<PlayerManaging>();

        playerScript.MaxX = MaxX;
        playerScript.MaxY = MaxY;
        int x, y;
        while (true)
        {
            x = UnityEngine.Random.Range(0, MaxX);
            y = UnityEngine.Random.Range(0, MaxY);
            if (tiles.Poses[x, y].isGo)
                break;
        }
        player.transform.position = changePosition(x, y, -1);
        playerScript.X = x;
        playerScript.Y = y;
    }

    Vector3 changePosition(int x, int y, int z)
    {
        return right * (x * tileXSize + firstPosX) + up * (y * tileYSize + firstPosY) + forward * z;
    }

    void GenerateMap(ref Tiles Tiles)
    {

        // 맵 초기화
        Tiles = MapInit();

        // 무작위 맵 채우기
        RandomMapFilling(ref Tiles);

        // 맵 스케일링 시작 ( rotateNum 만큼 반복 )
        MapScaling(ref Tiles, rotateNum);

        // 맵 속성 변환
        MakeEnv(ref Tiles);
    }

    Tiles MapInit()
    {
        Tiles tileTmp = new Tiles();
        tileTmp.TileNumber = new int[MaxY, MaxX];
        tileTmp.TileSet = new bool[MaxY, MaxX];
        tileTmp.Poses = new Position[MaxX, MaxY]; // 타일 정보 배열

        // 기본 타일 초기화 - 무에서 시작한다.
        for (int i = 0; i < MaxY; i++)
        {
            for (int j = 0; j < MaxX; j++)
            {
                // 모든 타일을 죽은 상태의 물타일로 변환
                tileTmp.Poses[j, i].TileEnv = 4;
                tileTmp.Poses[j, i].x = j;
                tileTmp.Poses[j, i].y = i;
                tileTmp.Poses[j, i].isGo = false;
            }
        }

        return tileTmp;
    }

    void RandomMapFilling(ref Tiles tiles)
    {
        int EnvRange;
        for (int i = 0; i < MaxY; i++)
        {
            for (int j = 0; j < MaxX; j++)
            {
                if (i == 0 || j == 0||i==MaxY-1||j==MaxX-1)
                {
                    // 맨 끝 타일은 물타일로 생성
                    tiles.Poses[j, i].TileEnv = 4;
                }
                // 위치에 따른 가중치를 부여하여 속성을 설정해준다.
                else
                {
                    EnvRange = UnityEngine.Random.Range(0, 101); // 0부터 100 까지 확인
                   
                    if(EnvRange > randomfillPercent)
                    {
                        tiles.Poses[j, i].TileEnv = 4;
                        tiles.Poses[j, i].Env = 0;
                    }
                    else
                    {
                        tiles.Poses[j, i].TileEnv = -1;
                        tiles.Poses[j, i].Env = -1;
                    }

                }
            }
        }
    }

    void MapScaling(ref Tiles tiles, int rotationNumber)
    {
        // 셀룰러 오토마타를 이용한 맵 다듬기 수행 ( 지정횟수 = rotationNumber만큼 수행 )
        for(int number = 0;number<rotationNumber;number++)
        {
            int Env=-1;
            for(int i=0;i<MaxY;i++)
            {
                for(int j=0;j<MaxX;j++)
                {
                    if (MapFinding(tiles, j, i) > TileSurviveNum)
                    {
                        tiles.Poses[j, i].TileEnv = -1;
                        tiles.Poses[j, i].Env = -1;
                    }
                    else
                    {
                        tiles.Poses[j, i].TileEnv = 4;
                        tiles.Poses[j, i].Env = 0;
                    }
                }
            }
        }
    }
    
    int MapFinding(Tiles tiles,int x, int y)
    {
        int number = 0;
        for(int i=-1;i<=1;i++)
        {
            for(int j=-1;j<=1;j++)
            {
                if(x==0 && i == -1)
                {
                    continue;
                }
                else if(y==0 && j==-1)
                {
                    continue;
                }
                else if(x == MaxX - 1 && i==1)
                {
                    continue;
                }
                else if (y == MaxY - 1 && j == 1)
                {
                    continue;
                }
                else if (tiles.Poses[x+i,y+j].TileEnv != 4)
                {
                    number++;
                }
            }
        }
        return number;
    }

    void MakeEnv(ref Tiles tiles)
    {
        // 호수를 찾는다.
        FindLake(ref tiles);

        // 강을 지정한다.
        MakeRiver(ref tiles);

        ChangeTile(ref tiles);
    }

    void FindLake (ref Tiles tiles)
    {
        // 호수 탐색
        for(int i=0;i<MaxX;i++)
        {
            for(int j=0;j<MaxY;j++)
            {
                // j가 맨 처음값일때 i의 직전 타일만 확인한다.
                if (tiles.Poses[j, i].TileEnv == 4)
                {
                    if (j == 0)
                    {
                        if (i == 0)
                        {
                            tiles.Poses[j, i].Env = 0;
                        }
                        else
                        {
                            if (tiles.Poses[j, i - 1].Env != 0)
                                tiles.Poses[j, i].Env = 1;
                        }
                    }
                    else
                    {
                        if (i == 0)
                        {
                            if (tiles.Poses[j - 1, i].Env != 0)
                                tiles.Poses[j, i].Env = 1;
                        }
                        else
                        {
                            if (tiles.Poses[j - 1, i].Env != 0 && tiles.Poses[j, i - 1].Env != 0)
                                tiles.Poses[j, i].Env = 1;
                        }
                    }
                }
            }
        }

        for (int i = MaxX-1; i >= 0; i--)
        {
            for (int j = MaxY-1; j >=0; j--)
            {
                // j가 맨 처음값일때 i의 직전 타일만 확인한다.
                if (tiles.Poses[j, i].TileEnv == 4)
                {
                    if (j == MaxY - 1)
                    {
                        if (i == MaxX - 1)
                        {
                            tiles.Poses[j, i].Env = 0;
                        }
                        else
                        {
                            if (tiles.Poses[j, i + 1].Env != 0)
                            {
                                if (tiles.Poses[j, i].Env != 0)
                                    tiles.Poses[j, i].Env = 1;
                            }
                            else
                            {
                                tiles.Poses[j, i].Env = 0;
                            }
                        }
                    }
                    else
                    {
                        if (i == MaxX - 1)
                        {
                            if (tiles.Poses[j + 1, i].Env != 0)
                            {
                                if (tiles.Poses[j, i].Env != 0)
                                    tiles.Poses[j, i].Env = 1;
                            }
                            else
                            {
                                tiles.Poses[j, i].Env = 0;
                            }
                        }
                        else
                        {
                            if (tiles.Poses[j + 1, i].Env != 0 && tiles.Poses[j, i + 1].Env != 0)
                            {
                                if (tiles.Poses[j, i].Env != 0)
                                    tiles.Poses[j, i].Env = 1;
                            }
                            else
                            {
                                tiles.Poses[j, i].Env = 0;
                            }
                        }
                    }
                }
            }
        }

        for (int i = 1; i < MaxX-1; i++)
        {
            for (int j = 1; j < MaxY-1; j++)
            {
                // j가 맨 처음값일때 i의 직전 타일만 확인한다.
                if (tiles.Poses[j, i].TileEnv == 4)
                {

                    if (tiles.Poses[j - 1, i].Env != 0 && tiles.Poses[j, i - 1].Env != 0&& tiles.Poses[j + 1, i].Env != 0 && tiles.Poses[j, i + 1].Env != 0)
                    {
                        if (tiles.Poses[j, i].Env != 0)
                            tiles.Poses[j, i].Env = 1;
                    }
                    else
                    {
                        tiles.Poses[j, i].Env = 0;
                    }
                }
            }
        }

        for (int i = MaxX - 2; i > 0; i--)
        {
            for (int j = MaxY - 2; j > 0; j--)
            {
                // j가 맨 처음값일때 i의 직전 타일만 확인한다.
                if (tiles.Poses[j, i].TileEnv == 4)
                {

                    if (tiles.Poses[j - 1, i].Env != 0 && tiles.Poses[j, i - 1].Env != 0 && tiles.Poses[j + 1, i].Env != 0 && tiles.Poses[j, i + 1].Env != 0)
                    {
                        if (tiles.Poses[j, i].Env != 0)
                            tiles.Poses[j, i].Env = 1;
                    }
                    else
                    {
                        tiles.Poses[j, i].Env = 0;
                    }
                }
            }
        }

        for (int j = 1; j < MaxY - 1; j++)
        {
            for (int i = 1; i < MaxX - 1; i++)
            {
                // j가 맨 처음값일때 i의 직전 타일만 확인한다.
                if (tiles.Poses[j, i].TileEnv == 4)
                {

                    if (tiles.Poses[j - 1, i].Env != 0 && tiles.Poses[j, i - 1].Env != 0 && tiles.Poses[j + 1, i].Env != 0 && tiles.Poses[j, i + 1].Env != 0)
                    {
                        if (tiles.Poses[j, i].Env != 0)
                            tiles.Poses[j, i].Env = 1;
                    }
                    else
                    {
                        tiles.Poses[j, i].Env = 0;
                    }
                }
            }
        }

        for (int j = MaxY - 2; j > 0; j--)
        {
            for (int i = MaxX - 2; i > 0; i--)
            {
                // j가 맨 처음값일때 i의 직전 타일만 확인한다.
                if (tiles.Poses[j, i].TileEnv == 4)
                {

                    if (tiles.Poses[j - 1, i].Env != 0 && tiles.Poses[j, i - 1].Env != 0 && tiles.Poses[j + 1, i].Env != 0 && tiles.Poses[j, i + 1].Env != 0)
                    {
                        if (tiles.Poses[j, i].Env != 0)
                            tiles.Poses[j, i].Env = 1;
                    }
                    else
                    {
                        tiles.Poses[j, i].Env = 0;
                    }
                }
            }
        }
    }

    void MakeRiver(ref Tiles tiles)
    {
        // 큰 흐름의 방향을 지정한다.
        // 이동하는 과정을 만든다.
        // 일정 확률로 갈라진다.
        //관통하면 강이 만들어지게 되는것이다.
        int startX;
        int startY;
        int endX;
        int endY;

        int HorizonMovePercent;
        int VerticalMovePercent;

        int separateNum = 0;



        int startDir = UnityEngine.Random.Range(0, 8); // 0,1 : 좌상 , 2,3 : 우상 , 4,5 : 좌하 , 6,7 : 우하


        //------------------------------------------<방향 정하는 과정>-------------------------------------------------------

        //==================================================================================================================
        // 짝수면 X는 0, Y는 1이 되어야 한다. 
        // 위치 설정 공식 위의 Dir 위치를 비교하여 0 ~ 3/1 또는 3/2 ~ 1 지점을 선택하여 결정한다.
        startX = UnityEngine.Random.Range((int)(MaxX * 0.3f * (startDir % 4 > 1 ? 2 : 0) * ((startDir + 1) % 2)), (int)(MaxX * 0.3f * ((startDir % 4 > 1 ? 2 : 0) + 1) * ((startDir + 1) % 2))); // 좌측 : 0 , 우측 : 2
        startY = UnityEngine.Random.Range((int)(MaxX * 0.3f * (startDir % 4 > 1 ? 2 : 0) * (startDir % 2)), (int)(MaxX * 0.3f * ((startDir % 4 > 1 ? 2 : 0) + 1) * (startDir % 2))); // 좌측 : 0 , 우측 : 2

        int endDir = UnityEngine.Random.Range(0, 3); // 75:25 상향, 50:50 대각선 , 25:75 평행

        endX = MaxX - 1 - (startX > MaxX / 2 ? MaxX - 1 : 0);
        endY = MaxY - 1 - (startY > MaxY / 2 ? MaxY - 1 : 0);

        HorizonMovePercent = 75 - (25 * endDir);
        VerticalMovePercent = 25 + (25 * endDir);

        int updown = (startDir / 4 < 1 ? -1 : 1); // up이면 +1, down이면 -1 "상"이 들어가면 down, "하"가 들어가면 up
        int leftright = (startDir % 4 > 1 ? -1 : 1); // left면 -1, right면 +1 "좌"가 들어가면 right, "우"가 들어가면 left -> 0145 & 2367
                                                     //===================================================================================================================

        //-------------------------------------------------------------------------------------------------------------------
        int stopX = (startX < MaxX / 2 ? MaxX : 0);
        int stopY = (startY < MaxY / 2 ? MaxY : 0);

        int Xinc = (startX < MaxX / 2 ? 1 : -1);
        int Yinc = (startY < MaxY / 2 ? 1 : -1);

        while (true)
        {
            int horizon = UnityEngine.Random.Range(0, 101);
            int vertical = UnityEngine.Random.Range(0, 101);
            bool positionChange = false;
            if (horizon >= HorizonMovePercent)
            {
                if (vertical < VerticalMovePercent)
                {
                    positionChange = true;
                    startY += Yinc;
                }
            }
            else if (vertical < VerticalMovePercent)
            {
                if (horizon < vertical)
                {
                    positionChange = true;
                    startY += Yinc;
                }
                else if (horizon > vertical)
                {
                    positionChange = true;
                    startX += Xinc;
                }
                else
                {
                    if (UnityEngine.Random.Range(0, 101) > 50)
                    {
                        positionChange = true;
                        startY += Yinc;
                    }
                    else
                    {
                        positionChange = true;
                        startX += Xinc;
                    }
                }
            }
            else
            {
                positionChange = true;
                startX += Xinc;
            }


            // 끝까지 간 경우 종료한다.
            if (startX == stopX || startY == stopY)
                break;

            // 타일 속성이 물인경우 -> 바다면 pass
            if (tiles.Poses[startX, startY].TileEnv == 4)
            {
                if (tiles.Poses[startX, startY].Env != 0)
                {
                    if (tiles.Poses[startX, startY].Env == 2 && positionChange)
                    {
                        Debug.Log("find River!" + tiles.Poses[startX, startY].TileEnv + " , " + tiles.Poses[startX, startY].Env);
                        break;
                    }
                }
            }
            // 그 외 땅인경우 -> 물줄기를 추가한다.
            else
            {
                // 타일을 물로 바꾸고 강으로 만든다.
                tiles.Poses[startX, startY].TileEnv = 4;
                tiles.Poses[startX, startY].Env = 2;
            }

        }

    }

    void ChangeTile(ref Tiles tiles)
    {
        int[] EnvNum = new int[4];
        Position[] MapQ = new Position[MaxX * MaxY];
        int head = -1;
        int tail = 0;
        int[] DecreaseQueue = new int[MaxX];
        int QueueHead = -1;
        int QueueCnt = 0;
        int EndCnt = 0;

        for (int i=0;i<4;i++)
        {
            EnvNum[i] = UnityEngine.Random.Range(0, 4);
            for (int j = 0; j < i; j++)
            {
                if (EnvNum[i] == EnvNum[j])
                {
                    i--;
                    break;
                }
            }
        }

        int cnt = 0;
        // 큐에 값을 넣는다.
        for (int rot = 0; rot < InputQueNumber; rot++)
        {
            for (int i = 0; i < 4; i++)
            {
                int X = UnityEngine.Random.Range(0, MaxX);
                int Y = UnityEngine.Random.Range(0, MaxY);
                if (tiles.Poses[X, Y].TileEnv != -1) // 해당 타일이 다른 속성 타일이거나 물타일인경우 되돌린다.
                {
                    i--;
                }
                else // 아닌 경우 타일을 집어넣는다.
                {
                    tileSetting(ref tiles, X, Y, true, EnvNum[i], cnt++);
                    MapQ[tail++] = tiles.Poses[X, Y];
                    if(tiles.Poses[X,Y].TileEnv==1)
                    {
                        QueueCnt++;
                    }
                }
            }
        }

        // 초기 큐카운트에 값을 집어넣고 종료
        DecreaseQueue[++QueueHead] = QueueCnt;
        QueueCnt = 0; 
        
        int EnvColor;
        int QtileNum;
        bool isStopSet;
        int Qx, Qy; // 맵 큐에서 꺼낸 값을 담는 변수
        int End = MaxX * MaxY;

        float forestPercent = RandomForestPercent[RandomForestPercent.Length-1];
        Debug.Log(forestPercent);
        while (head != tail)
        {
            isStopSet = true;
            Qx = MapQ[(++head) % End].x; // 범위 초과를 막기위한 방법 -> 나머지를 출력하여 초과하면 0으로 돌아간다.
            Qy = MapQ[head % End].y;
            QtileNum = MapQ[head % End].tilenumber;
            EnvColor = MapQ[head % End].TileEnv;

            if (tiles.Poses[Qx,Qy].TileEnv==1) // 꺼낸 타일 속성이 숲일 경우 EndCnt가 늘어난다.
                EndCnt++;
            if(EndCnt==DecreaseQueue[QueueHead % (MaxX)]) // 이전 단계의 큐 값일 경우 지금까지 쌓은 QueueCnt 값을 큐에 저장한다.
            {
                DecreaseQueue[(++QueueHead)%(MaxX)] = QueueCnt;
                forestPercent -= DecreaseForestPercent; // 숲의 나무생성 확률 감소
                
                // 큐카운트와 엔드카운트 초기화
                QueueCnt = 0;
                EndCnt = 0;
            }

            // 숲 생성 함수
            MakeForest(ref tiles, Qx, Qy, forestPercent);

            if (Qx != 0)
            {
                if (tiles.Poses[Qx - 1, Qy].TileEnv == -1)
                {
                    if (UnityEngine.Random.Range(0, 101) < RandomOutPercent) // 일정 확률로 해당 범위를 이동하지 않고 정체한다.
                    {
                        isStopSet = false; // 정체되어있는경우는 한번만 수행한다.
                        MapQ[(tail++) % End] = tiles.Poses[Qx, Qy];
                    }
                    else
                    {
                        tileSetting(ref tiles, Qx-1, Qy, true, EnvColor, QtileNum);
                        MapQ[(tail++) % End] = tiles.Poses[Qx - 1, Qy];
                        tiles.Poses[Qx - 1, Qy].isGo = true;
                    }
                    // 큐에 집어넣을 때마다 카운트를 늘린다.
                    if (EnvColor == 1)
                        QueueCnt++;
                }
                else if (tiles.Poses[Qx - 1, Qy].tilenumber != QtileNum && tiles.Poses[Qx-1,Qy].TileEnv!=4) // 만나는 타일이 존재하며, 그 타일이 바다가 아닌 경우
                {
                    MakeMountain(ref tiles, Qx-1, Qy);
                    MakeMountain(ref tiles, Qx, Qy);
                }
            }

            if (Qy != 0)
            {
                if (tiles.Poses[Qx, Qy - 1].TileEnv == -1)
                {
                    if (UnityEngine.Random.Range(0, 101) < RandomOutPercent)
                    {
                        if (isStopSet)
                        {
                            MapQ[(tail++) % End] = tiles.Poses[Qx, Qy];
                            
                            if (EnvColor == 1)
                                QueueCnt++;
                        }
                    }
                    else
                    {
                        tileSetting(ref tiles, Qx, Qy - 1, true, EnvColor, QtileNum);
                        MapQ[(tail++) % End] = tiles.Poses[Qx, Qy - 1];
                        
                        if (EnvColor == 1)
                            QueueCnt++;
                    }
                    
                }
                else if (tiles.Poses[Qx , Qy - 1].tilenumber != QtileNum && tiles.Poses[Qx, Qy - 1].TileEnv != 4)
                {
                    MakeMountain(ref tiles, Qx, Qy - 1);
                    MakeMountain(ref tiles, Qx, Qy);
                }
            }

            if (Qx < MaxX - 1)
            {
                if (tiles.Poses[Qx + 1, Qy].TileEnv == -1)
                {
                    if (UnityEngine.Random.Range(0, 101) < RandomOutPercent)
                    {
                        if (isStopSet)
                        {
                            MapQ[(tail++) % End] = tiles.Poses[Qx, Qy];
                            if (EnvColor == 1)
                                QueueCnt++;
                        }
                    }
                    else
                    {
                        tileSetting(ref tiles, Qx+1, Qy, true, EnvColor, QtileNum);
                        MapQ[(tail++) % End] = tiles.Poses[Qx + 1, Qy];

                        if (EnvColor == 1)
                            QueueCnt++;
                    }
                    
                }
                else if (tiles.Poses[Qx + 1, Qy].tilenumber != QtileNum && tiles.Poses[Qx + 1, Qy].TileEnv != 4)
                {
                    MakeMountain(ref tiles, Qx+1, Qy);
                    MakeMountain(ref tiles, Qx, Qy);
                }
            }

            if (Qy < MaxY - 1)
            {
                if (tiles.Poses[Qx, Qy + 1].TileEnv == -1)
                {
                    if (UnityEngine.Random.Range(0, 101) < RandomOutPercent)
                    {
                        if (isStopSet)
                        {
                            MapQ[(tail++) % End] = tiles.Poses[Qx, Qy];
                            if (EnvColor == 1)
                                QueueCnt++;
                        }
                    }
                    else
                    {
                        tileSetting(ref tiles, Qx, Qy + 1, true, EnvColor, QtileNum);
                        MapQ[(tail++) % End] = tiles.Poses[Qx, Qy + 1];

                        if (EnvColor == 1)
                            QueueCnt++;
                    }           
                }
                else if (tiles.Poses[Qx, Qy+1].tilenumber != QtileNum && tiles.Poses[Qx, Qy + 1].TileEnv != 4)
                {
                    MakeMountain(ref tiles, Qx, Qy + 1);
                    MakeMountain(ref tiles, Qx, Qy);
                }
            }
            // 큐가 빌 때까지 반복한다.
        }
    }

    void tileSetting(ref Tiles tiles, int x, int y, bool isGo, int Env, int tileNumber)
    {
        tiles.Poses[x, y].TileEnv = Env;
        tiles.Poses[x, y].isGo = isGo;
        tiles.Poses[x, y].tilenumber = tileNumber;
    }
    // 산 생성 함수
    void MakeMountain(ref Tiles tiles, int x, int y)
    {
        if (UnityEngine.Random.Range(0, 101) < RandomMountainPercent)
        {
            tiles.Poses[x, y].Env = 0;
            tiles.Poses[x, y].isGo = false;
        }
    }

    // 숲 생성 함수
    void MakeForest(ref Tiles tiles,int x,int y, float forestPercent)
    {
        // 숲 생성 알고리즘 실행
        if (tiles.Poses[x, y].TileEnv != 4)// 무작위로 생성시키는 부분
        {
            if (UnityEngine.Random.Range(0, 101) < RandomForestPercent[tiles.Poses[x, y].TileEnv])
            {
                tiles.Poses[x, y].Env = 1 + tiles.Poses[x, y].TileEnv;
            }
        }
        if (tiles.Poses[x, y].TileEnv == 1) // 숲환경에 한해서 뭉쳐서 나올수 있게 해주는 부분
        {
            if (UnityEngine.Random.Range(0, 101) < forestPercent)
            {
                tiles.Poses[x, y].Env = 1 + tiles.Poses[x, y].TileEnv;
            }
        }
    }

    void MakeMap(ref Tiles tileSet, ref GameObject[,] TileObjs, ref GameObject[,] TileEnvs)
    {
        for(int i=0;i<MaxY;i++)
        {
            for(int j=0;j<MaxX;j++)
            {
                try
                {
                    if (tileSet.Poses[j, i].TileEnv != 4)
                    {
                        TileObjs[j, i] = Instantiate(high[tileSet.Poses[j, i].TileEnv * 5]);
                        TileObjs[j, i].transform.position = right * (j * tileXSize + firstPosX) + up * (i * tileYSize + firstPosY);

                        if (tileSet.Poses[j, i].Env != -1)
                        {
                            TileEnvs[j, i] = Instantiate(env[tileSet.Poses[j, i].Env]);
                            TileEnvs[j, i].transform.position = right * (j * tileXSize + firstPosX) + up * (i * tileYSize + firstPosY) + forward * -3;
                        }
                    }
                    else
                    {
                        TileObjs[j, i] = Instantiate(high[(tileSet.Poses[j, i].TileEnv * 5)+tileSet.Poses[j,i].Env]);
                        TileObjs[j, i].transform.position = right * (j * tileXSize + firstPosX) + up * (i * tileYSize + firstPosY);
                    }
                }
                catch // 섬인경우 여기서 정해진다.
                {
                    tileSet.Poses[j, i].TileEnv = 1; // 숲으로 처리

                    TileObjs[j, i] = Instantiate(high[tileSet.Poses[j, i].TileEnv * 5]);
                    TileObjs[j, i].transform.position = right * (j * tileXSize + firstPosX) + up * (i * tileYSize + firstPosY);
                }
            }
        }
    }
}
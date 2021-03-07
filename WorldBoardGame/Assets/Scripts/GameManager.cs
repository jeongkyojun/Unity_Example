using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Tiles
{
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

    public int life; // 세포 자동화의 수명
    public int surviveTime; // 생존 주기 (0 부터 시작하여 점점 늘어난다.)

    public bool isDead;// 칸이 죽은 셀인지 아닌지를 확인하는 변수 ( true면 사망, false면 생존 )

    public int water_info; // 0 이면 물, 1이면 용암
    public int Env; // 환경 설정 0,1,2,3,4 로 구성, 4는 물 
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
    public int survive;
    public int die;
    public int tileSurviveNum;

    [Header("반복횟수")]
    public int rotateNum;

    [Header("랜덤관련")]
    public int seed; // 랜덤 시드넘버

    [Header("타일 전체 범위")]
    public int MaxX;
    public int MaxY;

    [Header("타일 기본 사이즈")]
    public float tileXSize = 2;
    public float tileYSize = 2;

    float firstPosX; // 첫 위치 지정
    float firstPosY; // 첫 위치 지정2

    // 등고선 관련
    public GameObject[] high;

    // 변수
    public Tiles TE = new Tiles();
    public GameObject[,] TilesArr; // 타일 정보 지정 ( 등고선 )
    GameObject[,] TileEnvArr;

    //기본 벡터
    Vector3 right = Vector3.right;
    Vector3 up = Vector3.up;
    Vector3 forward = Vector3.forward;

    // Start is called before the first frame update
    void Start()
    {
        MaxX = border * 2 + GridSize;
        MaxY = border * 2 + GridSize;

        TilesArr = new GameObject[MaxY, MaxX];

        firstPosX = tileXSize / 2;
        firstPosY = tileYSize / 2;

        UnityEngine.Random.seed = seed;

        GenerateMap(ref TE, ref TilesArr); // 맵 생성
        //맵 스케일링
        MakeMap(ref TE,ref TilesArr);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateMap(ref Tiles Tiles, ref GameObject[,] TileObjs)
    {
        /*
        //UnityEngine.Random.seed = seedNumber; // 수행시 같은 숫자만 나오기때문에 따로 가공과정 필요
        */

        // 맵 초기화
        Tiles = MapInit();

        // 무작위 맵 채우기
        RandomMapFilling(ref Tiles);
        
        // 맵 스케일링 시작 ( rotateNum 만큼 반복 )
        MapScaling(ref Tiles, rotateNum);

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
                tileTmp.Poses[j, i].Env = 4;
                tileTmp.Poses[j, i].isDead = true;
            }
        }

        return tileTmp;
    }

    void RandomMapFilling(ref Tiles tiles)
    {
        int EnvRange;
        for (int i = border; i < MaxY - border; i++)
        {
            for (int j = border; j < MaxX - border; j++)
            {
                tiles.Poses[j, i].isDead = false;
                tiles.Poses[j, i].life = survive;
                tiles.Poses[j, i].surviveTime = 0;
                if (i == 0 || j == 0)
                {
                    // 맨 끝 타일은 물타일로 생성
                    tiles.Poses[j, i].Env = 4;
                }
                // 위치에 따른 가중치를 부여하여 속성을 설정해준다.
                else
                {
                    EnvRange = UnityEngine.Random.Range(0, 101); // 0부터 100 까지 확인
                    // 0, 1, 2, 3, 4 각각의 확률은 12.5 , 12.5 , 12.5 , 12.5 ,10 으로 설정, 4번은 수원지이다.
                    // 북쪽으로 갈수록 1은 0에 가까워지고, 남쪽으로 갈수록 0이 0에 가까워진다.
                    // 동쪽으로갈수록 2가 0에 가까워지고, 서쪽으로 갈수록 3이 0에 가까워진다.
                    // 0 = (i * 25/MaxY) , 1 = 25 * (1-(i/MaxY))
                    int ice = i * 25 / MaxY;
                    int fire = 25;
                    int forest = 25+j * 25 / MaxX;
                    int rock = 50;
                    int water = 60;
                    if (EnvRange <= ice)
                        tiles.Poses[j, i].Env = 0;
                    else if (EnvRange <= fire)
                        tiles.Poses[j, i].Env = 2;
                    else if (EnvRange <= forest)
                        tiles.Poses[j, i].Env = 1;
                    else if (EnvRange <= rock)
                        tiles.Poses[j, i].Env = 3;
                    else if(EnvRange<=water)
                        tiles.Poses[j, i].Env = 4;
                    else
                        tiles.Poses[j, i].isDead = true;
                }

            }
        }
    }

    void MapScaling(ref Tiles tiles, int rotationNumber)
    {
        bool isAllFill = false;

        for(int number = 0;number<rotationNumber;number++)
        {
            int Env=-1;
            isAllFill = true;
            for(int i=0;i<MaxY;i++)
            {
                for(int j=0;j<MaxX;j++)
                {
                    if(tiles.Poses[j,i].isDead)
                    {
                        isAllFill = false;
                        if(MapFinding(tiles, ref Env, j, i) > tileSurviveNum)
                        {
                            tiles.Poses[j, i].isDead = false;
                            tiles.Poses[j, i].life = survive;
                            tiles.Poses[j, i].surviveTime = 0;
                            tiles.Poses[j, i].Env = Env;
                        }
                        else
                        {
                            tiles.Poses[j, i].surviveTime++;
                            if (tiles.Poses[j, i].surviveTime == die)
                            {
                                tiles.Poses[j, i].isDead = true;
                            }
                        }
                    }
                }
            }
        }
    }
    
    int MapFinding(Tiles tiles,ref int Env,int x, int y)
    {
        int number = 0;
        int[] Envs = new int[5];
        int maximum = -1;
        for(int i=0;i<4;i++)
        {
            Envs[i] = 0;
        }
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
                else if (!tiles.Poses[x+i,y+j].isDead /*&& tiles.Poses[x + i, y + j].surviveTime == survive*/) // 살아있는 셀인 경우 인식
                {
                    number++;
                    Envs[tiles.Poses[x + i, y + j].Env]++;
                    if (Envs[tiles.Poses[x + i, y + j].Env] > maximum)
                    {
                        maximum = Envs[tiles.Poses[x + i, y + j].Env];
                        Env = tiles.Poses[x + i, y + j].Env;
                    }
                }
            }
        }
        return number;
    }

    void MakeMap(ref Tiles tileSet, ref GameObject[,] TileObjs)
    {
        for(int i=0;i<MaxY;i++)
        {
            for(int j=0;j<MaxX;j++)
            {
                Debug.Log(tileSet.Poses[j, i].Env);
                Debug.Log(tileSet.Poses[j, i].surviveTime);
                TileObjs[j, i] = Instantiate(high[tileSet.Poses[j, i].Env * 5 + (tileSet.Poses[j, i].surviveTime%5)]);
                TileObjs[j, i].transform.position = right * (j * tileXSize + firstPosX) + up * (i * tileYSize + firstPosY);
            }
        }
    }
}
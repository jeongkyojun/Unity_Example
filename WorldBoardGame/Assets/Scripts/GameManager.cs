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
    public int boundary;  // 그리드의 경계너비 - 너비가 클수록 산이 덜 뭉친다.
    public int border;   // 그리드 테두리의 너비
    public int GridNum;  // 1행/1열당 그리드의 개수 그리드 개수가 많을수록 산이 많아진다.

    // 세포 자동화 방식의 생존 및 사망 숫자
    [Header("셀룰러 오토마타 정보")]
    public int survive;
    public int die;

    [Header("반복횟수")]
    public int rotateNum;

    [Header("랜덤관련")]
    public int seed;

    float setPercent = 40;
    //static float stopPercent = 10;
    float changePercent = 1f;
    public int seedNumber; // 랜덤 시드넘버

    public int MaxX;
    public int MaxY;

    // 타일관련 정보
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
        MaxX = border * 2 + GridSize * GridNum + boundary * (GridNum - 1);
        MaxY = border * 2 + GridSize * GridNum + boundary * (GridNum - 1);
        
        firstPosX = tileXSize / 2;
        firstPosY = tileYSize / 2;

        Random.seed = seed;

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
        // 타일 번호 지정
        

        bool[,] isSet = new bool[GridNum, GridNum]; // 그리드 사용여부 확인

        int Xpos, Ypos;

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
        for (int i = 0; i < MaxY; i++)
        {
            for (int j = 0; j < MaxX; j++)
            {
                if (i == 0 || j == 0)
                {
                    // 맨 끝 타일은 물타일로 생성
                    tiles.Poses[j, i].Env = 4;
                    tiles.Poses[j, i].isDead = false;
                }
                // 위치에 따른 가중치를 부여하여 속성을 설정해준다.
                else
                {
                    EnvRange = UnityEngine.Random.Range(0, 101); // 0부터 100 까지 확인
                    
                }

            }
        }
    }

    void MapScaling(ref Tiles tiles, int rotationNumber)
    {

    }
    
    void MakeMap(ref Tiles tileSet, ref GameObject[,] TileObjs)
    {
    }
}
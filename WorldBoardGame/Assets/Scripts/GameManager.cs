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
    public int high; // 높낮이
    public int continent_number;
    public int water_info; // 0 이면 물, 1이면 용암
    public int Env; // 환경 설정
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
    public int BigGrid; // 기준이 되는 그리드의 크기
    public int boundary; // 그리드의 경계너비 - 너비가 클수록 산이 덜 뭉친다.
    public int border =100;   // 그리드 테두리의 너비
    public int GridNum; // 1행/1열당 그리드의 개수 그리드 개수가 많을수록 산이 많아진다.

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
        MaxX = border * 2 + BigGrid * GridNum + boundary * (GridNum - 1);
        MaxY = border * 2 + BigGrid * GridNum + boundary * (GridNum - 1);
        
        firstPosX = tileXSize / 2;
        firstPosY = tileYSize / 2;

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
        Tiles.TileNumber = new int[MaxY, MaxX];
        Tiles.TileSet = new bool[MaxY, MaxX];

        Tiles.Poses = new Position[MaxX, MaxY]; // 타일 정보 배열
        bool[,] isSet = new bool[GridNum, GridNum]; // 그리드 사용여부 확인

        int Xpos, Ypos;
        List<point> pointQ = new List<point>();

        /*
        //UnityEngine.Random.seed = seedNumber; // 수행시 같은 숫자만 나오기때문에 따로 가공과정 필요
        */

        // 기본 타일 초기화 - 무에서 시작한다.
        for (int i = 0; i < MaxY; i++)
        {
            for (int j = 0; j < MaxX; j++)
            {
                Tiles.Poses[i, j].high = UnityEngine.Random.Range(0,2);
                Tiles.TileNumber[i, j] = -1;
                Tiles.TileSet[i, j] = false;
            }
        }

        // 그리드 초기화
        for (int i = 0; i < GridNum; i++)
        {
            for (int j = 0; j < GridNum; j++)
            {
                isSet[i, j] = false;
            }
        }

        for(int i=0;i<GridNum * (GridNum-1);i++)
        {
            int x = UnityEngine.Random.Range(0, GridNum);
            int y = UnityEngine.Random.Range(0, GridNum);

            if (isSet[y, x])
                i--;
            else
            {
                int k = UnityEngine.Random.Range(border + x * (BigGrid + boundary), border + x * (BigGrid + boundary) + boundary);
                int l = UnityEngine.Random.Range(border + y * (BigGrid + boundary), border + y * (BigGrid + boundary) + boundary);
                isSet[y, x] = true;
                point p = new point();
                p.x = k;
                p.y = l;
                p.tilenumber = i;
                p.Env = UnityEngine.Random.Range(1, 4);
                pointQ.Add(p);
            }
        }

        GenerateTile(ref Tiles, ref TileObjs, ref pointQ, setPercent, changePercent);
    }

    void GenerateTile(ref Tiles Tiles,ref GameObject[,] TileObjs, ref List<point> pointQ,float setPercent, float changePercent)
    {
        GameObject[,] TileTmps = new GameObject[MaxX, MaxY]; // 타일 게임오브젝트 배열
        

        while(pointQ.Count>0)
        {
            Tiles.Poses[pointQ[0].y, pointQ[0].x].high += 5;
            Tiles.Poses[pointQ[0].y, pointQ[0].x].Env = pointQ[0].Env * 5;
            Tiles.TileSet[pointQ[0].y, pointQ[0].x] = true;
  
            if (pointQ[0].x > 0)
            {
                if (UnityEngine.Random.Range(0, 101) < setPercent)
                {
                    point p = new point();
                    p.x = pointQ[0].x - 1;
                    p.y = pointQ[0].y;
                    p.Env = pointQ[0].Env;
                    if(!Tiles.TileSet[p.y, p.x])
                        pointQ.Add(p);
                }
            }
            if (pointQ[0].y > 0)
            {
                if (UnityEngine.Random.Range(0, 101) < setPercent)
                {
                    point p = new point();
                    p.x = pointQ[0].x;
                    p.y = pointQ[0].y - 1;
                    p.Env = pointQ[0].Env;
                    if (!Tiles.TileSet[p.y, p.x])
                        pointQ.Add(p);
                }
            }
            if (pointQ[0].x < MaxX-1)
            {
                if (UnityEngine.Random.Range(0, 101) < setPercent)
                {
                    point p = new point();
                    p.x = pointQ[0].x + 1;
                    p.y = pointQ[0].y;
                    p.Env = pointQ[0].Env;
                    if (!Tiles.TileSet[p.y, p.x])
                        pointQ.Add(p);
                }
            }
            if (pointQ[0].y < MaxY-1)
            {
                if (UnityEngine.Random.Range(0, 101) < setPercent)
                {
                    point p = new point();
                    p.x = pointQ[0].x;
                    p.y = pointQ[0].y + 1;
                    p.Env = pointQ[0].Env;
                    if (!Tiles.TileSet[p.y, p.x])
                        pointQ.Add(p);
                }
            }

            pointQ.RemoveAt(0);
            Debug.Log(setPercent);
        }
        TileObjs = TileTmps;
    }
    
    void MakeMap(ref Tiles tileSet, ref GameObject[,] TileObjs)
    {
        for (int i = 0; i < MaxY; i++)
        {
            for (int j = 0; j < MaxX; j++)
            {

                TileObjs[i, j] = Instantiate(high[tileSet.Poses[i,j].Env]);

                TileObjs[i, j].transform.position = right * (firstPosY + i * tileYSize) + up * (firstPosX + j * tileXSize)+ forward * -3;
            }
        }
    }
}
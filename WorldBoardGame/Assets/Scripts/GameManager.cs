using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Tiles
{
    public Position[,] Poses;
    public int[,] TileNumber;
};

public struct Position
{
    public int x,y;
    public int high;
    public int setnum;
    public int continent_number;
};

public class GameManager : MonoBehaviour
{
    // 맵 구성 관련 정보
    public int BigGrid = 30; // 기준이 되는 그리드의 크기
    public int boundary = 10; // 그리드의 경계너비 - 너비가 클수록 산이 덜 뭉친다.
    public int border = 100;   // 그리드 테두리의 너비
    public int GridNum = 3; // 1행/1열당 그리드의 개수 그리드 개수가 많을수록 산이 많아진다.

    static float setPercent = 90;
    static float stopPercent = 10;

    public int MaxX;
    public int MaxY;

    // 타일관련 정보
    public float tileXSize = 2;
    public float tileYSize = 2;

    // 등고선 관련
    public GameObject[] high;

    // 변수
    public Tiles TE = new Tiles();
    public GameObject[,] TilesArr; // 타일 정보 지정 ( 등고선 )
    public GameObject[,,] TilesArrEtc;
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

        GenerateMap(ref TE, ref TilesArr, ref TilesArrEtc);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateMap(ref Tiles Tiles, ref GameObject[,] TileObjs, ref GameObject[,,] TileEtcs)
    {
        // 타일 번호 지정
        Tiles.TileNumber = new int[MaxY, MaxX];

        Tiles.Poses = new Position[MaxX, MaxY]; // 타일 정보 배열
        bool[,] isSet = new bool[GridNum, GridNum]; // 그리드 사용여부 확인

        int Xpos, Ypos;

        // 기본 타일 초기화 - 무에서 시작한다.
        for (int i = 0; i < MaxY; i++)
        {
            for (int j = 0; j < MaxX; j++)
            {
                Tiles.Poses[i, j].high = 0;
                Tiles.TileNumber[i, j] = -1;
            }
        }

        for (int i = 0; i < GridNum; i++)
        {
            for (int j = 0; j < GridNum; j++)
            {
                isSet[i, j] = false;
            }
        }
        GenerateTile(ref Tiles, ref TileObjs);
    }

    void GenerateTile(ref Tiles Tiles,ref GameObject[,] TileObjs)
    {
        GameObject[,] TileTmps = new GameObject[MaxX, MaxY]; // 타일 게임오브젝트 배열
        GameObject[,,] TileEtcTmps = new GameObject[10, MaxX, MaxY]; // 타일 게임오브젝트 배열

        float firstPosX = tileXSize / 2; // 첫 위치 지정
        float firstPosY = tileYSize / 2; // 첫 위치 지정2

        for (int i = 0; i < MaxY; i++)
        {
            for (int j = 0; j < MaxX; j++)
            {
                TileTmps[j, i] = Instantiate(high[Tiles.Poses[j, i].high]);
                TileTmps[j, i].transform.position = right * (j * tileXSize + firstPosX) + up * (i * tileYSize + firstPosY); // transform.position (위치) 설정
                Tiles.Poses[j, i].setnum = Tiles.Poses[j, i].high;
            }
        }
        TileObjs = TileTmps;
    }
}

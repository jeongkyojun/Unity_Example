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
};

public class GameManager : MonoBehaviour
{
    // 맵 구성 관련 정보
    static int BigGrid = 6; // 기준이 되는 그리드의 크기
    static int boundary = 6; // 그리드의 경계너비 - 너비가 클수록 산이 덜 뭉친다.
    static int border = 100;   // 그리드 테두리의 너비
    static int GridNum = 20; // 1행/1열당 그리드의 개수 그리드 개수가 많을수록 산이 많아진다.

    static float setPercent = 90;
    static float stopPercent = 10;

    static int MaxX = border * 2 + BigGrid * GridNum + boundary * (GridNum - 1);
    static int MaxY = border * 2 + BigGrid * GridNum + boundary * (GridNum - 1);

    // 타일관련 정보
    float tileXSize = 2;
    float tileYSize = 2;

    // 등고선 관련
    public GameObject[] high;
    static int highCnt = 10;

    // 변수
    Tiles TE = new Tiles();
    GameObject[,] TilesArr; // 타일 정보 지정 ( 등고선 )
    GameObject[,] TileEnvArr;

    //기본 벡터
    Vector3 right = Vector3.right;
    Vector3 up = Vector3.up;
    Vector3 forward = Vector3.forward;

    // Start is called before the first frame update
    void Start()
    {
        GenerateMap(ref TE, ref TilesArr, ref TileEnvArr);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateMap(ref Tiles Tiles, ref GameObject[,] TileObjs, ref GameObject[,] TileEnvs)
    {
        float changePercent = 90 / (GridNum * GridNum + 1);
        // 타일 번호 지정
        Tiles.TileNumber = new int[MaxY, MaxX];

        Tiles.Poses = new Position[MaxX, MaxY]; // 타일 정보 배열
        GameObject[,] TileTmps = new GameObject[MaxX, MaxY]; // 타일 게임오브젝트 배열

        bool[,] isSet = new bool[GridNum, GridNum]; // 그리드 사용여부 확인
        int MapSettingNumber = 0; // 그리드 사용 칸 값 확인

        float firstPosX = tileXSize / 2; // 첫 위치 지정
        float firstPosY = tileYSize / 2; // 첫 위치 지정2

        int Xpos, Ypos;

        // 기본 타일 초기화 - 무에서 시작한다.
        for (int i = 0; i < MaxY; i++)
        {
            for (int j = 0; j < MaxX; j++)
            {
                Tiles.Poses[i, j].high = 0;
            }
        }
        for (int i = 0; i < GridNum; i++)
        {
            for (int j = 0; j < GridNum; j++)
            {
                isSet[i, j] = false;
            }
        }
        while (true)
        {
            int i = UnityEngine.Random.Range(0, GridNum);
            int j = UnityEngine.Random.Range(0, GridNum);
            if (!isSet[i, j])
            {
                isSet[i, j] = true;
                MapSettingNumber++;

                Ypos = UnityEngine.Random.Range(border + BigGrid * i + boundary * i, border + BigGrid * (i + 1) + boundary * i);
                Xpos = UnityEngine.Random.Range(border + BigGrid * j + boundary * j, border + BigGrid * (j + 1) + boundary * j);

                // 타일에 흙을 얹는다.
                if(Tiles.Poses[Ypos,Xpos].high<9)
                    GenerateTile(ref Tiles, (int)(setPercent - (changePercent*MapSettingNumber)), (int)(stopPercent + (changePercent*MapSettingNumber)), Xpos, Ypos, MapSettingNumber);
            }
            if (MapSettingNumber == GridNum * GridNum)
                break;
        }
        for (int i = 0; i < MaxY; i++)
        {
            for (int j = 0; j < MaxX; j++)
            {
                TileTmps[j, i] = Instantiate(high[Tiles.Poses[j, i].high]);
                TileTmps[j,i].transform.position = right * (j * tileXSize + firstPosX) + up * (i * tileYSize + firstPosY); // transform.position (위치) 설정
            }
        }
        TileObjs = TileTmps;
    }

    void GenerateTile(ref Tiles tiles, int setPercent, int stopPercent, int Xpos, int Ypos, int continent_number)
    {
        // 타일 번호, 시작 지점, 퍼질 확률, 타일
        tiles.Poses[Ypos, Xpos].high++;
        tiles.TileNumber[Ypos, Xpos] = continent_number;
        // 다른 대륙간 경계면일시 일정 확률로 산맥 생성
        if (Xpos > 0)
        {
            if ((UnityEngine.Random.Range(0, 101) < setPercent) && tiles.Poses[Ypos,Xpos-1].high < 9 && tiles.TileNumber[Ypos,Xpos-1] != continent_number)
            {
                GenerateTile(ref tiles, setPercent,stopPercent,  Xpos - 1, Ypos,  continent_number);
            }
        }
        if (Xpos < MaxX - 1)
        {
            if ((UnityEngine.Random.Range(0, 101) < (int)setPercent) && tiles.Poses[Ypos, Xpos + 1].high < 9 && tiles.TileNumber[Ypos, Xpos+1] != continent_number)
            {
                GenerateTile(ref tiles, setPercent, stopPercent,  Xpos + 1, Ypos, 
                    continent_number);
            }
        }
        if (Ypos > 0)
        {
            if ((UnityEngine.Random.Range(0, 101) < (int)setPercent) && tiles.Poses[Ypos - 1, Xpos].high < 9 && tiles.TileNumber[Ypos - 1, Xpos] != continent_number)
            {
                GenerateTile(ref tiles, setPercent,
                    stopPercent,
                    Xpos, Ypos - 1,
                    continent_number);
            }
        }
        if (Ypos < MaxY - 1)
        {
            if ((UnityEngine.Random.Range(0, 101) < (int)setPercent) && tiles.Poses[Ypos+1, Xpos].high < 9 && tiles.TileNumber[Ypos+1, Xpos] != continent_number)
            {
                GenerateTile(ref tiles, setPercent,
                    stopPercent,
                    Xpos, Ypos + 1,
                    continent_number);
            }
        }
        return;
    }
}

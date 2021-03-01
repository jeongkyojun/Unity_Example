using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

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
    public int Environment; // 세부환경 0:평지, 1:산, 2:숲
    public int TileStatus; // 타일 상태 여부 - 몬스터나 어떠한 이벤트가 있는지 확인

    public Color defaultColor;

    public int continent_number;
};

public class GameManager : MonoBehaviour
{
    static int BigGrid = 9; // 기준이 되는 그리드의 크기
    static int boundary = 10; // 그리드의 경계너비
    static int border = 100;   // 그리드 테두리의 너비
    static int GridNum = 10; // 1행/1열당 그리드의 개수

    static float forestPercent = 60f;
    static float setPercent = 90f;
    static float stopPercent = 100 - setPercent;

    static int MaxX = border*2 + BigGrid * GridNum + boundary * (GridNum-1);
    static int MaxY = border*2 + BigGrid * GridNum + boundary * (GridNum-1);
    float TileXSize = 2;
    float TileYSize = 2;

    public GameObject normalMapTile;
    public GameObject FireMapTile;
    public GameObject IceMapTile;
    public GameObject WindMapTile;
    public GameObject EarthMapTile;
    public GameObject VoidMapTile;

    public GameObject FireForest;
    public GameObject IceForest;
    public GameObject EarthForest;

    public GameObject Mountain;

    int[] TileSet = new int[5];

    public GameObject player;
    public GameObject monster;

    PlayerManaging playerInfo;

    Vector3 up = Vector3.up;
    Vector3 right = Vector3.right;
    Vector3 foward = Vector3.forward;

    public TileEntity TE;
    GameObject[,] TilesArr;
    GameObject[,] TileEnvArr;

    bool isMoved, YMoved=false, XMoved=false;
    public int turn;

    Plane GroupPlane = new Plane(Vector3.zero, Vector3.forward);

    public int moveCnt;
    public Button TurnEndBtn;
    public Text MoveCntText;
    // Start is called before the first frame update
    void Start()
    {
        moveCnt = 5;
        playerInfo = FindObjectOfType<PlayerManaging>();
        isMoved = false;
        turn = 1;
        // 각 속성 타일의 수를 정한다.
        int minRange = (GridNum*GridNum)/4;
        int maxRange = (GridNum*GridNum)/4+2;
        for(int i=0;i<4;i++)
        {
            //TileSet[i] = UnityEngine.Random.Range(minRange,maxRange);
            TileSet[i] = (GridNum * GridNum) / 4+1;
        }
        //TileSet[4] = GridNum*GridNum*2/9;
        TileSet[4] = 0;
        GenerateMap(ref TE, ref TilesArr, ref TileEnvArr);
        playerInfo.MaxX = MaxX;
        playerInfo.MaxY = MaxY;
        playerInfo.TileXSize = TileXSize;
        playerInfo.TileYSize = TileYSize;
        while (true)
        {
            playerInfo.X = UnityEngine.Random.Range(0, MaxX);
            playerInfo.Y = UnityEngine.Random.Range(0, MaxY);
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
            Debug.Log("PlayerTurn End, EnemyTurn Start");
            /*
             * 적 턴 시작
             */
            Debug.Log("EnemyTurn End, PlayerTurn Start");
            turn = 1;
            moveCnt = 5;
            MoveCntText.text = "MoveCnt : " + moveCnt.ToString();
            TurnEndBtn.interactable = true;
        }

        // 이동 카운트를 다쓰면 자동 턴 종료
        if (moveCnt == 0)
            turn=2;
    }

    void GenerateMap(ref TileEntity Tiles, ref GameObject[,] TileObjs, ref GameObject[,] TileEnvs)
    {
        Tiles.Poses = new Pos[MaxX,MaxY];
        GameObject[,] TileTmps = new GameObject[MaxX, MaxY];
        GameObject[,] TileEnvTmps = new GameObject[MaxX, MaxY];
        Tiles.TileSetting = new bool[MaxX, MaxY];
        bool[,] isSet = new bool[GridNum, GridNum];
        int MapSettingNumber = 0;

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
            if (!isSet[i,j])
            {
                isSet[i, j] = true;
                MapSettingNumber++;
                while (true)
                {
                    //가중치를 이용해 속성값 출력
                    // i 가 0에 가까울수록 얼음(1)타일의 확률이 늘어나며, i가 최대값에 가까울수록 불(0)타일의 확률이 늘어난다.
                    // j 가 0에 가까울수록 고원(2)타일의 확률이 늘어나며, j가 최대값에 가까울수록 숲(3)타일의 확률이 늘어난다.
                    Select = UnityEngine.Random.Range(0, 101);

                    if (Select <= 50 / (GridNum - 1) * i) // 0 이면 Gridnum + -Gridnum / Gridnum * 25
                        SettingEnv = 0;
                    else if (Select <= 50)
                        SettingEnv = 1;
                    else if (Select <= 50 / (GridNum - 1) * j + 50)
                        SettingEnv = 2;
                    else
                        SettingEnv = 3;

                    if (TileSet[SettingEnv] > 0)
                        break;
                }

                Ypos = UnityEngine.Random.Range(border + BigGrid * i + boundary * i, border + BigGrid * (i + 1) + boundary * i);
                Xpos = UnityEngine.Random.Range(border + BigGrid * j + boundary * j, border + BigGrid * (j + 1) + boundary * j);
                if (!Tiles.TileSetting[Ypos, Xpos])
                {
                    Debug.Log("[ " + j + " , " + i + " ] Env : " + SettingEnv);
                    GenerateTile(ref Tiles, setPercent, stopPercent, Xpos, Ypos, SettingEnv, i + j, forestPercent);
                    TileSet[SettingEnv]--;
                }
            }
            if (MapSettingNumber==GridNum*GridNum)
                break;
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
                            switch (Tiles.Poses[j, i].Environment)
                            {
                                case 0:
                                    break;
                                case 1:
                                    TileEnvTmps[j, i] = Instantiate(Mountain);
                                    Tiles.Poses[j, i].isTrue = false;
                                    break;
                                case 2:
                                    TileEnvTmps[j, i] = Instantiate(FireForest);
                                    break;
                            }
                            break;
                        case 1:
                            TileTmps[j, i] = Instantiate(IceMapTile);
                            Tiles.Poses[j, i].isTrue = true;
                            switch (Tiles.Poses[j, i].Environment)
                            {
                                case 0:
                                    break;
                                case 1:
                                    TileEnvTmps[j, i] = Instantiate(Mountain);
                                    Tiles.Poses[j, i].isTrue = false;
                                    break;
                                case 2:
                                    TileEnvTmps[j, i] = Instantiate(IceForest);
                                    break;
                            }
                            break;
                        case 2:
                            TileTmps[j, i] = Instantiate(WindMapTile);
                            Tiles.Poses[j, i].isTrue = true;
                            switch (Tiles.Poses[j, i].Environment)
                            {
                                case 0:
                                    break;
                                case 1:
                                    TileEnvTmps[j, i] = Instantiate(Mountain);
                                    Tiles.Poses[j, i].isTrue = false;
                                    break;
                                case 2:
                                    break;
                            }
                            break;
                        case 3:
                            TileTmps[j, i] = Instantiate(EarthMapTile);
                            Tiles.Poses[j, i].isTrue = true;
                            switch (Tiles.Poses[j, i].Environment)
                            {
                                case 0:
                                    break;
                                case 1:
                                    TileEnvTmps[j, i] = Instantiate(Mountain);
                                    Tiles.Poses[j, i].isTrue = false;
                                    break;
                                case 2:
                                    TileEnvTmps[j, i] = Instantiate(EarthForest);
                                    break;
                            }
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
                try
                {
                    TileEnvTmps[j, i].transform.position = Tiles.Poses[j, i].TilePosition + foward * -4;
                }
                catch(Exception e)
                {
                    continue;
                }
            }
        }
        TileObjs = TileTmps;
        TileEnvs = TileEnvTmps;
    }

    void GenerateTile(ref TileEntity tiles, float setPercent, float stopPercent, int Xpos, int Ypos, int SettingEnv, int continent_number, float forestPercent)
    {
        // 타일 번호, 시작 지점, 퍼질 확률, 타일
        float changePercent = 0.3f;
        float mountainPercent = 50;
        float fire = -30, ice = -30, Earth = 0, Wind = -forestPercent;
        tiles.TileSetting[Ypos, Xpos] = true;
        tiles.Poses[Ypos, Xpos].TileEnv = SettingEnv;
        tiles.Poses[Ypos, Xpos].continent_number = continent_number;

        // 각 환경마다 일정확률로 숲 생성
        switch (SettingEnv)
        {
            case 0:
                if (UnityEngine.Random.Range(0, 101) <= forestPercent + fire)
                    tiles.Poses[Ypos, Xpos].Environment = 2;
                else
                    tiles.Poses[Ypos, Xpos].Environment = 0;
                break;
            case 1:
                if (UnityEngine.Random.Range(0, 101) <= forestPercent + ice)
                    tiles.Poses[Ypos, Xpos].Environment = 2;
                else
                    tiles.Poses[Ypos, Xpos].Environment = 0;
                break;
            case 2:
                if (UnityEngine.Random.Range(0, 101) <= forestPercent + Wind)
                    tiles.Poses[Ypos, Xpos].Environment = 2;
                else
                    tiles.Poses[Ypos, Xpos].Environment = 0;
                break;
            case 3:
                if (UnityEngine.Random.Range(0, 101) <= forestPercent + Earth)
                    tiles.Poses[Ypos, Xpos].Environment = 2;
                else
                    tiles.Poses[Ypos, Xpos].Environment = 0;
                break;
            default:
                break;
        }

        // 경계면일시 5%확률로 산맥 생성
        if (Xpos > 0 && UnityEngine.Random.Range(0, 101) < setPercent)
        {
            if(!tiles.TileSetting[Ypos, Xpos - 1])
                GenerateTile(ref tiles, setPercent - changePercent, stopPercent + changePercent, Xpos - 1, Ypos, SettingEnv, continent_number, forestPercent-changePercent);
            else if(tiles.Poses[Ypos,Xpos-1].continent_number != tiles.Poses[Ypos,Xpos].continent_number)
            {
                if (UnityEngine.Random.Range(0, 101) <= mountainPercent)
                {
                    tiles.Poses[Ypos, Xpos].isTrue = false;
                    tiles.Poses[Ypos, Xpos].Environment = 1;
                }
            }
        }
        if (Xpos < MaxX - 1 && UnityEngine.Random.Range(0, 101) < setPercent)
        {
            if(!tiles.TileSetting[Ypos, Xpos + 1])
                GenerateTile(ref tiles, setPercent - changePercent, stopPercent + changePercent, Xpos + 1, Ypos, SettingEnv, continent_number, forestPercent-changePercent);
            else if(tiles.Poses[Ypos, Xpos + 1].continent_number != tiles.Poses[Ypos, Xpos].continent_number)
            {
                if (UnityEngine.Random.Range(0, 101) <= mountainPercent)
                {
                    tiles.Poses[Ypos, Xpos].isTrue = false;
                    tiles.Poses[Ypos, Xpos].Environment = 1;
                }
            }
        }
        if (Ypos > 0 && UnityEngine.Random.Range(0, 101) < setPercent)
        {
            if (!tiles.TileSetting[Ypos - 1, Xpos])
                GenerateTile(ref tiles, setPercent - changePercent, stopPercent + changePercent, Xpos, Ypos - 1, SettingEnv, continent_number, forestPercent- changePercent);
            else if (tiles.Poses[Ypos-1, Xpos].continent_number != tiles.Poses[Ypos, Xpos].continent_number)
            {
                if (UnityEngine.Random.Range(0, 101) <= mountainPercent)
                {
                    tiles.Poses[Ypos, Xpos].isTrue = false;
                    tiles.Poses[Ypos, Xpos].Environment = 1;
                }
            }
        }
        if (Ypos < MaxY - 1 && UnityEngine.Random.Range(0, 101) < setPercent)
        {
            if (!tiles.TileSetting[Ypos + 1, Xpos])
                GenerateTile(ref tiles, setPercent - changePercent, stopPercent + changePercent, Xpos, Ypos + 1, SettingEnv, continent_number, forestPercent - changePercent);

            else if (tiles.Poses[Ypos+1, Xpos].continent_number != tiles.Poses[Ypos, Xpos].continent_number)
            {
                if (UnityEngine.Random.Range(0, 101) <= mountainPercent)
                {
                    tiles.Poses[Ypos, Xpos].isTrue = false;
                    tiles.Poses[Ypos, Xpos].Environment = 1;
                }
            }
        }
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

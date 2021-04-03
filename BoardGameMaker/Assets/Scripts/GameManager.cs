using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public struct Map// 전체 맵
{
    public int Seed; // 맵 시드
    public Position[,] Tiles; // 타일 정보
    public Room[,] Rooms; // 방 정보
}

public struct Room
{
    public Vector2 RomSize; // 방의 좌표
    public Vector2 RoomStartNumber; // 방의 시작 지점(tile 기준)
    public bool isRoom;// 방인지 통로인지 확인
    public int RoomNumber; // 방 번호 확인, 합쳐져 있는 방을 찾는데 사용
    public bool isFind; // 방문여부 확인

    public int RoomType; // 방 타입 종류 설정
}

public struct Position
{
    public Vector2 number; // 맵 세팅 번호
    public Vector3 TileVec; // 타일의 트랜스폼 값

    public int temperation; // 온도 설정 (가상값)

    public bool isGo; // 통과 가능 여부 확인 (true면 통과 가능, false면 불가능)
    public bool isGround; // 땅인지 물인지 확인
    public Environment Env; // 환경(열거형) - 평야, 숲, 강 등을 확인한다.
}

public struct Box
{
    Vector2 startPoint;
    Vector2 Size;
    Vector2 endPoint;
}

public class GameManager : MonoBehaviour
{
    [Header("게임 오브젝트 설정")]
    public GameObject Walls; // 장애물
    public GameObject Tiles; // 단순 지형
    public Vector2 TileSize; // 타일 크기 설정

    public GameObject player; // 플레이어
    public GameObject monster; // 몬스터

    public Vector3 firstPos; // 처음 위치

    [Header("방 크기 설정 - RoomNumber와 RoomSize에 따라 자동 변경")]
    public Vector2 Size;

    [Header("백트래킹 설정")]
    public Vector2 RoomNumber;
    public Vector2 RoomSize;

    [Header("미로 설정")]
    public int WallWidth;
    public int BoxNumber;
    public int roadWidth;

    public Map GameMap; // 맵 저장 정보
    public GameObject[,] MapObjects;
    public Room[] Rooms;

    Vector3 up = Vector3.up;
    Vector3 right = Vector3.right;
    Vector3 foward = Vector3.forward;
    Vector3 one = Vector3.one;

    Vector2 up2 = Vector2.up;
    Vector2 right2 = Vector2.right;
    Vector2 one2 = Vector2.one;

    // Start is called before the first frame update
    void Start()
    {
        // 사이즈 지정
        Size = RoomSize * RoomNumber + (RoomNumber + one2) * WallWidth;

        InitMaps(ref GameMap); // 구조체 선언 및 초기화 작업 실행 -> 격자형 구조 생성

        GettingRoom(ref GameMap, BoxNumber);
        
        //MapMaking_Stack(ref GameMap); // 메이즈 생성 알고리즘 3 - 스택

        MakingTile(ref GameMap,ref MapObjects);
    }

    // Update is called once per frame
    void Update()
    {

    }
    // 구조체 선언 + 맵 초기화
    void InitMaps(ref Map GameMap)
    {
        // 구조체 크기 할당함수
        GameMap = new Map();
        //GameMap.Seed = 00000042;
        GameMap.Seed = UnityEngine.Random.Range(0, 100000000);
        UnityEngine.Random.InitState(GameMap.Seed);

        
        GameMap.Tiles = new Position[(int)Size.x, (int)Size.y]; // 타일 크기 설정
        GameMap.Rooms = new Room[(int)RoomNumber.x, (int)RoomNumber.y]; // 룸 설정

        MapObjects = new GameObject[(int)Size.x,(int)Size.y];

        for(int i=0;i<RoomNumber.x;i++)
        {
            for(int j=0;j<RoomNumber.y;j++)
            {
                GameMap.Rooms[i, j].isFind = false;
                GameMap.Rooms[i, j].RoomStartNumber = new Vector2((i * (RoomSize.x+WallWidth)), (j * (RoomSize.y + WallWidth)));
            }
        }

        #region 초기 맵 생성 알고리즘
        // 격자모양 생성 - 기본 통로형 생성
        //MakingWindow(ref GameMap, WallWidth,RoomSize, RoomNumber);
        MakingRoom(ref GameMap, WallWidth,roadWidth, RoomSize, RoomNumber);
        #endregion
    }

    #region 초기화 알고리즘
    // 격자 형태로 초기화
    void MakingWindow(ref Map GameMap, int WallWidth, Vector2 RoomSize, Vector2 RoomNumber)
    {
        // 격자 설정
        for (int i = 0; i < RoomNumber.x; i++)
        {
            for (int j = 0; j < RoomNumber.y; j++)
            {
                // 여기까지 Room 번호부여
                for (int a = 0; a < RoomSize.x; a++)
                {
                    for (int b = 0; b < RoomSize.y; b++)
                    {
                        // 이제부터 내부 채우기 -> 시작점 RoomSize.x*i + WallWidth*i , RoomSize.y*j + WallWidth * j
                        //(지금은 일단 true/false로만 채움)
                        GameMap.Tiles[i * (int)(RoomSize.x + WallWidth) + WallWidth + a,
                            j * (int)(RoomSize.y + WallWidth) + WallWidth + b].isGo = true;

                    }
                }
            }
        }
        //세로격자 채우기
        for (int i = 0; i < RoomNumber.x; i++)
        {
            for (int j = 0; j < RoomNumber.y; j++)
            {
                for (int a = 0; a < WallWidth; a++)
                {
                    for (int b = 0; b < RoomSize.y; b++)
                    {
                        // 벽 만들기(세로)
                        GameMap.Tiles[(int)(i * (RoomSize.x + WallWidth) + a),
                            (int)(j * (RoomSize.y + WallWidth) + b)].isGo = false;
                    }
                }

                for (int a = 0; a < RoomSize.x + WallWidth; a++)
                {
                    for (int b = 0; b < WallWidth; b++)
                    {
                        // 벽 만들기(세로)
                        GameMap.Tiles[(int)(i * (RoomSize.x + WallWidth) + a), 
                            (int)(j * (RoomSize.y + WallWidth) + b)].isGo = false;
                    }
                }
            }
        }
    }

    void MakingRoom(ref Map GameMap, int WallWidth, int RoadWidth, Vector2 RoomSize, Vector2 RoomNumber)
    {
        for(int i=0;i<Size.x;i++)
        {
            for(int j=0;j<Size.y;j++)
            {
                GameMap.Tiles[i, j].isGo = false;
            }
        }

        // 격자 설정
        for (int i = 0; i < RoomNumber.x; i++)
        {
            for (int j = 0; j < RoomNumber.y; j++)
            {
                // roomsize 중간에 roadsize만큼만 길 뚫기
                // -> ~(roomsize - roadsize)/2 , (roomsize+roadsize)/2 ~
                // 여기까지 Room 번호부여
                for (int a = (int)(RoomSize.x-RoadWidth)/2; a < (RoomSize.x+RoadWidth)/2; a++)
                {
                    for (int b = (int)(RoomSize.y-RoadWidth)/2; b < (RoomSize.y+RoadWidth)/2; b++)
                    {
                        // 이제부터 내부 채우기 -> 시작점 RoomSize.x*i + WallWidth*i , RoomSize.y*j + WallWidth * j
                        //(지금은 일단 true/false로만 채움)
                        GameMap.Tiles[i * (int)(RoomSize.x + WallWidth) + WallWidth + a,
                            j * (int)(RoomSize.y + WallWidth) + WallWidth + b].isGo = true;

                    }
                }
            }
        }
    }

    // 모두 true로 초기화
    void AllWhite(ref Map GameMap, Vector2 Size)
    {
        for (int i = 0; i < Size.x; i++)
        {
            for (int j = 0; j < Size.y; j++)
            {
                // 이제부터 내부 채우기 -> 시작점 RoomSize.x*i + WallWidth*i , RoomSize.y*j + WallWidth * j
                //(지금은 일단 true/false로만 채움)
                GameMap.Tiles[i,j].isGo = true;
            }
        }
    }

    // 테두리를 제외하고 true로 초기화
    void MakingOneRoom(ref Map GameMap, int border, Vector2 Size)
    {
        for (int i = 0; i < Size.x; i++)
        {
            for (int j = 0; j < Size.y; j++)
            {
                if (i < border || j < border)
                    GameMap.Tiles[i, j].isGo = false;
                else if (i >= Size.x - border || j >= Size.y - border)
                    GameMap.Tiles[i, j].isGo = false;
                else
                    GameMap.Tiles[i, j].isGo = true;
            }
        }
    }

    #endregion

    void GettingRoom(ref Map GameMap, int BoxNumbers)
    {
        int RoomSizex, RoomSizey;
        int pointX, pointY;
        for (int i=0;i<BoxNumbers;i++)
        {
            // 방의 크기 무작위 생성 -> 1x1 ~ 3x3까지
            RoomSizex = UnityEngine.Random.Range(1, 4);
            RoomSizey = UnityEngine.Random.Range(1, RoomSizex+2);

            pointX = UnityEngine.Random.Range(0, (int)RoomNumber.x - RoomSizex);
            pointY = UnityEngine.Random.Range(0, (int)RoomNumber.y - RoomSizey);

            if (isRoomSettingOK(GameMap, pointX, pointY, RoomSizex, RoomSizey))
            {
                for(int x = pointX; x<pointX+RoomSizex;x++)
                { 
                    for (int y=pointY;y<pointY+RoomSizey;y++)
                    {
                        OpenRoom(ref GameMap, x, y);
                        GameMap.Rooms[x, y].isRoom = true;
                        /*
                         * 이후 방 번호 부여, 정보 저장은 여기서
                         */
                    }
                }
                MakingBigRooms(ref GameMap, pointX, pointY, RoomSizex, RoomSizey);
            }
            else
            {
                i--;
            }
        }
    }

    void OpenRoom(ref Map GameMap,int RoomNumberx, int RoomNumbery)
    {
        for (int a = 0; a < RoomSize.x; a++)
        {
            for (int b = 0; b < RoomSize.y; b++)
            {
                // 이제부터 내부 채우기 -> 시작점 RoomSize.x*i + WallWidth*i , RoomSize.y*j + WallWidth * j
                //(지금은 일단 true/false로만 채움)
                GameMap.Tiles[RoomNumberx * (int)(RoomSize.x + WallWidth) + WallWidth + a,
                    RoomNumbery * (int)(RoomSize.y + WallWidth) + WallWidth + b].isGo = true;

            }
        }
    }

    bool isRoomSettingOK(Map GameMap, int x,int y, int widthX, int widthY)
    {
        for(int i=x; i<x+widthX;i++)
        {
            for(int j=y;j<y+widthY;j++)
            {
                if (GameMap.Rooms[i, j].isRoom)
                    return false;
            }
        }
        return true;
    }

    // 여러 블록을 합쳐 큰 하나의 방으로 변환
    void MakingBigRooms(ref Map GameMap, int x, int y, int Sizex, int Sizey)
    {

    }

    #region 메이즈 알고리즘

    void MapMaking_BST(ref Map GameMap)
    {
        Vector2 nowPos;
        for(int i=0;i<RoomSize.x;i++)
        {
            for(int j=0;j<RoomSize.y;j++)
            {
                nowPos = right2 * i + up2 * j;
                if (i != RoomSize.x - 1)
                {
                    if (j != RoomSize.y - 1)
                    {
                        int Open = UnityEngine.Random.Range(0, 2);
                        if (Open == 0)
                        {
                            // 옆을 뚫는다
                            BreakingWall(ref GameMap, nowPos, nowPos + up2);
                        }
                        else
                        {
                            // 아래를 뚫는다.
                            BreakingWall(ref GameMap, nowPos, nowPos + right2);
                        }
                    }
                    else
                    {
                        BreakingWall(ref GameMap, nowPos, nowPos + right2);
                    }
                }
                else
                {
                    if (j != RoomSize.y - 1)
                    {
                        BreakingWall(ref GameMap, nowPos, nowPos + up2);
                    }
                }
            }
        }
    }

    void MapMaking_SideWinder(ref Map GameMap)
    {
        bool isOpen;
        Vector2 nowPos;
        for(int i=0;i<RoomSize.x;i++)
        {
            isOpen = false;
            for(int j=0;j<RoomSize.y;j++)
            {
                nowPos = i * right2 + j * up2;
                
                if (i == 0)
                {
                    // 맨 처음줄은 일렬로 쭉 뚫는다.
                    if (j < RoomSize.y - 1)
                    {
                        BreakingWall(ref GameMap, nowPos, nowPos + up2);
                    }
                }
                else
                {
                    // 확률에 따라 위를 뚫는다. 만약 이미 뚫었다면, 넘어간다.
                    if (UnityEngine.Random.Range(0, 2) == 0 && !isOpen)
                    {
                        BreakingWall(ref GameMap, nowPos, nowPos - right2);
                        isOpen = true;
                    }
                    // 두번째부터는 옆으로 계속 뚫거나, 다음으로 넘어가는것을 수행한다.
                    if (UnityEngine.Random.Range(0, 2) == 0 && j < RoomSize.y - 1)
                    {
                        BreakingWall(ref GameMap, nowPos, nowPos + up2);
                    }
                    else
                    {
                        // 만약, 구획이 끝났는데도 위가 뚫리지 않으면 뚫는다.
                        if (!isOpen && i != 0)
                            BreakingWall(ref GameMap, nowPos, nowPos - right2);
                        isOpen = false;
                    }
                }           
            }
        }
    }

    void MapMaking_Stack(ref Map GameMap)
    {
        // 스택을 이용한 메이즈 알고리즘
        Vector2[] stacks = new Vector2[(int)Size.x*(int)Size.y];
        int top = -1;
        stacks[++top] = new Vector2(0, 0);
        GameMap.Rooms[0, 0].isFind = true;
        int cnt; // 진행 가능한 부분이 았는지 확인
        int num = 0;
        while(true)
        {
            if (top == -1)
                break;
            Vector2 NextRoom = stacks[top];
            cnt = getCnt(GameMap, NextRoom);
            if (cnt > 0)
            {
                while (true)
                {
                    int i = UnityEngine.Random.Range(0, 4);
                    if (i == 0)//위
                    {
                        if (NextRoom.y < RoomNumber.y - 1 && !GameMap.Rooms[(int)NextRoom.x, (int)NextRoom.y + 1].isFind)
                        {
                            BreakingWall(ref GameMap, NextRoom, NextRoom + Vector2.up);
                            NextRoom += Vector2.up;
                            break;
                        }
                    }
                    if (i == 1)//아래
                    {
                        if (NextRoom.y > 0 && !GameMap.Rooms[(int)NextRoom.x, (int)NextRoom.y - 1].isFind)
                        {
                            BreakingWall(ref GameMap, NextRoom, NextRoom - Vector2.up);
                            NextRoom -= Vector2.up;
                            break;
                        }
                    }

                    if (i == 2)//왼쪽
                    {
                        if (NextRoom.x > 0 && !GameMap.Rooms[(int)NextRoom.x - 1, (int)NextRoom.y].isFind)
                        {
                            BreakingWall(ref GameMap, NextRoom, NextRoom - Vector2.right);
                            NextRoom -= Vector2.right;
                            break;
                        }
                    }

                    if (i == 3)//오른쪽
                    {
                        if (NextRoom.x < RoomNumber.x - 1 && !GameMap.Rooms[(int)NextRoom.x + 1, (int)NextRoom.y].isFind)
                        {
                            BreakingWall(ref GameMap, NextRoom, NextRoom + Vector2.right);
                            NextRoom += Vector2.right;
                            break;
                        }
                    }

                }
                stacks[++top] = NextRoom;
                GameMap.Rooms[(int)NextRoom.x, (int)NextRoom.y].isFind = true;
            }
            else
            {
                top--;
            }
        }
    }

    #endregion

    #region 베지에 곡선 알고리즘

    Vector2[] Bezier(Vector2 start, Vector2 end,int max)// 1차 베지에 곡선(선형)
    {
        List<Vector2> info = new List<Vector2>();
        Vector2 res;
        // 베지에 곡선을 이용해 곡선이 성립하는 좌표값을 구한다.
        for(int i=0;i<=max;i++)
        {
            res = start+((end - start) / max * i);
            if(i==0)
            {
                info.Add(res);
            }
            else if((int)info[info.Count-1].x!=(int)res.x||(int)info[info.Count-1].y!=(int)res.y) // 직전값과 다른값을 가진 경우
            {
                info.Add(res); // 저장한다.
            }
        }
        Vector2[] set = new Vector2[info.Count];
        for(int i=0;i<info.Count;i++)
        {
            set[i] = info[i];
        }
        return set;
    }

    Vector2[] Bezier2(Vector2 start, Vector2 middle, Vector2 end, int max) // 2차 베지에 곡선(곡선형)
    {
        List<Vector2> info = new List<Vector2>();
        Vector2 res;
        // 베지에 곡선을 이용해 곡선이 성립하는 좌표값을 구한다.
        Vector2[] start2 = Bezier(start, middle, max);
        Vector2[] end2 = Bezier(middle, end, max);
        Debug.Log(start2.Length + " & " + end2.Length);
        for (int i = 0; i <= max; i++)
        {
            Debug.Log(i + " : " +(float)start2.Length/max*i + " , "+ (float)end2.Length/max*i);
            res = start2[(int)(((float)start2.Length - 1) / max * i)] + ((end2[(int)(((float)end2.Length-1)/max*i)]-start2[(int)(((float)start2.Length-1)/max*i)]))/max*i;
            // res = start2 + (end2-start2)/max*i
            if (i == 0)
            {
                info.Add(res);
            }
            else if ((int)info[info.Count - 1].x != (int)res.x || (int)info[info.Count - 1].y != (int)res.y) // 직전값과 다른값을 가진 경우
            {
                info.Add(res); // 저장한다.
            }
        }
        Vector2[] set = new Vector2[info.Count];
        for (int i = 0; i < info.Count; i++)
        {
            set[i] = info[i];
        }
        return set;
    }

    Vector2[] BezierSet(Vector2[] poses,int max)
    {
        Vector2[] result = new Vector2[max];
        Vector2[,] infoMat = new Vector2[max, max];
        Vector2[,] infoMatTmp = new Vector2[max, max];
        Vector2 res;

        for(int i=0;i<poses.Length-1;i++)
        {
            for(int j=0;j<=max;j++)
            {
                // res 값은 출발지 + (목적지-출발지) / 최대 값 * 현재 값
                infoMat[i,j] = poses[i] + ((poses[i + 1] - poses[i]) / max * i);
            }
        }

        return result;
    }

    #endregion

    /// <summary>
    /// 상하좌우 막혀있는지 아닌지 파악하는 함수
    /// 
    /// MapMaking(ref Map GameMap) 함수에서 사용된다.
    /// 
    /// </summary>
    /// <param name="GameMap"> 타일맵을 저장하는 구조체 </param>
    /// <param name="NextRoom"> 현재 위치 </param>
    /// <returns></returns>
    int getCnt(Map GameMap, Vector2 NextRoom)
    {
        int cnt = 0;
        // 위, 아래, 왼쪽, 오른쪽이 되는지 확인
        if (NextRoom.x > 0 && !GameMap.Rooms[(int)NextRoom.x - 1, (int)NextRoom.y].isFind)
        {
            cnt++;
        }
        if (NextRoom.y > 0 && !GameMap.Rooms[(int)NextRoom.x, (int)NextRoom.y - 1].isFind)
        {
            cnt++;
        }
        if (NextRoom.x < RoomNumber.x - 1 && !GameMap.Rooms[(int)NextRoom.x + 1, (int)NextRoom.y].isFind)
        {
            cnt++;
        }
        if (NextRoom.y < RoomNumber.y - 1 && !GameMap.Rooms[(int)NextRoom.x, (int)NextRoom.y + 1].isFind)
        {
            cnt++;
        }
        return cnt;
    }

    /// <summary>
    /// 현재 위치와 다음 위치간 막혀있는 벽을 뚫는 함수
    /// 
    /// MapMaking 함수에서 사용된다.
    /// 
    /// </summary>
    /// <param name="before"> 현재 위치 </param>
    /// <param name="after"> 이후로 이동할 위치 </param>
    void BreakingWall(ref Map GameMap, Vector2 before, Vector2 after)
    {
        // 이전위치와 이후위치 사이의 벽을 희게 바꿔준다.
        // x,y -> x+1, y or x,y -> x,y+1
        // 이때, x, y 의 위치는 (x,y)*(RoomSize+WallWidth) ~ (x,y) * (RoomSize+WallWidth) + RoomSize
        // x+1 인경우 before.x.end ~ after.x.start , before.y.start ~ after.y.end

        if (before.x < after.x || before.y < after.y)
        {
            for (int i = (int)(before.x * (RoomSize.x + WallWidth)+WallWidth); i < (after.x+1) * (RoomSize.x + WallWidth); i++)
            {
                for (int j = (int)(before.y * (RoomSize.y + WallWidth) + WallWidth); j < (after.y+1) * (RoomSize.y + WallWidth); j++)
                {
                    GameMap.Tiles[i, j].isGo = true;
                }
            }
        }
        else
        {
            for (int i = (int)(after.x * (RoomSize.x + WallWidth) + WallWidth); i < (before.x+1) * (RoomSize.x + WallWidth); i++)
            {
                for (int j = (int)(after.y * (RoomSize.y + WallWidth) + WallWidth); j < (before.y+1) * (RoomSize.y + WallWidth); j++)
                {
                    GameMap.Tiles[i, j].isGo = true;
                }
            }
        }
    }

    void MakingTile(ref Map GameMap,ref GameObject[,] MapObjects)
    {
        for(int i=0;i<Size.x;i++)
        {
            for(int j=0;j<Size.y;j++)
            {
                if(GameMap.Tiles[i,j].isGo)
                {
                    MapObjects[i, j] = Instantiate(Tiles);
                }
                else
                {
                    MapObjects[i, j] = Instantiate(Walls);
                }

                MapObjects[i, j].transform.position = firstPos + right * (i*(TileSize.x)) + up * (j*TileSize.y);
            }
        }
    }
}
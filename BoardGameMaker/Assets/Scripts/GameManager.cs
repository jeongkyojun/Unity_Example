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
    public Vector2 RomNumber;
    public Vector2 RoomStartNumber; // 방의 시작 지점(tile 기준)
    public bool isFind;
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

    [Header("세포 자동화 설정")]
    [Range(0,100)]
    public float isGroundPercent;
    public int GroundSetNumber;
    public int rotateNumber;
    public int isMountainNum;
    public int isMountainNum2;

    [Header("백트래킹 설정")]
    public Vector2 RoomNumber;
    public Vector2 RoomSize;

    [Header("산맥 관련 설정")]
    public int mountainWidth;

    public Map GameMap; // 맵 저장 정보
    public GameObject[,] MapObjects;

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
        Size = RoomSize * RoomNumber + (RoomNumber + one2) * mountainWidth;

        InitMaps(ref GameMap); // 구조체 선언 및 초기화 작업 실행

        #region 메이즈 알고리즘 3종(주석처리)
        // 메이즈 알고리즘 3종
        // 미로 생성 후, 세포자동화 수행시 산맥이 생성되지 않을까 해서 만들었으나, 안되서 주석처리함
        //MapMaking_BST(ref GameMap); // 메이즈 생성 알고리즘 1 - 이진탐색트리
        //MapMaking_SideWinder(ref GameMap); // 메이즈 생성 알고리즘 2 - 사이드와인더
        //MapMaking_Stack(ref GameMap); // 메이즈 생성 알고리즘 3 - 스택
        #endregion



        //MountainScaling(ref GameMap); // 산맥 스케일링

        MakingRiver(ref GameMap); // 강변 만들기

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
                GameMap.Rooms[i, j].RomNumber = new Vector2(i, j);
                GameMap.Rooms[i, j].RoomStartNumber = new Vector2((i * (RoomSize.x+mountainWidth)), (j * (RoomSize.y + mountainWidth)));
            }
        }

        //MakingWindow(ref GameMap, mountainWidth, RoomSize, RoomNumber);
        AllWhite(ref GameMap, Size);
    }

    #region 초기화 알고리즘
    // 격자 형태로 초기화
    void MakingWindow(ref Map GameMap, int mountainWidth, Vector2 RoomSize, Vector2 RoomNumber)
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
                        // 이제부터 내부 채우기 -> 시작점 RoomSize.x*i + mountainWidth*i , RoomSize.y*j + mountainWidth * j
                        //(지금은 일단 true/false로만 채움)
                        GameMap.Tiles[i * (int)(RoomSize.x + mountainWidth) + mountainWidth + a, j * (int)(RoomSize.y + mountainWidth) + mountainWidth + b].isGo = true;

                    }
                }
            }
        }
        //세로격자 채우기
        for (int i = 0; i < RoomNumber.x; i++)
        {
            for (int j = 0; j < RoomNumber.y; j++)
            {
                for (int a = 0; a < mountainWidth; a++)
                {
                    for (int b = 0; b < RoomSize.y; b++)
                    {
                        // 벽 만들기(세로)
                        GameMap.Tiles[(int)(i * (RoomSize.x + mountainWidth) + a), (int)(j * (RoomSize.y + mountainWidth) + b)].isGo = false;
                    }
                }

                for (int a = 0; a < RoomSize.x + mountainWidth; a++)
                {
                    for (int b = 0; b < mountainWidth; b++)
                    {
                        // 벽 만들기(세로)
                        GameMap.Tiles[(int)(i * (RoomSize.x + mountainWidth) + a), (int)(j * (RoomSize.y + mountainWidth) + b)].isGo = false;
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
                // 이제부터 내부 채우기 -> 시작점 RoomSize.x*i + mountainWidth*i , RoomSize.y*j + mountainWidth * j
                //(지금은 일단 true/false로만 채움)
                GameMap.Tiles[i,j].isGo = true;
            }
        }
    }
    #endregion

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

    Vector2[] Bezier(Vector2 start, Vector2 end,int max)// 2차 베지에 곡선
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

    Vector2[] Bezier2(Vector2 start, Vector2 middle, Vector2 end, int max) // 3차 베지에 곡선
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

    Vector2[] BezierSet(Vector2[] poses)
    {
        List<Vector2> info = new List<Vector2>();
        List<List<Vector2>> infoMat = new List<List<Vector2>>();
        int rotate = poses.Length;
        
        Vector2[] set = new Vector2[info.Count];
        for(int i=0;i<info.Count;i++)
        {
            set[i] = info[i];
        }
        return set;
    }

    #endregion

    void MakingRiver (ref Map GameMap)
    {
        int x, beforeWidth,middleWidth;
        Vector2 bef, now, middle;
        Vector2[] Line1,Line2;

        //기본 설정 - 초기값
        x = mountainWidth;
        // 무작위 지점을 구한다. 
        int RiverWidth = UnityEngine.Random.Range(3, 5);
        int y = UnityEngine.Random.Range(0, (int)Size.y - RiverWidth); // 특정 지점 설정
        now = x * right + y * up;

        middleWidth = RiverWidth;
        middle = now;

        // 2차 값
        RiverWidth = UnityEngine.Random.Range(3, 5);
        y = UnityEngine.Random.Range(0, (int)Size.y - RiverWidth); // 특정 지점 설정
        now = x * right + y * up;

        beforeWidth = middleWidth;
        bef = middle;

        middleWidth = RiverWidth;
        middle = now;

        for (int i = 2; i < RoomNumber.x; i++)
        {
            x = i * ((int)RoomSize.x + mountainWidth) + mountainWidth;
            // 무작위 지점을 방마다 구한다. 
            RiverWidth = UnityEngine.Random.Range(3, 5);
            y = UnityEngine.Random.Range(0, (int)Size.y - RiverWidth); // 특정 지점 설정
            now = x * right2 + y * up2;

            Line1 = Bezier2(bef, middle, now, 100);
            //Line2 = Bezier(bef + beforeWidth * up2, now + RiverWidth * up2, 100);

            for(int j=0;j<Line1.Length;j++)
            {
                GameMap.Tiles[(int)Line1[j].x, (int)Line1[j].y].isGo = false;
            }

            beforeWidth = middleWidth;
            bef = middle;

            middleWidth = RiverWidth;
            middle = now;
        }
    }

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
        // 이때, x, y 의 위치는 (x,y)*(RoomSize+MountainWidth) ~ (x,y) * (RoomSize+MountainWidth) + RoomSize
        // x+1 인경우 before.x.end ~ after.x.start , before.y.start ~ after.y.end

        if (before.x < after.x || before.y < after.y)
        {
            for (int i = (int)(before.x * (RoomSize.x + mountainWidth)+mountainWidth); i < (after.x+1) * (RoomSize.x + mountainWidth); i++)
            {
                for (int j = (int)(before.y * (RoomSize.y + mountainWidth) + mountainWidth); j < (after.y+1) * (RoomSize.y + mountainWidth); j++)
                {
                    GameMap.Tiles[i, j].isGo = true;
                }
            }
        }
        else
        {
            for (int i = (int)(after.x * (RoomSize.x + mountainWidth) + mountainWidth); i < (before.x+1) * (RoomSize.x + mountainWidth); i++)
            {
                for (int j = (int)(after.y * (RoomSize.y + mountainWidth) + mountainWidth); j < (before.y+1) * (RoomSize.y + mountainWidth); j++)
                {
                    GameMap.Tiles[i, j].isGo = true;
                }
            }
        }
    }

    void MountainScaling(ref Map GameMap)
    {
        Map reflica = GameMap;// reflica 에서 변경시킨뒤, 1회의 자동화가 끝나면 동기화
        // 세포 자동화를 수행한다.
        for (int rot = 0; rot < rotateNumber; rot++)
        {
            reflica = GameMap;
            for (int i = 0; i < Size.x; i++)
            {
                for (int j = 0; j < Size.y; j++)
                {
                    // Cellular 함수를 통해 주위 조건의 개수 확인, 지정조건(isMountainNum) 보다 크면 땅으로, 아니면 벽으로 변환
                    if (Cellular(ref GameMap,i,j) > isMountainNum)
                    {
                        reflica.Tiles[i, j].isGo = true;
                    }
                    else
                    {
                        reflica.Tiles[i, j].isGo = false;
                    }
                }
            }
            GameMap = reflica;
        }
    }

    int Cellular(ref Map GameMap,int x,int y)
    {
        //모든 방향의 숫자를 출력한다.
        int result = 0;
        for(int i=-1;i<=1;i++)
        {
            for(int j=-1;j<=1;j++)
            {
                if (j == 0 && i == 0)
                    continue;
                else if (x == 0 && i == -1)
                    continue;
                else if (y == 0 && j == -1)
                    continue;
                else if (x == Size.x - 1 && i == 1)
                    continue;
                else if (y == Size.y - 1 && j == 1)
                    continue;
                else
                {
                    // 여기서 주위 8칸을 확인, 조건에 맞을수록 점수가 1씩 늘어난다.
                    if(GameMap.Tiles[x+i,y+j].isGo) // true일때(땅일때) result++된다
                    {
                        result++;
                    }
                }
            }
        }
        return result; // 점수 반환
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
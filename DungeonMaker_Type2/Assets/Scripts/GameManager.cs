using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Map
{
    public Tile[,] Tiles;
    public Room[,] Rooms;
    public Box[] Boxes;
    int[,] Connected;
}

public struct Tile
{
    public bool isWall;
}

public struct Room
{
    public int RoomNumber; // 방 번호 (방을 구분한다)
    public int Dir; // 진행 방향
    public bool isRoom; // 방 생성 여부
    public bool IntegrateX; // 좌측 방 결합 여부 (직전 방이랑 합쳐졌었는지 확인)
    public bool IntegrateY; // 하단 방 결합 여부 (바로 위의 방이랑 합쳐졌었는지 확인)
    public bool isCurbe; // 방향전환 여부
}

public struct Box
{
    public Vector2Int Size; // 방 크기
    public int type; // 타입
}

public class GameManager : MonoBehaviour
{
    // 방 : 배열 자체를 의미
    // 박스 : 생성되는 방을 의미
    [Header("타일 크기")]
    public Vector2 TileSize;
    public Vector2 StartPoint;

    [Header("맵 크기, 벽 크기")]
    public Vector2Int WallSize;
    public Vector2Int RoomSize; // 방 개수, 가급적 홀수로 수행할 것
    public Vector2Int RoomNumber;
    public int BoxNumber;

    public Vector2Int Size;

    Vector2Int RoomStart;

    [Header("확률")]
    public int IntegratePercent; // 병합 확률
    public int livingPercent;

    [Header("프리팹")]
    public GameObject wall;
    public GameObject land;

    
    public Map GameMap;
    

    Vector3 right = Vector3.right;
    Vector3 up = Vector3.up;
    Vector3 forward = Vector3.forward;

    Vector2 right2 = Vector2.right;
    Vector2 up2 = Vector2.up;

    // Start is called before the first frame update
    void Start()
    {
        Size = (RoomSize * RoomNumber) + (WallSize * (RoomNumber+Vector2Int.one));
        MapMaking1(ref GameMap);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #region MapMaking1
    void MapMaking1(ref Map GameMap)
    {
        /*
            1. 무작위 방을 생성, 크기와 모양, 형태는 랜덤으로
            2. 방 중 2개를 연결, 연결 리스트로 이동
            3. 연결리스트의 방 하나를 선택해 비연결리스트의 방 하나와 연결
            4. 모든 방이 연결될때까지 반복
            5. 연결, 이와중에 만난 방과 연결관계 성립
         */
        InitState(ref GameMap);
        RoomStart = GetStart();
        MakingRooms(ref GameMap);
        SettingRoom(ref GameMap, RoomStart);
        MakingMap(ref GameMap);
    }

    void InitState(ref Map GameMap)
    {
        // 초기화 과정
        GameMap.Tiles = new Tile[Size.x, Size.y]; // 타일맵 가정
        GameMap.Rooms = new Room[RoomNumber.x, RoomNumber.y]; // 룸 생성
        GameMap.Boxes = new Box[BoxNumber];
        // 타일 초기화
        for(int i=0;i<Size.x;i++)
        {
            for(int j=0;j<Size.y;j++)
            {
                GameMap.Tiles[i, j].isWall = true;
            }
        }

        // 방 초기화
        for(int i=0;i<RoomNumber.x;i++)
        {
            for(int j=0;j<RoomNumber.y;j++)
            {
                GameMap.Rooms[i, j].isRoom = false;
                GameMap.Rooms[i, j].isCurbe = false;
                GameMap.Rooms[i, j].IntegrateX = false;
                GameMap.Rooms[i, j].IntegrateY = false;
            }
        }
    }

    void MakingRooms(ref Map GameMap)
    {
        for(int i=0;i<RoomNumber.x; i++)
        {
            for(int j=0;j<RoomNumber.y;j++)
            {
                for(int x=0;x<RoomSize.x;x++)
                {
                    for(int y=0;y<RoomSize.y;y++)
                    {
                        GameMap.Tiles[(i * (RoomSize + WallSize).x)+WallSize.x + x, (j * (RoomSize + WallSize).y)+WallSize.y + y].isWall = false;
                    }
                }
            }
        }
    }

    Vector2Int GetStart()
    {
        return RoomNumber / 2;
    }

    void SettingRoom(ref Map GameMap, Vector2Int RoomStart)
    {
        bool integrate = false;
        int X, Y;
        for (int i = 0; i < RoomNumber.x; i++)
        {
            for (int j = 0; j < RoomNumber.y; j++)
            {
                if (UnityEngine.Random.Range(0, 101) > livingPercent)
                {
                    RoomClose(ref GameMap, i, j);
                }
                else
                {
                    
                    if (i < RoomNumber.x - 1) // 우측 결합
                    {
                        if (!GameMap.Rooms[i, j].IntegrateX)// 좌측과 연결되어 있는 적이 있는지 확인
                        {
                            if (UnityEngine.Random.Range(0, 101) <= IntegratePercent)
                            {
                                if (!(j> 0 &&GameMap.Rooms[i, j].IntegrateY &&GameMap.Rooms[i, j - 1].IntegrateX)
                                    && !(i<RoomNumber.x-2&&GameMap.Rooms[i+1,j].IntegrateY&&GameMap.Rooms[i+2,j-1].IntegrateX))
                                {
                                    //  x   x   x       x   x   x
                                    //  x   -   -       -   -   x
                                    //  -   -   x       x   -   -  (두가지)
                                    
                                    // 우측 연결 (아직 상단과는 연결 안됨)
                                    // 1. 
                                    // 하단이 존재하는지 (j>0)
                                    // 하단과 연결되었는지 확인 (GameMap.Rooms[i,j].IntegrateY)
                                    // 하단이 좌측과 연결되었는지 (GameMap.Rooms[i,j-1].IntegrateX)
                                    // 2.
                                    // 앞 두칸이 존재하는지 (i<RoomNumber.x-2)
                                    // 우측하단이 우측과 연결되어있는지 (GameMap.Rooms[i+1,j].IntegrateY)
                                    // 우측하단이 우측하단의 우측과 연결되어있는지 (GameMap.Rooms[i+2,j].IntegrateX)

                                    GameMap.Rooms[i + 1, j].IntegrateX = true;
                                    BreakWall(ref GameMap, new Vector2Int(i, j), new Vector2Int(i + 1, j));
                                }
                            }
                        }
                        else if(i>0)// 좌측과 연결되어있는경우, 그리고 최하단이 아닌 경우
                        {

                        }
                    }
                    if (j < RoomNumber.y - 1)
                    {
                        if (!GameMap.Rooms[i, j].IntegrateY) // 하단과 연결되어 있는지 확인
                        {
                            // 좌측과 연결되어있지 않은 경우
                            if (UnityEngine.Random.Range(0, 101) <= IntegratePercent)
                            {
                                // 상단 연결
                                // 1. 좌측과 연결되어있는경우 좌측 하단이 연결되어있는지
                                // 2. 우측과 연결되어있는경우 우측 하단이 연결되어있는지

                                if ((i == 0 || !GameMap.Rooms[i, j].IntegrateX || !GameMap.Rooms[i - 1, j].IntegrateY)
                                    && (i==RoomNumber.x-1||!GameMap.Rooms[i+1,j].IntegrateX||!GameMap.Rooms[i+1,j].IntegrateY))
                                {
                                    //  x   x   x       x   x   x
                                    //  -   -   x       x   -   -
                                    //  -   x   x       x   x   -  (두가지)

                                    GameMap.Rooms[i, j + 1].IntegrateY = true;
                                    BreakWall(ref GameMap, new Vector2Int(i, j), new Vector2Int(i, j + 1)); // 우측 연결
                                }
                            }
                            else
                            {

                            }
                        }
                        else
                        {

                        }
                    }
                }
            }
        }
    }

    // 방을 메꾼다.
    void RoomClose(ref Map GameMap, int i, int j)
    {
        for (int x = 0; x < RoomSize.x; x++)
        {
            for (int y = 0; y < RoomSize.y; y++)
            {
                GameMap.Tiles[(i * (RoomSize + WallSize).x) + WallSize.x + x, (j * (RoomSize + WallSize).y) + WallSize.y + y].isWall = true;
            }
        }
    }

    void CloseWall(ref Map GameMap, Vector2Int before, Vector2Int after)
    {
        Vector2Int subA, subB; // subA > subB 가 성립하도록 대입
        if (before.x < after.x || before.y < after.y)
        {
            subA = after;
            subB = before;
        }
        else
        {
            subA = before;
            subB = after;
        }

        for (int i = 0; i < (subA.x == subB.x ? RoomSize.x : WallSize.x); i++)
        {
            for (int j = 0; j < (subA.y == subB.y ? RoomSize.y : WallSize.y); j++)
            {
                if (subA.x == subB.x)
                {
                    GameMap.Tiles[subB.x * (WallSize.x + RoomSize.x) + WallSize.x + i, (subB.y + 1) * (WallSize.y + RoomSize.y) + j].isWall = true;
                }
                if (subA.y == subB.y)
                {
                    GameMap.Tiles[(subB.x + 1) * (WallSize.x + RoomSize.x) + i, subB.y * (WallSize.y + RoomSize.y) + WallSize.y + j].isWall = true;
                }
            }
        }
    }

    void BreakWall(ref Map GameMap, Vector2Int before, Vector2Int after)
    {
        Vector2Int subA, subB; // subA > subB 가 성립하도록 대입
        if(before.x < after.x||before.y<after.y)
        {
            subA = after;
            subB = before;
        }
        else
        {
            subA = before;
            subB = after;
        }

        for(int i=0;i<(subA.x==subB.x?RoomSize.x:WallSize.x);i++)
        {
            for(int j=0;j<(subA.y==subB.y?RoomSize.y:WallSize.y);j++)
            {
                if(subA.x==subB.x)
                {
                    GameMap.Tiles[subB.x * (WallSize.x + RoomSize.x) + WallSize.x + i, (subB.y + 1) * (WallSize.y + RoomSize.y) + j].isWall = false;
                }
                if(subA.y==subB.y)
                {
                    GameMap.Tiles[(subB.x+1) * (WallSize.x + RoomSize.x) + i, subB.y * (WallSize.y + RoomSize.y) + WallSize.y + j].isWall = false;
                }    
            }
        }
    }

    int GetMapCnt(ref Map GameMap,int x,int y)
    {
        int cnt = 0;
        for(int i=-1;i<=1;i++)
        {
            for(int j=-1;j<=1;j++)
            {
                if (i == 0 && j == 0)
                {
                    continue;
                }
                else if (x == 0 && i == -1)
                {
                    continue;
                }
                else if (y == 0&&j==-1)
                {
                    continue;
                }
                else if(x==Size.x-1 && i==1)
                {
                    continue;
                }
                else if(y==Size.y-1&&j==1)
                {
                    continue;
                }
                else
                {
                    if (GameMap.Tiles[x + i, y + j].isWall)
                        cnt++;
                }
            }
        }
        return cnt;
    }
    void MakingMap(ref Map GameMap)
    {
        for(int i=0;i<Size.x;i++)
        {
            for(int j=0;j<Size.y;j++)
            {
                GameObject Entity;
                if (GameMap.Tiles[i,j].isWall)
                {
                    if (GetMapCnt(ref GameMap, i, j) < 8)
                    {
                        Entity = Instantiate(wall);
                        Entity.transform.position = AtoP(i, j);
                    }
                }
                else
                {
                    Entity = Instantiate(land);
                    Entity.transform.position = AtoP(i, j);
                }           
            }
        }
    }

    #endregion

    #region Array to Position (현재 Vector2Int 버전, Int버전 2개 존재)
    // Vector2Int 형으로 위치를 받을때
    Vector3 AtoP(Vector2Int Array)
    {
        return right * (Array.x * (TileSize.x) + StartPoint.x) + up * (Array.y * (TileSize.y) + StartPoint.y);
    }
    // int형으로 x,y 위치를 받을 때

    Vector3 AtoP(int x, int y)
    {
        return right * (x * (TileSize.x) + StartPoint.x) + up * (y * (TileSize.y) + StartPoint.y);
    }
    #endregion

}

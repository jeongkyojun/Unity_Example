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
    public GameObject[] Walls; // 장애물
    public GameObject[] Tiles; // 단순 지형

    public GameObject player;
    public GameObject monster;

    [Header("방 크기 설정")]
    public Vector2 Size;

    [Header("세포 자동화 설정")]
    [Range(0,100)]
    public float isGroundPercent;
    public int GroundSetNumber;
    public int rotateNumber;
    public int isMountainNum;

    [Header("백트래킹 설정")]
    public Vector2 RoomNumber;
    public Vector2 RoomSize;

    [Header("산맥 관련 설정")]
    public int mountainWidth;

    public Map GameMap; // 맵 저장 정보

    Vector3 up = Vector3.up;
    Vector3 right = Vector3.right;
    Vector3 foward = Vector3.forward;
    
    // Start is called before the first frame update
    void Start()
    {
        InitMaps(ref GameMap); // 구조체 선언 및 초기화 작업 실행

        // 산맥 형성
        MapMaking(ref GameMap);

        MakingTile(ref GameMap);
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
        GameMap.Seed = 00000042;
        UnityEngine.Random.InitState(GameMap.Seed);

        
        GameMap.Tiles = new Position[(int)Size.x, (int)Size.y]; // 타일 크기 설정

        // Xsize = RoomSizeX * RoomNumber + (RoomNumber-1) * MountainWidth 
        GameMap.Rooms = new Room[(int)RoomNumber.x, (int)RoomNumber.y]; // 룸 설정
        RoomSize = (Size - (RoomNumber - new Vector2(1, 1))*mountainWidth) / RoomNumber;

        for(int i=0;i<RoomNumber.x;i++)
        {
            for(int j=0;j<RoomNumber.y;j++)
            {
                GameMap.Rooms[i, j].isFind = false;
                GameMap.Rooms[i, j].RomNumber = new Vector2(i, j);
                GameMap.Rooms[i, j].RoomStartNumber = new Vector2((i * (RoomSize.x+mountainWidth)), (j * (RoomSize.y + mountainWidth)));
            }
        }

        // 격자 설정
        for(int i = 0;i<RoomNumber.x;i++)
        {
            for(int j=0;j<RoomNumber.y;j++)
            {
                // 여기까지 Room 번호부여
                for(int a = 0; a<RoomSize.x;a++)
                {
                    for(int b = 0; b<RoomSize.y;b++)
                    {
                        // 이제부터 내부 채우기 -> 시작점 RoomSize.x*i + mountainWidth*i , RoomSize.y*j + mountainWidth * j
                        //(지금은 일단 true/false로만 채움)
                        GameMap.Tiles[i * (int)(RoomSize.x + mountainWidth)+a, j * (int)(RoomSize.y + mountainWidth)+b].isGo = true;

                    }
                }
            }
        }
        //세로격자 채우기
        for(int i=0;i<RoomNumber.x-1;i++)
        {
            for(int j=0;j<RoomNumber.y-1;j++)
            {
                for (int a = 0; a < mountainWidth; a++)
                {
                    for (int b = 0; b < RoomSize.y; b++)
                    {
                        // 벽 만들기(세로)
                        GameMap.Tiles[(int)(i * (RoomSize.x + mountainWidth) + RoomSize.x + a), (int)(j * (RoomSize.y + mountainWidth)+ RoomSize.y + b)].isGo = false;
                    }
                }

                for(int a = 0; a<RoomSize.x+mountainWidth;a++)
                {
                    for (int b = 0; b < mountainWidth; b++)
                    {
                        // 벽 만들기(세로)
                        GameMap.Tiles[(int)(i * (RoomSize.x + mountainWidth) + a), (int)(j * (RoomSize.y + mountainWidth) + RoomSize.y + b)].isGo = false;
                    }
                }
            }
        }
    }

    void MapMaking(ref Map GameMap)
    {
        // 백트래킹을 이용한 메이즈 알고리즘
        Vector2[] stacks = new Vector2[(int)Size.x*(int)Size.y];
        int top = -1;
        stacks[++top] = new Vector2(0, 0);
        GameMap.Rooms[0, 0].isFind = true;
        int cnt; // 진행 가능한 부분이 았는지 확인
        while(top >= 0)
        {
            Vector2 NextRoom = stacks[top--];       
            cnt = getCnt(GameMap, NextRoom);

            if (cnt>0)
            {
                while(true)
                {
                    int i = UnityEngine.Random.Range(0, 4);
                    if(i==0)//위
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

    }

    void MapScaling(ref Map GameMap)
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
                    if (Cellular(ref GameMap,i,j)<isMountainNum)
                    {
                        //reflica.Tiles[i,j]
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
                    if(GameMap.Tiles[x+i,y+j].isGo)
                    {
                        result++;
                    }
                }
            }
        }
        return result;
    }

    void MakingTile(ref Map GameMap)
    {
        for(int i=0;i<Size.x;i++)
        {
            for(int j=0;j<Size.y;j++)
            {

            }
        }
    }
}
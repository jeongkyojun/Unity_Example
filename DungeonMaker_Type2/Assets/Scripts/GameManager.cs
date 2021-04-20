using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Map
{

    int[,] RoomConected; // 방끼리 연결 여부
                         // 예를 들어, Room1의 4번 문을 통해 Room4로 갈수 있는 경우 RoomConnected[1,4] = 4; 가 성립
                         // 반대로, Room4의 3번 문을 통해 Room1으로 갈수 있으면 RoomConnected[4,1] = 3;이 성립
    Room[,] Rooms;
    Box[] Boxes;
}
public struct Room
{
    bool isFind; // 방문여부 확인
    bool isRoom;

    // 아래쪽은 방인 경우에만 사용하는 변수들
    int RoomNumber; // 방 번호
    bool isDoor; // 문이 있는지 아닌지 확인
    int DoorNumber; // 문 번호를 기억
}

public struct Box
{

}

public class GameManager : MonoBehaviour
{
    public Vector2Int RoomSize; // 방으로 삼을 구역의 크기
    public Vector2Int WallSize; // 벽으로 삼을 구역의 크기
    public Vector2Int RoadSize; // 복도의 너비
    Vector2Int RoadWall; // 복도 좌,우의 벽 너비


    // Start is called before the first frame update
    void Start()
    {
        RoadWall = (RoomSize - RoadSize) / 2; // (방 너비 - 복도 너비) /2
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

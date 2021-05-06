using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 맵 관련 정보
public struct Map
{
    public Tiles[,] tiles;
}

public struct Tiles
{
    bool isGo; // 이동가능 여부 확인
    public bool isEntity; // 개체 존재 여부 확인
    public int EntityNum; // 타일과 배열 매핑
}

public struct lives
{
    public GameObject Character;
    public LivingEntity Info;
    public float Speed; // 속도
    public Vector2Int Position; // 좌표상 위치
    public Image monsterImage;
}

public class GameManager : MonoBehaviour
{
    public GameObject Land;
    public GameObject Wall;

    public Vector2Int MapSize;

    public Vector2 TileSize;
    public Vector2 StartPosition;

    [Header("monsters")]
    public int[] monster_Number; // 몬스터의 숫자
    public int[] monster_Health; // 몬스터 체력
    public float[] monster_AttackPower; // 몬스터의 공격력
    public int[] monster_DefendPower; // 몬스터의 방어력
    public float[] monster_Speed; // 몬스터의 속도
    public int[] monster_MovePower; // 몬스터의 이동력
    public GameObject[] monsters; // 몬스터의 프리팹
    public Image[] monsterImages;

    public Map GameMap;
    public lives[] Entities; // 여기에 생성된 객체를 저장한다

    public int Turn;
    public int MaxTurnNumber;
    public int monsterCnt;

    int TurnNumber;
    GameObject nowPlayer;
    // Start is called before the first frame update
    void Start()
    {
        MakingMap();
        WarGameStart();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void MakingMap()
    {
        for (int i = 0; i < MapSize.x; i++)
        {
            for (int j = 0; j < MapSize.y; j++)
            {
                GameObject Tiles = Instantiate(Land);
                Tiles.transform.position = PointToPosition(i, j);
            }
        }
    }

    void WarGameStart()
    {
        InitStage(ref GameMap); // 배열에 값 집어넣고, 
        TurnNumber = 0;
        EntitySort(ref Entities, 0, monsterCnt);
    }

    void InitStage(ref Map GameMap)
    {
        monsterCnt = -1;
        GameMap.tiles = new Tiles[MapSize.x, MapSize.y];
        Entities = new lives[1000];

        // 캐릭터 오브젝트 생성
        for (int i = 0; i < monster_Number.Length; i++)
        {
            for (int j = 0; j < monster_Number[i]; j++)
            {
                //몬스터 정보 저장
                Entities[++monsterCnt].Speed = monster_Speed[i];

                // GameObject 생성
                Entities[monsterCnt].Character = Instantiate(monsters[i]);
                // 생성된 GameObject의 자식 컴포넌트 획득
                Entities[monsterCnt].Info = Entities[monsterCnt].Character.GetComponentInChildren<LivingEntity>();
                // 지정된 이미지 저장
                //Entities[monsterCnt].monsterImage = monsterImages[i];

                // 객체별 정보 저장
                Entities[monsterCnt].Info.EntityInfo.Atk = monster_AttackPower[i];
                Entities[monsterCnt].Info.EntityInfo.Defend = monster_DefendPower[i];
                Entities[monsterCnt].Info.EntityInfo.Health = monster_Health[i];
                Entities[monsterCnt].Info.EntityInfo.MovePower = monster_MovePower[i];
                Entities[monsterCnt].Info.EntityInfo.Shield = 0;
                Entities[monsterCnt].Info.EntityInfo.unDefend = 0;

                // 위치 설정
                Entities[monsterCnt].Character.transform.position = PointToPosition3(Entities[i].Position.x, Entities[i].Position.y, 1);
            }
        }
    }

    // speed 를 기준으로 퀵소트 수행
    void EntitySort(ref lives[] Entities, int low, int high)
    {
        int i, j = low, k = low;
        lives tmp;
        if (low < high)
        {
            for (i = low + 1; i <= high; i++)
            {
                if (Entities[i].Speed < Entities[k].Speed)
                {
                    tmp = Entities[i];
                    Entities[i] = Entities[++j];
                    Entities[j] = tmp;
                }
            }
            tmp = Entities[j];
            Entities[j] = Entities[k];
            Entities[k] = tmp;

            EntitySort(ref Entities, low, j - 1);
            EntitySort(ref Entities, j + 1, high);
        }
    }

    public void ChangeTurn()
    {
        // 현재 객체의 조종권을 박탈한다.
        Entities[Turn].Info.playAble = false;

        // 현재 객체의 속도가 매우 빠르면 한번 더 이동권을 준다.
        /*
         * (미구현)
         */

        // 그렇지 않다면, 다음으로 넘긴다.
        //else 문 사용 (미구현)
        //{
        // 만약 현재 턴이 마지막 큐라면, Turn을 0으로 바꾼뒤, 재정렬을 수행한다.
        if (Turn == monsterCnt)
        {
            EntitySort(ref Entities, 0, monsterCnt);
            Turn = 0;
        }
        // 현재 객체가 죽지 않았을 때, 이동권을 부여한다.
        if (!Entities[++Turn].Info.isDead)
        {
            Entities[Turn].Info.playAble = true;
        }
        else
        {
            // 죽은 경우
        }
        //}
    }

    public void ShowEntry(ref lives Entities, int turn)
    {
       
    }

    void GetTurn(lives character)
    {
        character.Info.playAble = true;
    }
    void EndTurn(lives character)
    {
        character.Info.playAble = false;
    }

    Vector2 PointToPosition(int x, int y)
    {
        return Vector2.right * (x * (TileSize.x) + StartPosition.x) + Vector2.up * (y * (TileSize.y) + StartPosition.y);
    }

    Vector3 PointToPosition3(int x, int y,int z)
    {
        return Vector3.right * (x * (TileSize.x) + StartPosition.x) + Vector3.up * (y * (TileSize.y) + StartPosition.y) + Vector3.forward * z;
    }
}

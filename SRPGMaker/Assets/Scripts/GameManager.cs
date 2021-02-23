using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct TileInfo
{
    public position[,] pos;
};
public struct position
{
    public int row, col; // 타일 번호
    public bool isSet; // 이벤트 배치 여부
    public bool isTrue; // 이동 가능 상태
    public bool isSelect; // 선택 여부 상태
    public Color TileColor; // 타일 색깔
    public Color SelectColor; // 선택되었을때의 타일 색깔
    public Vector3 TileScale; // 타일 기본 크기
    public Vector3 TilePos; // 타일 위치
};

public class GameManager : MonoBehaviour
{
    public GameObject normalPrefab; // 기본 프리팹
    public GameObject cityPrefab; // 도시 프리팹
    GameObject[,] gameArr; // 게임 오브젝트를 담는 배열
    TileBtn[,] TileArr; // 코드를 담는 배열

    RaycastHit hitInfo;

    GameObject mouseSelect;
    position recentposition;

    TileInfo tileInfo = new TileInfo();

    Plane GroupPlane = new Plane(Vector3.forward, Vector3.zero);
    Vector3 shootingdir;

    int RowSize = 70;
    int ColSize = 70;

    // Start is called before the first frame update
    void Start()
    {
        GenerateMap(ref tileInfo,ref gameArr,ref TileArr);
    }

    private void Update()
    {
        MousePosition();
    }

    void GenerateMap(ref TileInfo tileInfo,ref GameObject[,] gameArr,ref TileBtn[,] tiles)
    {
        float firstPosRow = 0f;
        float firstPosCol = 0f;

        float firstLineRow = 0.75f;
        float firstLineCol = 1f;

        float secondLineCol = 0.5f;

        float isred=1, isblue=1, isgreen=1;

        position[,] Mappos = new position[RowSize,ColSize];
        gameArr = new GameObject[RowSize, ColSize];
        tiles = new TileBtn[RowSize, ColSize];
        for(int i=0;i<RowSize;i++)
        {
            for (int j = 0; j < ColSize; j++)
            {
                if (i / (RowSize / 3 + 1) == 0 || j / (RowSize / 3 + 1) == 0)
                    isred = 0;
                else
                    isred = 1;

                if (i / (RowSize / 3 + 1) == 1 || j / (RowSize / 3 + 1) == 1)
                    isblue = 0;
                else
                    isblue = 1;

                if (i / (RowSize / 3 + 1) == 2 || j / (RowSize / 3 + 1) == 2)
                    isgreen = 0;
                else
                    isgreen = 1;

                // 맵 정보 구조체에 정보 저장
                Mappos[i, j].row = i;
                Mappos[i, j].col = j;
                Mappos[i, j].isTrue = true;
                Mappos[i, j].TileColor = new Color(isred, isgreen, isblue);
                Mappos[i, j].SelectColor = new Color((float)(((int)isred + 1) % 2),
                    (float)(((int)isgreen + 1) % 2),
                    (float)(((int)isblue + 1) % 2));
                Mappos[i, j].TilePos = new Vector3(firstPosCol + (j * firstLineCol) + ((i % 2 == 0) ? secondLineCol : 0),// 0 + 1*n + (0.75 or 0)
                    firstPosRow + (i * firstLineRow),// 0 + 0.75 * m
                    0);
                Mappos[i, j].isSelect = false;

                // 프리팹 생성 & 게임 오브젝트 배열에 저장
                gameArr[i, j] = Instantiate(normalPrefab);
                gameArr[i, j].transform.position = Mappos[i, j].TilePos;
                tiles[i,j] = gameArr[i, j].GetComponentInChildren<TileBtn>();
                tiles[i,j].row = i;
                tiles[i,j].col = j;
                tiles[i,j].Tilesprite.color = Mappos[i, j].TileColor;
            }
        }
        tileInfo.pos = Mappos;
    }

    void MousePosition()
    {
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        float rayLength; // ray의 길이를 받는 선

        if (GroupPlane.Raycast(cameraRay, out rayLength)) // z축이 0인 가상의 평면과 교차하는경우
        {
            shootingdir = cameraRay.GetPoint(rayLength); // 끝 점을 저장받는다.
        }

        // 1 * x + 0.5(even) , 0.75 * y
        // 그렇다면, n,m의 좌표를 얻기 위해서는 
        // x 축 : n / 0.5 를 한 뒤, +1 또는 그대로의 값을 확인
        // y 축 : m / 0.75를 한 뒤, +1 또는 그대로의 값을 확인
        
        int mouseRow = (int)(shootingdir.y / 0.75f); // y축
        //(0,0.75) : 0 or 1 , (0.75,1.5) : 1 or 2

        int mouseCol = (int)((mouseRow % 2 == 0 ? 0 : 0.5f)+shootingdir.x); // x축
        //mouseCol = (int)(((mouseRow % 2 == 1) ? 0.5 : 0) + shootingdir.x);

        /*
         * shootingdir과 세 점 간 거리를 비교, 가장 거리가 짧은 점의 좌표를 받아온다.
         */
        if ((mouseRow >= 0 && mouseCol >= 0) && (mouseRow < RowSize && mouseCol < ColSize) && tileInfo.pos[mouseRow, mouseCol].isTrue)
        {
            //선택하는 세가지 좌표 안내
            if (Input.GetMouseButtonDown(0))
                Debug.Log("(" + mouseRow.ToString() + " , " + mouseCol.ToString() + ") , ("
                + (mouseRow + 1).ToString() + " , " + mouseCol.ToString() + ") , ("
                + (mouseRow + 1).ToString() + " , " + (mouseCol - 1).ToString() + ")");

            //(mouseRow,mouseCol) -> y,x
            float DistA = Vector3.Distance(shootingdir, tileInfo.pos[mouseRow, mouseCol].TilePos);
            float DistB = -1;
            float DistC = -1;
            //(mouseRow+1,mouseCol) ->y+1,x
            if (mouseRow < RowSize)
                DistB = Vector3.Distance(shootingdir, tileInfo.pos[mouseRow + 1, mouseCol].TilePos);

            //(mouseRow+1,mouseCol+1)
            if (mouseCol < ColSize && mouseRow < RowSize)
                DistC = Vector3.Distance(shootingdir, tileInfo.pos[mouseRow + 1, mouseCol + 1].TilePos);

            if (DistA > DistB)
            {
                mouseRow++;
                if (DistB > DistC)
                    mouseCol++;
            }
            else if (DistA > DistC)
            {
                mouseRow++;
                if (DistB > DistC)
                    mouseCol++;
            }

            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log(mouseRow + " , " + mouseCol);
                if (!tileInfo.pos[mouseRow, mouseCol].isSelect)
                {
                    tileInfo.pos[mouseRow, mouseCol].isSelect = true;
                    TileArr[mouseRow, mouseCol].Tilesprite.color = tileInfo.pos[mouseRow, mouseCol].SelectColor;
                }
                else
                {
                    tileInfo.pos[mouseRow, mouseCol].isSelect = false;
                    TileArr[mouseRow, mouseCol].Tilesprite.color = tileInfo.pos[mouseRow, mouseCol].TileColor;
                }

            }
        }
    }
}

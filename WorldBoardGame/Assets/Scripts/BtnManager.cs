using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnManager : MonoBehaviour
{
    // Start is called before the first frame update
    GameManager GM;
    Vector3 right = Vector3.right;
    Vector3 up = Vector3.up;

    float firstPosX; // 첫 위치 지정
    float firstPosY; // 첫 위치 지정2

    void Start()
    {
        GM = FindObjectOfType<GameManager>();
        firstPosX = GM.tileXSize / 2; // 첫 위치 지정
        firstPosY = GM.tileYSize / 2; // 첫 위치 지정2
    }


    public void OnClickLavaBtn()
    {
        
    }

    public void OnClickMapBtn()
    {

    }

    void GenerateTile(ref Tiles tiles, int SettingTime,int setTime, int Xpos, int Ypos, int continent_number)
    {
        if (tiles.Poses[Ypos, Xpos].high < 9)
        {
            tiles.Poses[Ypos, Xpos].high++;
            tiles.TileNumber[Ypos, Xpos] = continent_number;
            tiles.Poses[Ypos, Xpos].continent_number = continent_number;
        }
        if(SettingTime > setTime)
        {
            if(Xpos>0 && tiles.TileNumber[Ypos,Xpos-1]!=continent_number)
            {
                GenerateTile(ref tiles, SettingTime, setTime + 1, Xpos - 1, Ypos, continent_number);
            }
            if(Ypos>0 && tiles.TileNumber[Ypos-1, Xpos] != continent_number)
            {
                GenerateTile(ref tiles, SettingTime, setTime + 1, Xpos, Ypos-1, continent_number);
            }
            if(Xpos<GM.MaxX-1 && tiles.TileNumber[Ypos, Xpos + 1] != continent_number)
            {
                GenerateTile(ref tiles, SettingTime, setTime + 1, Xpos + 1, Ypos, continent_number);
            }
            if(Ypos<GM.MaxY-1 && tiles.TileNumber[Ypos+1, Xpos] != continent_number)
            {
                GenerateTile(ref tiles, SettingTime, setTime + 1, Xpos, Ypos+1, continent_number);
            }
        }
    }

    /*
    /// <summary>
    /// find : 시야 탐색 함수
    /// </summary>
    /// <param name="x"> 나의 현재위치 x 좌표 </param>
    /// <param name="y"> 나의 현재위치 y 좌표 </param>
    /// <param name="set"> 타일 정보 , 시야가 막히는지 아닌지 확인한다. false : 막힌다, true : 막히지 않는다.</param>
    /// <param name="range"> 시야 범위 </param>
    void find(int x, int y, bool[,] set, int range)
    {
        if(set[x,y]) // 사야가 막히지 않는다면
        {

            leftup(x,y,set, range, 1, true); // 좌상향 탐색
            rightup(x, y, set, range, 1 , true); // 우상향 탐색

            bool isSight = true;
            #region 상향, 좌우 탐색
            // 상향 탐색 - for문으로 해결
            for (int i=1;i<range; i++)
            {
                if (!set[x, y + i])
                {
                    isSight = false;
                }

                if(isSight)
                {

                }
                else
                {

                }
            }

            for (int i = 1; i < range; i++)
            {
                if (!set[x+i, y])
                {
                    isSight = false;
                }

                if (isSight)
                {

                }
                else
                {

                }
            }

            for (int i = 1; i < range; i++)
            {
                if (!set[x-i, y])
                {
                    isSight = false;
                }

                if (isSight)
                {

                }
                else
                {

                }
            }
            #endregion
        }
    }

    void leftup(int x, int y, bool[,]set, int range, int myrange, bool isSight)
    {
        // 좌상향 x - myrange, y + myrange
        
        if(!set[x-myrange,y+myrange])
        {
            isSight = false;
        }

        if(isSight)
        {
            //시야 밝힘
        }
        else
        {
            //시야끈다.
        }

        bool VerticalSight = isSight;

        // 위와 옆의 시야 확인
        for (int i = 1; i < range-myrange; i++)
        {
            if (!set[x, y + i])
            {
                VerticalSight = false;
            }

            if (VerticalSight)
            {

            }
            else
            {

            }
        }

        VerticalSight = isSight;
        // 삼각형 모양으로 하려면 추가, 싫으면 제거
        for (int i = 1; i < range-myrange; i++)
        {
            if (!set[x - i, y])
            {
                VerticalSight = false;
            }

            if (VerticalSight)
            {

            }
            else
            {

            }
        }

        if (myrange<range-myrange)
            leftup(x, y, set, range, myrange + 1, isSight);
    }
    void rightup(int x, int y, bool[,] set, int range, int myrange, bool isSight)
    {
        if (!set[x + myrange, y + myrange])
        {
            isSight = false;
        }

        if (isSight)
        {
            //시야 밝힘
        }
        else
        {
            //시야끈다.
        }

        bool VerticalSight = isSight;

        // 위와 옆의 시야 확인
        for (int i = 1; i < range - myrange; i++)
        {
            if (!set[x, y + i])
            {
                VerticalSight = false;
            }

            if (VerticalSight)
            {

            }
            else
            {

            }
        }

        VerticalSight = isSight;
        // 삼각형 모양으로 하려면 추가, 싫으면 제거
        for (int i = 1; i < range - myrange; i++)
        {
            if (!set[x + i, y])
            {
                VerticalSight = false;
            }

            if (VerticalSight)
            {

            }
            else
            {

            }
        }

        if (myrange < range - myrange)
            leftup(x, y, set, range, myrange + 1, isSight);
    }

    void sightUp(int x, int y, bool[,]Tile,bool[,]Set, int range)
    {
        //bool[,] Tile 은 기본값이 false, 시야 true
        //bool[,] set은 기본값이 true,장애물 false
        // 플레이어 위치 (x,y)


        for(int i=0;i<range;i++) // y 위치
        {
            for (int j = 0; j < range-i; j++) // x 위치
            {
                Tile[x - i, y + j] = true;
                Tile[x + i, y + j] = true;
            }
        }

        for (int i = 0; i < range; i++) // y 위치
        {
            for (int j = 0; j < range - i; j++) // x 위치
            {
                if(!Set[x-i,y+j])
                {
                    for(int n = i;n<range;n++)
                    {
                        for(int m = j+n;m<range;m++)
                        {

                        }
                    }
                }
            }
        }
    }
    */
}

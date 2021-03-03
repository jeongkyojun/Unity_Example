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

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickBtn()
    {
        bool[,] isSet = new bool[GM.GridNum, GM.GridNum];
        int Ypos, Xpos;

        int max = UnityEngine.Random.Range(GM.GridNum, GM.GridNum * GM.GridNum);
        for (int x=0;x<max;x++)
        {
            int i = UnityEngine.Random.Range(0, GM.GridNum);
            int j = UnityEngine.Random.Range(0, GM.GridNum);
            if (!isSet[i, j])
            {
                isSet[i, j] = true;

                Ypos = UnityEngine.Random.Range(GM.border + GM.BigGrid * i + GM.boundary * i, GM.border + GM.BigGrid * (i + 1) + GM.boundary * i);
                Xpos = UnityEngine.Random.Range(GM.border + GM.BigGrid * j + GM.boundary * j, GM.border + GM.BigGrid * (j + 1) + GM.boundary * j);

                int settingTime = UnityEngine.Random.Range(GM.BigGrid - GM.boundary, GM.BigGrid*2);
                // 타일에 흙을 얹는다.
                if (GM.TE.Poses[Ypos, Xpos].high < 9)
                    GenerateTile(ref GM.TE, settingTime,0, Xpos, Ypos, i*100+j);
            }
            else
                x--;
        }

        for (int i = 0; i < GM.MaxY; i++)
        {
            for (int j = 0; j < GM.MaxX; j++)
            {
                if (GM.TE.Poses[j, i].high != GM.TE.Poses[j, i].setnum)
                {
                    //GM.TilesArrEtc[GM.TE.Poses[j, i].setnum, j, i] = GM.TilesArr[j, i];
                    //GM.TilesArrEtc[GM.TE.Poses[j, i].setnum, j, i].transform.position = right * -1 *(j * GM.tileXSize + firstPosX) + up *(-1*(i * GM.tileYSize + firstPosY) +GM.MaxY* GM.TE.Poses[j, i].setnum);
                    Destroy(GM.TilesArr[j, i]);
                    GM.TilesArr[j, i] = Instantiate(GM.high[GM.TE.Poses[j, i].high]);
                    GM.TE.Poses[j, i].setnum = GM.TE.Poses[j, i].high;
                    GM.TilesArr[j, i].transform.position = right * (j * GM.tileXSize + firstPosX) + up * (i * GM.tileYSize + firstPosY); // transform.position (위치) 설정
                }
                GM.TE.TileNumber[j, i] = -1;
            }
        }
        Debug.Log("set world , max : "+max);
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManaging : MonoBehaviour
{
    Vector3 up = Vector3.up;
    Vector3 right = Vector3.right;
    Vector3 forward = Vector3.forward;

    public int X, Y,MaxX,MaxY;
    public float TileXSize,TileYSize;
    public GameObject cam;
    public GameObject minmapCam;

    public GameObject GameManager;
    GameManager GM;

    int moveCnt = 0;
    bool MoveLeft, MoveRight, MoveUp, MoveDown;

    // Start is called before the first frame update
    void Start()
    {
        GM = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        cam.transform.position = transform.position + -7*Vector3.forward;
        minmapCam.transform.position = transform.position + -5 * Vector3.forward;
        InputManaging();
        Move();
    }

    void InputManaging()
    {
        MoveLeft = Input.GetKeyDown("left");
        MoveRight = Input.GetKeyDown("right");
        MoveUp = Input.GetKeyDown("up");
        MoveDown = Input.GetKeyDown("down");
    }

    void Move()
    {
        if (MoveLeft && X > 0)
        {
            if(GM.TE.Poses[X-1,Y].isTrue)
                X--;
        }
        if (MoveRight && X < MaxX - 1)
        {
            if (GM.TE.Poses[X + 1,Y].isTrue)
                X++;
        }
        if (MoveDown && Y > 0)
        {
            if (GM.TE.Poses[X,Y-1].isTrue)
                Y--;
        }
        if (MoveUp && Y < MaxY - 1)
        {
            if (GM.TE.Poses[X,Y+1].isTrue)
                Y++;
        }
        transform.position = up * (TileYSize * Y + TileYSize / 2) + right * (TileXSize * X + TileXSize / 2) + forward * -3f;
    }
}

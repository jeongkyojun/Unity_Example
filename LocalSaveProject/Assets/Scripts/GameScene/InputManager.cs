using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

#region 입력 관련

// 키를 누르는 동안 유지되는 변수
public struct PressKey
{
    //GetAxis
    public float move;

    //
    public bool jump;

}

// 키를 누르는 그 프레임 순간만 변화
public struct GetKeyDown
{
    //GetKeyDown
    public bool look;
    public bool jump;

    public bool save;
    public bool load;
    //

}

public struct GetKeyUp
{
    //GetKeyUp
    public bool jump;
}

#endregion

public class InputManager : MonoBehaviour
{
    // c : continue, d : down, u : up
    public PressKey c;
    public GetKeyDown d;
    public GetKeyUp u;

    Dictionary<string, string> keyName = new Dictionary<string, string>();

    // Start is called before the first frame update
    void Start()
    {
        SetKeyName(out keyName);
    }

    // Update is called once per frame
    void Update()
    {
        c.move = Input.GetAxis(keyName["move"]); // move는 c(continue)에서만 있다.

        d.jump = Input.GetKeyDown(keyName["jump"]);
        u.jump = Input.GetKeyUp(keyName["jump"]);

        d.save = Input.GetKeyDown(keyName["save"]);
        d.load = Input.GetKeyDown(keyName["load"]);

        if (d.jump)
            c.jump = true;
        if (u.jump)
            c.jump = false;
    }

    void SetKeyName(out Dictionary<string, string> keyName)
    {
        Dictionary<string, string> tmp = new Dictionary<string, string>();
        Debug.Log("기본 키값 설정중...");
        /// <summary> keyName 구조체 구조
        ///struct KeyName
        ///{
        ///     public string jump;
        ///     public string move;
        ///     
        ///     public string save;
        ///     public string load;
        ///}
        /// </summary>
        /// 

        tmp.Add("jump", "space");

        tmp.Add("move", "Horizontal");

        tmp.Add("save", "c");
        tmp.Add("load", "l");

        keyName = tmp;

        Debug.Log("기본 키값 설정 완료");
    }
}

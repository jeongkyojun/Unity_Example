using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public struct InputName
{
    public string Horizontal_string_name;
    public string right;
    public string left;
    public string jump;
}

// 키 이름에 따른 능력
public enum KeyAction
{
    Up,
    Down,
    Left,
    Right,

    Jump,
    Dash,

    KeyCount
};

public static class KeySetting
{
    public static Dictionary<KeyAction, KeyCode> keys = new Dictionary<KeyAction, KeyCode>();
}
public class InputManagement : MonoBehaviour
{
    int key;
    KeyCode[] defaultKeys = new KeyCode[] {KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.Space, KeyCode.Z };
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < (int)KeyAction.KeyCount; i++)
        {
            KeySetting.keys.Add((KeyAction)i, defaultKeys[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnGUI()
    {
        Event keyEvent = Event.current;
        if(keyEvent.isKey)
            KeySetting.keys[(KeyAction)key] = keyEvent.keyCode;
    }
    public void ChangeKey(int num)
    {
        key = num;
    }
}

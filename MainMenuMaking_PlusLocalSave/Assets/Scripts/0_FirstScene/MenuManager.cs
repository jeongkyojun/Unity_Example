using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public CanvasGroup MainGroup;
    public CanvasGroup OptionGroup;
    public CanvasGroup VolumeGroup;
    public CanvasGroup LoadGroup;

    public CanvasGroup[] GroupArr = new CanvasGroup[5];
    public int ArrPoint = -1;
    // Start is called before the first frame update
    void Start()
    {
        GroupArr[++ArrPoint] = MainGroup;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

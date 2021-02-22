using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileBtn : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{ 
    public int row, col;
    public SpriteRenderer Tilesprite;
    Vector3 defaultSize;

    // Start is called before the first frame update
    void Start()
    {
        defaultSize = transform.localScale;
        //Tilesprite = GetComponent<SpriteRenderer>();
        Debug.Log("size : " + defaultSize);
    }

    public void OnPointerEnter(PointerEventData data)
    {
        transform.localScale = defaultSize * 1.1f;
    }
    public void OnPointerExit(PointerEventData data)
    {
        transform.localScale = defaultSize;
    }
}

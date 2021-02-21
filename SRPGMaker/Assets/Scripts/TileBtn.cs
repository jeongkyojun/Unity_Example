using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBtn : MonoBehaviour
{
    SpriteRenderer Tilesprite;
    // Start is called before the first frame update
    void Start()
    {
        Tilesprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnClickTileBtn()
    {
        Tilesprite.color = Color.red;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ListViewBtnImage : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Image listViewImage;
    Color defaultColor;
    // Start is called before the first frame update
    void Start()
    {
        listViewImage = GetComponent<Image>();
        defaultColor = listViewImage.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        listViewImage.color = defaultColor /2;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        listViewImage.color = defaultColor;
    }
}

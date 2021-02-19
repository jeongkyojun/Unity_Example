using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeListView : MonoBehaviour
{
    public GameObject ListPrefabs;
    int keyTypeNum = 0;
    // Start is called before the first frame update
    void Start()
    {
        for(int i=0;i<(int)KeyAction.KeyCount;i++)
        {
            GameObject ListObject = Instantiate(ListPrefabs);
            ListObject.transform.position = gameObject.transform.position;

            RectTransform listPos = ListObject.GetComponent<RectTransform>();
            ListViewKeyCodeName listClickOpt = ListObject.GetComponentInChildren<ListViewKeyCodeName>();
            ListViewKeyName listName = ListObject.GetComponentInChildren<ListViewKeyName>();

            listClickOpt.keyType = (KeyAction)i;
            listName.keyType = (KeyAction)i;

            listPos.SetParent(gameObject.transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

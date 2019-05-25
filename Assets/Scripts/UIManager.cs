using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    private GUI tileInfo;
    private GameObject currentOptionList;
    public GameObject buttonPrefab;
    public bool isMouseOnUI;
    public void manageUI(TileClass tile)
    {
        Destroy(currentOptionList);
        if (tile!=null)
        {
            print(tile.gameObject.name);
            BroadcastMessage("onTileSelected", tile);
            //string[] optionList = tile.getOptions();
            string[] optionList = { "A", "B" };
            GameObject panel = new GameObject("Panel");
            panel.AddComponent<CanvasRenderer>();
            int n = 0;
            foreach (string option in optionList)
            {
                GameObject opt = Instantiate(buttonPrefab);
                addMouseHoverListener(opt);
                opt.GetComponentInChildren<Text>().text = option;
                opt.transform.SetParent(panel.transform);
                float height = opt.GetComponent<RectTransform>().rect.height;
                opt.GetComponent<RectTransform>().anchoredPosition -= new Vector2(0, n*height);
                n++;
            }
            panel.transform.SetParent(this.transform, false);
            panel.transform.position = Input.mousePosition + new Vector3(-10,0,0);
            currentOptionList = panel;
        }
        else
        {
            print("SPACE");
            BroadcastMessage("onTileUnSelected");
        }
        
    }

    public void addMouseHoverListener(GameObject obj)
    {
        EventTrigger trigger = obj.AddComponent<EventTrigger>();
        EventTrigger.Entry entry1 = new EventTrigger.Entry();
        EventTrigger.Entry entry2 = new EventTrigger.Entry();
        entry1.eventID = EventTriggerType.PointerEnter;
        entry1.callback.AddListener((data) => { onMouseHoverUI(); });
        entry2.eventID = EventTriggerType.PointerExit;
        entry2.callback.AddListener((data) => { onMouseLeaveUI(); });
        trigger.triggers.Add(entry1);
        trigger.triggers.Add(entry2);
    }

    private void Start()
    {
        foreach(Transform child in transform)
        {
            addMouseHoverListener(child.gameObject);
        }
    }

    public void onMouseHoverUI()
    {
        this.isMouseOnUI = true;
    }
    public void onMouseLeaveUI()
    {
        this.isMouseOnUI = false;
    }
}

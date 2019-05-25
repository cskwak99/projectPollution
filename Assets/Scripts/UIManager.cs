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
    public bool isMouseOnUI;
    public void manageUI(TileClass tile)
    {
        Destroy(currentOptionList);
        if (tile!=null)
        {
            print(tile.gameObject.name);
            BroadcastMessage("onTileSelected", tile);
            //string[] optionList = tile.getOptions();
            string[] optionList = { "Worker", "B" };
            currentOptionList = this.GetComponent<OptionManager>().createOptionPanel("TileOption", gameObject, optionList, Input.mousePosition);
            Transform tmp;
            GameObject buildOptionParent;
            GameObject workerOptionParent;
            GameObject buildingOptionParent;
            if(tmp = currentOptionList.transform.Find("Build"))
            {
                buildOptionParent = tmp.gameObject;
                this.GetComponent<OptionManager>().createOptionPanel("BuildOption", buildOptionParent, optionList, currentOptionList.transform.position);
            }

            if (tmp = currentOptionList.transform.Find("Worker"))
            {
                print("YAY");
                workerOptionParent = tmp.gameObject;
                this.GetComponent<OptionManager>().createOptionPanel("WorkerOption", workerOptionParent, optionList, currentOptionList.transform.position);
            }
            if (tmp = currentOptionList.transform.Find("Building"))
            {
                buildingOptionParent = tmp.gameObject;
                this.GetComponent<OptionManager>().createOptionPanel("BuildingOption", buildingOptionParent, optionList, currentOptionList.transform.position);
            }
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

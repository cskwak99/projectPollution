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
    private OptionManager OPM;
    public bool isMouseOnUI;
    public void manageUI(TileClass tile)
    {
        Destroy(currentOptionList);
        if (tile!=null)
        {
            print(tile.gameObject.name);
            BroadcastMessage("onTileSelected", tile);
            //string[] optionList = tile.getOptions();
            string[] dummyoptionList = { "Worker", "Build", "Building" };
            currentOptionList = OPM.createOptionPanel("TileOption", gameObject, dummyoptionList, Camera.main.WorldToScreenPoint(tile.transform.position));
            Transform tmp;
            GameObject buildOption;
            string[] buildOptionList = {};
            GameObject workerOption;
            string[] workerOptionList = {};
            GameObject buildingOption;
            string[] buildingOptionList = {};
            ////////////////////////BUILD OPTION/////////////////////////////
            if(tmp = currentOptionList.transform.Find("Build"))
            {
                buildOption = tmp.gameObject;
                buildOption.GetComponent<Button>().onClick.AddListener(/*() => buildingOptionList = tile.getBuildingList()*/() => { this.setActiveOption(buildOption); });
                //Change from parent option to child options
                buildOption = OPM.createOptionPanel("BuildOption", buildOption, dummyoptionList, buildOption.transform.position);
                buildOption.SetActive(false);
            }
            ////////////////////////WORKER OPTION///////////////////////////////
            if (tmp = currentOptionList.transform.Find("Worker"))
            {
                workerOption = tmp.gameObject;
                workerOption.GetComponent<Button>().onClick.AddListener( /*() =>workerOptionList = tile.getBuildingList()*/() => { this.setActiveOption(workerOption); });
                //Change from parent option to child options
                workerOption = OPM.createOptionPanel("WorkerOption", workerOption, dummyoptionList, workerOption.transform.position);
                workerOption.SetActive(false);
            }
            ///////////////////////BUILDING OPTION/////////////////////////////////
            if (tmp = currentOptionList.transform.Find("Building"))
            {
                buildingOption = tmp.gameObject;
                buildingOption.GetComponent<Button>().onClick.AddListener(() => { this.setActiveOption(buildingOption); });
                //Change from parent option to child options
                buildingOption = OPM.createOptionPanel("BuildingOption", buildingOption, buildingOptionList, buildingOption.transform.position);
                buildingOption.SetActive(false);
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

    private void setActiveOption(GameObject daOption)
    {
        foreach(Transform child in currentOptionList.transform)
            child.Find(child.name + "Option").gameObject.SetActive(false);
        daOption.SetActive(true);
    }

    private void Start()
    {
        foreach(Transform child in transform)
        {
            addMouseHoverListener(child.gameObject);
        }
        OPM = this.GetComponent<OptionManager>();
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

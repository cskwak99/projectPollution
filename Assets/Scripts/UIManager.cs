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
    public GameObject gameEndPanel;
    public void manageUI(TileClass tile)
    {
        destroyCurrentOption();
        if (tile!=null)
        {
            print(tile.gameObject.name);
            BroadcastMessage("onTileSelected", tile);
            //string[] optionList = tile.getOptions();
            Building buildingOnTile = tile.transform.GetComponentInChildren<Building>();
            string[] dummyoptionList = { "Worker", "Build", "Building" };
            currentOptionList = OPM.createOptionPanel("TileOption", gameObject, dummyoptionList, Input.mousePosition);
            Transform tmp;
            GameObject buildOption = null;
            string[] buildOptionList = {};
            GameObject workerOption = null;
            string[] workerOptionList = {};
            GameObject buildingOption = null;
            string[] buildingOptionList = {};
            ////////////////////////BUILD OPTION/////////////////////////////
            if(tmp = currentOptionList.transform.Find("Build"))
            {
                GameObject buildOptionRoot = tmp.gameObject;
                buildOptionRoot.GetComponent<Button>().onClick.AddListener(() => {
                    buildOptionList = tile.getBuildable();
                    buildOption = OPM.createOptionPanel("BuildOption", buildOptionRoot, buildOptionList, buildOptionRoot.transform.position);
                    foreach(Transform option in buildOption.transform)
                    {
                        Button btn = option.GetComponent<Button>();
                        if (!btn)
                            continue;
                        btn.onClick.AddListener(() => {
                            GameObject.Find("_BuildManager").GetComponent<BuildManager>().route_construction(option.name, tile);
                        });
                    }
                    this.setActiveOption(buildOption, workerOption, buildingOption);
                });
            }
            ////////////////////////WORKER OPTION///////////////////////////////
            if (tmp = currentOptionList.transform.Find("Worker"))
            {
                GameObject workerOptionRoot = tmp.gameObject;
                workerOptionRoot.GetComponent<Button>().onClick.AddListener(() => {
                    if(false)
                    {
                        workerOptionList = tile.transform.GetComponentInChildren<Building>().getBuildingFunc();
                    }
                    workerOption = OPM.createOptionPanel("WorkerOption", workerOptionRoot, dummyoptionList, workerOptionRoot.transform.position);
                    this.setActiveOption(workerOption, buildingOption, buildOption);
                });
            }
            ///////////////////////BUILDING OPTION/////////////////////////////////
            if (tmp = currentOptionList.transform.Find("Building"))
            {
                GameObject buildingOptionRoot = tmp.gameObject;
                buildingOptionRoot.GetComponent<Button>().onClick.AddListener(() => {
                    if(buildingOnTile){
                        buildingOptionList = buildingOnTile.getBuildingFunc();
                    }
                    buildingOption = OPM.createOptionPanel("BuildingOption", buildingOptionRoot, buildingOptionList, buildingOptionRoot.transform.position);
                    this.setActiveOption(buildingOption, buildOption, workerOption);
                });
            }
        }
        else
        {
            print("SPACE");
            BroadcastMessage("onTileUnSelected");
        }
    }
    public void showGameEnd()
    {
        GameObject endPanel = Instantiate(gameEndPanel);
        endPanel.transform.SetParent(transform);
        endPanel.GetComponent<RectTransform>().anchoredPosition = gameObject.GetComponent<RectTransform>().anchoredPosition;
    }
    public void destroyCurrentOption()
    {
        Destroy(currentOptionList);
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

    private void setActiveOption(GameObject daOption, params GameObject[] otherOptions)
    {
        //FIRST OPTION SENT WILL BE ACTIVATED, ELSE WILL BE DEACTIVATED
        foreach (GameObject other in otherOptions)
            if(other != null)
                other.SetActive(false);
        if(daOption != null)
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

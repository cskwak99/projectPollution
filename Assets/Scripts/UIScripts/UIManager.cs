using System.Collections;
using System.Collections.Generic;

using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    private GUI tileInfo;
    private OptionManager OPM;
    public bool isMouseOnUI;
    public bool isOnDestTileSelection;
    public bool isOnTileSelected;
    public GameObject gameEndPanel;
    public GameObject popUpPanel;
    public GameObject tileSelectionBorder;
    private GameObject currentOptionList;
    private GameObject currentBorder;
    public void hoverTile(TileClass tile)
    {
        destroyBorder();
        if (tile != null)
        {
            BroadcastMessage("onTileSelected", tile);
            currentBorder = Instantiate(tileSelectionBorder, tile.gameObject.transform);
            currentBorder.transform.position = currentBorder.transform.parent.position + new Vector3(0,0.05f,0);
        }
        else
        {
            //print("SPACE");
            BroadcastMessage("onTileUnSelected");
        }

    }
    public void destroyBorder()
    {
        Destroy(currentBorder);
    }
    public void manageUI(TileClass tile)
    {
        destroyCurrentOption();
        if (tile != null)
        {
            //print(tile.gameObject.name);
            //string[] optionList = tile.getOptions();
            Building buildingOnTile = tile.transform.GetComponentInChildren<Building>();
            string[] dummyoptionList = { "Worker", "Build", "Building" };
            currentOptionList = OPM.createOptionPanel("TileOption", gameObject, dummyoptionList, Input.mousePosition);
            Transform tmp;
            GameObject buildOption = null;
            string[] buildOptionList = { };
            GameObject workerOption = null;
            string[] workerOptionList = { };
            GameObject buildingOption = null;
            string[] buildingOptionList = { };
            ////////////////////////BUILD OPTION/////////////////////////////
            if (tmp = currentOptionList.transform.Find("Build"))
            {
                GameObject buildOptionRoot = tmp.gameObject;
                buildOptionRoot.GetComponent<Button>().onClick.AddListener(() =>
                {
                    buildOptionList = tile.getBuildable();
                    //public GameObject createOptionPanel(string name, GameObject parent, string[] optionList, Vector3 iniPosition)
                    
                    if (buildingOnTile == null)
                    {
                        buildOption = OPM.createOptionPanel("BuildOption", buildOptionRoot, buildOptionList, buildOptionRoot.transform.position);
                        foreach (Transform option in buildOption.transform)
                        {
                            Button btn = option.GetComponent<Button>();
                            if (!btn)
                                continue;
                            btn.onClick.AddListener(() =>
                            {
                                GameObject.Find("_BuildManager").GetComponent<BuildManager>().route_construction(option.name, tile);
                                isMouseOnUI = false;
                            });
                        }
                        this.setActiveOption(buildOption, workerOption, buildingOption);
                    }
                });
            }
            ////////////////////////WORKER OPTION///////////////////////////////
            //worker and building should be distinguishable between players.
            if (tmp = currentOptionList.transform.Find("Worker"))
            {
                GameObject workerOptionRoot = tmp.gameObject;
                GameObject workerQueue;
                workerOptionList = tile.getWorker();
                workerOptionRoot.GetComponent<Button>().onClick.AddListener(() =>
                {
                    workerOption = OPM.createOptionPanel("WorkerOption", workerOptionRoot, workerOptionList, workerOptionRoot.transform.position);
                    foreach (Transform unitWorker in workerOption.transform)
                    {
                        GameObject unitOptionRoot = unitWorker.gameObject;
                        GameObject selected_worker = GameObject.Find(unitWorker.name);
                        GameObject current_player = GameObject.Find("TurnManager").GetComponent<TurnManager>().current_player;
                        GameObject worker_manager = current_player.GetComponent<PlayerStats>().workerManager;
                        if(selected_worker.GetComponent<worker>().is_assigned == true)
                            unitWorker.GetComponent<Image>().color = Color.red;
                        unitWorker.gameObject.GetComponent<Button>().onClick.AddListener(() =>
                        {
                            Debug.Log(unitWorker.name);                            
                            Debug.Log(selected_worker.GetComponent<worker>().location.name);                            
                            Debug.Log(worker_manager.GetComponent<WorkerManager>().player.player_number);
                            /*
                            string[] action_list;
                            if (selected_worker.GetComponent<worker>().is_assigned == false)
                            {
                                action_list = new string[] { "Move", "Pollute" };
                            }
                            else
                            {
                                action_list = new string[] { "Abort" };
                            }
                            */
                            GameObject unitOption = OPM.createOptionPanel("UnitOption", unitOptionRoot, selected_worker.GetComponent<worker>().get_action(), unitOptionRoot.transform.position);
                            foreach (Transform unitAction in unitOption.transform)
                            {
                                //Debug.Log(unitAction.gameObject.name);
                                if (unitAction.gameObject.name == "Move")
                                {
                                    unitAction.gameObject.GetComponent<Button>().onClick.AddListener(() =>
                                    {
                                        Debug.Log(unitAction.gameObject.name);
                                        StartCoroutine(worker_manager.GetComponent<WorkerManager>().move_worker(selected_worker.GetComponent<worker>()));
                                        isMouseOnUI = false;
                                        Destroy(currentOptionList);
                                    });
                                }
                                else if (unitAction.name == "Collect")
                                {
                                    unitAction.GetComponent<Button>().onClick.AddListener(() =>
                                    {
                                        Debug.Log(unitAction.gameObject.name);
                                        worker_manager.GetComponent<WorkerManager>().Update_Worker(selected_worker,worker.Action.collect,selected_worker.GetComponent<worker>().location);
                                        isMouseOnUI = false;
                                    });
                                }
                                else if (unitAction.name == "Dump")
                                {
                                    unitAction.GetComponent<Button>().onClick.AddListener(() =>
                                    {
                                        Debug.Log(unitAction.gameObject.name);
                                        worker_manager.GetComponent<WorkerManager>().Update_Worker(selected_worker, worker.Action.dump, selected_worker.GetComponent<worker>().location);
                                        isMouseOnUI = false;
                                    });
                                }
                                else if (unitAction.name == "Work")
                                {
                                    unitAction.GetComponent<Button>().onClick.AddListener(() =>
                                    {
                                        Debug.Log(unitAction.gameObject.name);
                                        worker_manager.GetComponent<WorkerManager>().Update_Worker(selected_worker, worker.Action.work, selected_worker.GetComponent<worker>().location);
                                        isMouseOnUI = false;
                                    });
                                }
                                else if (unitAction.name == "Abort")
                                {
                                    unitAction.GetComponent<Button>().onClick.AddListener(() =>
                                    {
                                        Debug.Log(unitAction.gameObject.name);
                                        worker_manager.GetComponent<WorkerManager>().Update_Worker(selected_worker, worker.Action.abort, selected_worker.GetComponent<worker>().location);
                                        isMouseOnUI = false;
                                    });
                                }
                                else if(unitAction.name == "Info")
                                {
                                    GameObject popUp = Instantiate(popUpPanel, transform, false);
                                    IEnumerator destroy_pop = DestroyPopup(popUp);
                                    unitAction.GetComponent<Button>().onClick.AddListener(() =>
                                    {
                                        popUp.transform.SetParent(transform);
                                        string popUpText = "Name : "+selected_worker.name+", HP : "+ selected_worker.GetComponent<worker>().hp+", Waste : " + selected_worker.GetComponent<worker>().waste_on_worker;
                                        Debug.Log(unitAction.gameObject.name);
                                        popUp.GetComponentInChildren<Text>().text = popUpText;
                                        //destroy_pop= DestroyPopup(popUp);
                                        StartCoroutine(destroy_pop);
                                        isMouseOnUI = false;
                                    });
                                }
                            }       
                        });
                        this.setActiveOption(workerOption, buildingOption, buildOption);
                    }
                });
                this.setActiveOption(workerOption, buildingOption, buildOption);
            }
            ///////////////////////BUILDING OPTION/////////////////////////////////
            if (tmp = currentOptionList.transform.Find("Building"))
            {
                GameObject buildingOptionRoot = tmp.gameObject;
                buildingOptionRoot.GetComponent<Button>().onClick.AddListener(() =>
                {
                    GameObject current_player = GameObject.Find("TurnManager").GetComponent<TurnManager>().current_player;
                    if (current_player.GetComponent<PlayerStats>().player_number==1 && tile.name == "0101Dome_tile")
                    {
                        buildingOptionList = new string[1] { "Info" };
                    }
                    if (current_player.GetComponent<PlayerStats>().player_number == 2 && tile.name == "1306Dome_tile")
                    {
                        buildingOptionList = new string[1] { "Info" };
                    }
                    if (buildingOnTile)
                    {
                        buildingOptionList = buildingOnTile.getBuildingFunc();
                    }
                    buildingOption = OPM.createOptionPanel("BuildingOption", buildingOptionRoot, buildingOptionList, buildingOptionRoot.transform.position);
                    foreach (Transform option in buildingOption.transform)
                    {
                        Button btn = option.GetComponent<Button>();
                        if (!btn)
                            continue;
                        btn.onClick.AddListener(() =>
                        {
                            //GameObject.Find("_BuildManager").GetComponent<BuildManager>().route_construction(option.name, tile);
                            
                            GameObject popUp = Instantiate(popUpPanel, transform, false);
                            IEnumerator destroy_pop = DestroyPopup(popUp);
                            if (option.name == "Info")
                            {
                                //popUp = Instantiate(popUpPanel, transform, false);
                                popUp.transform.SetParent(transform);
                                string popUpText;
                                if (tile.name.Substring(4) == "Dome_tile")
                                {
                                    Vector4 produce = current_player.GetComponent<PlayerStats>().dome_tile.GetComponent<Dome_tile>().resources;
                                    popUpText = "Dome current resources : " + produce.x + ", food : " + produce.y + ", metal : " + produce.z + ", waste : " + produce.w + ".";
                                }
                                else if (buildingOnTile.assignedWorker != null)
                                {            
                                    popUpText = buildingOnTile.name.Substring(0, buildingOnTile.name.Length - 7) + " : " + buildingOnTile.assignedWorker.name + " ,";
                                    Vector4 produce = buildingOnTile.getResources();
                                    if (buildingOnTile.name.Substring(0, buildingOnTile.name.Length - 7) == "Landfill")
                                    {
                                        popUpText += "waste : " + buildingOnTile.nowWaste + ", capacity : " + buildingOnTile.wasteCapacity;
                                    }
                                    else
                                    {
                                        popUpText += "produces water : " + produce.x + ", food : " + produce.y + ", metal : " + produce.z + ", waste : " + produce.w + ".";
                                        if (tile.resources.w > 0)
                                            popUpText += "current waste on tile : " + tile.resources.w;
                                    }                                    
                                    Debug.Log(buildingOnTile.buildingType);                                    
                                }
                                else
                                {
                                    popUpText = buildingOnTile.name.Substring(0, buildingOnTile.name.Length - 7) + " : worker not assigned";
                                }
                                popUp.GetComponentInChildren<Text>().text = popUpText;
                                //destroy_pop= DestroyPopup(popUp);
                                StartCoroutine(destroy_pop);
                                isMouseOnUI = false;
                            }
                            /*
                            if(option.name == "Close")
                            {
                                Debug.Log("destroy popup");
                                //StopCoroutine(destroy_pop);
                                Destroy(popUp);
                            }
                            */
                        });
                    }
                    this.setActiveOption(buildingOption, buildOption, workerOption);
                });
            }
        }
    }

    public void showGameEnd()
    {
        GameObject endPanel = Instantiate(gameEndPanel);
        endPanel.transform.SetParent(transform);
        endPanel.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
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
            Destroy(other);
        if (daOption != null)
            daOption.SetActive(true);
    }

    private void Start()
    {
        currentBorder = null;
        foreach (Transform child in transform)
        {
            if(child.name != "BuildingInfo")
                addMouseHoverListener(child.gameObject);
        }
        this.isOnDestTileSelection = false;
        BroadcastMessage("onTileUnSelected");
        OPM = this.GetComponent<OptionManager>();
    }
    public void showPopup(string popUpText)
    {
        GameObject popUp = Instantiate(popUpPanel, transform, false);
        popUp.transform.SetParent(transform);
        popUp.GetComponentInChildren<Text>().text = popUpText;
        StartCoroutine(DestroyPopup(popUp));
    }
    IEnumerator DestroyPopup(GameObject popUp)
    {
        yield return new WaitForSeconds(5);
        Destroy(popUp);
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

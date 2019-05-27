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
        if (tile != null)
        {
            print(tile.gameObject.name);
            BroadcastMessage("onTileSelected", tile);
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
                    buildOption = OPM.createOptionPanel("BuildOption", buildOptionRoot, buildOptionList, buildOptionRoot.transform.position);
                    foreach (Transform option in buildOption.transform)
                    {
                        Button btn = option.GetComponent<Button>();
                        if (!btn)
                            continue;
                        btn.onClick.AddListener(() =>
                        {
                            GameObject.Find("_BuildManager").GetComponent<BuildManager>().route_construction(option.name, tile);
                        });
                    }
                    this.setActiveOption(buildOption, workerOption, buildingOption);
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
                    //generate worker UI panel
                    if (workerOptionList.Length > 0)
                    {
                        workerOption = OPM.createOptionPanel("WorkerList", workerOptionRoot, workerOptionList, workerOptionRoot.transform.position);
                        foreach (Transform option in workerOption.transform)
                        {
                            Button btn = option.GetComponent<Button>();
                            if (!btn)
                                continue;


                            /*
                            Debug.Log(worker_action_root.transform.position);
                            worker_action_root.transform.position = btn.gameObject.transform.position;
                            */
                            GameObject worker_action_root = option.transform.gameObject;
                            btn.onClick.AddListener(() =>
                            {
                                //GameObject.Find("Option").GetComponent<UIManager>().route_construction(option.name, tile);

                                workerQueue = OPM.createOptionPanel("WorkerOption", worker_action_root, GameObject.Find(option.name).GetComponent<worker>().action_list, worker_action_root.transform.position);
                                GameObject worker = GameObject.Find(option.name);
                                GameObject worker_manager;
                                if (GameObject.Find("TurnManager").GetComponent<TurnManager>().current_player.GetComponent<PlayerStats>().player_number == 1)
                                    worker_manager = GameObject.Find("wm_player1");
                                else
                                {
                                    worker_manager = GameObject.Find("wm_player2");
                                }
                                //Debug.Log("1");
                                foreach (Transform option_ in workerQueue.transform)
                                {
                                    //Debug.Log("2");
                                    Button btn_ = option_.GetComponent<Button>();
                                    if (!btn_)
                                        continue;
                                    //Debug.Log("8");
                                    btn_.onClick.AddListener(() =>
                                    {
                                        //Debug.Log("7");
                                        if (option_.name == "idle")
                                        {
                                            //Debug.Log("5");
                                            worker_manager.GetComponent<WorkerManager>().Update_Worker(worker, (worker.Action)0, worker.GetComponent<worker>().location);
                                        }
                                        else if (option_.name == "abort")
                                        {
                                            //Debug.Log("6");
                                            worker_manager.GetComponent<WorkerManager>().Update_Worker(worker, (worker.Action)4, worker.GetComponent<worker>().location);
                                        }
                                        else
                                        {
                                            //Debug.Log("4");
                                            GameObject dest = null;
                                            Ray ray;
                                            /*
                                            while(dest==null)
                                            {
                                                Debug.Log("3");
                                                if (Input.GetMouseButtonDown(0) && !this.isMouseOnUI)
                                                {
                                                    RaycastHit hit;
                                                    ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                                                    Physics.Raycast(ray, out hit, 1000.0f);
                                                    if (hit.transform != null)
                                                    {
                                                        dest = hit.transform.gameObject;
                                                    }
                                                }
                                                await Task.Delay(10);
                                            }
                                            */
                                            if (option_.name == "move")
                                            {
                                                worker_manager.GetComponent<WorkerManager>().Update_Worker(worker, (worker.Action)1, dest);
                                            }
                                            else if (option_.name == "work")
                                            {
                                                worker_manager.GetComponent<WorkerManager>().Update_Worker(worker, (worker.Action)3, dest);
                                            }
                                            else if (option_.name == "build")
                                            {
                                                worker_manager.GetComponent<WorkerManager>().Update_Worker(worker, (worker.Action)2, dest);
                                            }
                                        }
                                    });
                                }
                            });
                        }
                    }
                    //workerOption = OPM.createOptionPanel("WorkerOption", workerOptionRoot, dummyoptionList, workerOptionRoot.transform.position);
                    this.setActiveOption(workerOption, buildingOption, buildOption);
                });
            }
            ///////////////////////BUILDING OPTION/////////////////////////////////
            if (tmp = currentOptionList.transform.Find("Building"))
            {
                GameObject buildingOptionRoot = tmp.gameObject;
                buildingOptionRoot.GetComponent<Button>().onClick.AddListener(() =>
                {
                    if (buildingOnTile)
                    {
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

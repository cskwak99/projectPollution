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
    public GameObject tileCoverPrefab;
    public GameObject arrowP1Prefab;
    public GameObject arrowP2Prefab;
    private GameObject currentOptionList;
    private GameObject currentBorder;
    private GameObject currentPopup;
    private GameObject currentCover;

    public void coverTile(List<TileClass> tile)
    {
        GameObject cover;
        currentCover = Instantiate(new GameObject());
        foreach (TileClass t in tile)
        {
            cover = Instantiate(tileCoverPrefab, currentCover.transform);
            cover.transform.position = t.transform.position + new Vector3(0, 0.05f, 0);
        }
    }
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
    public void selectTile(TileClass tile)
    {
        TurnManager TM = GameObject.Find("TurnManager").GetComponent<TurnManager>();
        destroyCurrentOption();
        destroyCurrentCover();
        if (tile!=null && (tile.isBuildingOn()|| tile.isPlayerWorkerOn(TM.current_player.GetComponent<PlayerStats>())))
        {
            isOnTileSelected = true;
            if (tile.isBuildingOn())
            {
                coverTile(tile.getBuilding().getAffectedArea());
            }
            if(tile.isPlayerWorkerOn(TM.current_player.GetComponent<PlayerStats>()))
            {
                buildPanel(tile);
            }
        }
        else
            isOnTileSelected = false;
    }

    private void buildPanel(TileClass tile)
    {
        TurnManager TM = GameObject.Find("TurnManager").GetComponent<TurnManager>();
        ////CREATE OPTION PANEL
        worker workerOnTile = tile.getWorker().GetComponent<worker>();
        string[] optionList = workerOnTile.get_action();
        Building buildingOnTile = tile.GetComponentInChildren<Building>();
        currentOptionList = OPM.createOptionPanel("TileOption", gameObject, optionList, Input.mousePosition);
        /////ADD LISTENER TO EACH OPTION///////
        Transform tmp;
        GameObject buildOption = null;
        string[] buildOptionList = { };
        ////////////////////////BUILD OPTION/////////////////////////////
        if (tmp = currentOptionList.transform.Find("Build"))
        {
            GameObject buildOptionRoot = tmp.gameObject;
            buildOptionRoot.GetComponent<Button>().onClick.AddListener(() =>
            {
                buildOptionList = tile.getBuildable();
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
                            workerOnTile.build(option.name, tile, TM.current_player);
                            destroyCurrentOption();
                            isMouseOnUI = false;
                        });
                    }
                }
            });
        }
        ///////////////////////PURIFY////////////////////////////////////
        if (tmp = currentOptionList.transform.Find("Purify"))
        {
            GameObject buildOptionRoot = tmp.gameObject;
            buildOptionRoot.GetComponent<Button>().onClick.AddListener(() =>
            {
                StartCoroutine(workerOnTile.purify_tile());
                destroyCurrentOption();
                isMouseOnUI = false;
            });
        }
        ///////////////////////MOVE////////////////////////////////////
        if (tmp = currentOptionList.transform.Find("Move"))
        {
            GameObject MoveOptionRoot = tmp.gameObject;
            MoveOptionRoot.GetComponent<Button>().onClick.AddListener(() =>
            {
                StartCoroutine(workerOnTile.move_worker());
                destroyCurrentOption();
                isMouseOnUI = false;
            });
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
        isMouseOnUI = false;
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
    public void UpdateResourcesPerTurn()
    {
        gameObject.transform.Find("Resources").GetComponent<show_resources>().UpdateResourcesPerTurn();
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
        destroyPopup();
        GameObject currentPopup = Instantiate(popUpPanel, transform, false);
        currentPopup.transform.SetParent(transform);
        currentPopup.GetComponentInChildren<Text>().text = popUpText;
        StartCoroutine(PopupBomb(currentPopup));
    }
    IEnumerator PopupBomb(GameObject popUp)
    {
        yield return new WaitForSeconds(5);
        Destroy(popUp);
    }
    public void destroyPopup()
    {
        Destroy(currentPopup);
    }
    public void onMouseHoverUI()
    {
        this.isMouseOnUI = true;
    }
    public void onMouseLeaveUI()
    {
        this.isMouseOnUI = false;
    }
    public void destroyCurrentCover()
    {
        Destroy(this.currentCover);
    }
}

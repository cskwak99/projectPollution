using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BuildingUIManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.parent.Find("BuildingInfo").GetComponent<CanvasGroup>().alpha = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void showBuildInfo(string buildingName)
    {
        transform.parent.Find("BuildingInfo").GetComponent<CanvasGroup>().alpha = 1;
        if (buildingName == "Mine")
        {
            transform.parent.Find("BuildingInfo").Find("Text").GetComponent<Text>().text = "\"Heigh-ho, heigh-ho\nIt\'s home from work we go\nHeigh - ho, heigh - ho, heigh - ho\nHeigh - ho, heigh - ho\nIt's home from work we go\nHeigh - ho, heigh - ho\"";
        }
        if (buildingName == "Farm")
        {
            transform.parent.Find("BuildingInfo").Find("Text").GetComponent<Text>().text = "We all know what a farm is for, do you need description or The foundation of sedentarism";
        }
        if (buildingName == "Residential")
        {
            transform.parent.Find("BuildingInfo").Find("Text").GetComponent<Text>().text = "The much-needed homes in this forsaken world";
        }
        if (buildingName == "Water Pump")
        {
            transform.parent.Find("BuildingInfo").Find("Text").GetComponent<Text>().text = "\"Does someone know where we get the energy for that ?\" \"No, that’s why we are worker, not nobles\"";
        }
        if (buildingName == "Landfill")
        {
            transform.parent.Find("BuildingInfo").Find("Text").GetComponent<Text>().text = "\"And that’s where we put our war weapon !\"\n\"But that’s just waste\"\n\"Nothing more deadly for the nobles ...\"";
        }
    }

    public void hideBuildInfo()
    {
        transform.parent.Find("BuildingInfo").GetComponent<CanvasGroup>().alpha = 0;
    }

    public void onClickBuildButton(string buildingName)
    {
        this.transform.parent.GetComponent<UIManager>().destroyCurrentOption();
        StartCoroutine(buildTileSelection(buildingName));
    }
    IEnumerator buildTileSelection(string buildingName)
    {
        BuildManager BM = GameObject.Find("_BuildManager").GetComponent<BuildManager>();
        TileClass destTile = null;
        yield return StartCoroutine(transform.parent.GetComponent<clickHandler>().getDestTile(tile => destTile = tile));
        if (destTile.getBuildable().Contains(buildingName))
        {
            BM.route_construction(buildingName, destTile);
        }
        else
        {
            transform.parent.GetComponent<UIManager>().showPopup("The building can't be built here");
        }
    }

}

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
        hideBuildInfo();
    }

    public void showBuildInfo(string buildingName)
    {
        BuildManager BM = GameObject.Find("_BuildManager").GetComponent<BuildManager>();
        transform.parent.Find("BuildingInfo").GetComponent<CanvasGroup>().alpha = 1;
        if (buildingName == "Mine")
        {
            transform.parent.Find("BuildingInfo").Find("Text").GetComponent<Text>().text =
                "\"Heigh-ho, heigh-ho\n" +
                "It\'s home from work we go.\n" +
                "Mine always produce toxic substances, pollution will increase on the tile\n\n" +
                "Produce " + BM.metalPerTurn + " Metal/turn, use worker\n" +
                "Cost: " + BM.mine_cost + " Metal";
        }
        if (buildingName == "Farm")
        {
            transform.parent.Find("BuildingInfo").Find("Text").GetComponent<Text>().text =
                "We all know what a farm is for, do you need description or The foundation of sedentarism\n" +
                "They all use toxic fertilizer on farm, pollution will increase on the tile\n\n" +
                "Produce " + BM.foodPerTurn + " Food/turn, use worker\n" +
                "Cost: "+BM.farm_cost+" Metal";
        }
        if (buildingName == "Residential")
        {
            transform.parent.Find("BuildingInfo").Find("Text").GetComponent<Text>().text = 
                "The much-needed homes in this forsaken world\n\n" +
                "Increase maximum workers.\n" +
                "Costs: " + BM.residential_cost + " Metal";
        }
        if (buildingName == "Water Pump")
        {
            transform.parent.Find("BuildingInfo").Find("Text").GetComponent<Text>().text =
                "\"Does someone know where we get the energy for that ?\"\n" +
                "\"No, that’s why we are worker, not nobles\"\n\n" +
                "Produce " + BM.waterPerTurn + " Water/turn, use worker\n" +
                "Cost: " + BM.water_pump_cost + " Metal";
        }
        if (buildingName == "Landfill")
        {
            transform.parent.Find("BuildingInfo").Find("Text").GetComponent<Text>().text = 
                "\"And that’s where we put our war weapon !\"\n" +
                "\"But that’s just waste\"\n" +
                "\"Nothing more deadly for the nobles ...\"\n\n"+
                "Strongly pollute tiles around.\n" +
                "Cost: " + BM.landfill_cost + " Metal";
        }
        if (buildingName == "Factory")
        {
            transform.parent.Find("BuildingInfo").Find("Text").GetComponent<Text>().text =
                "\"And that’s where we put our war weapon !\"\n" +
                "\"But that’s just waste\"\n" +
                "\"Nothing more deadly for the nobles ...\"\n\n" +
                "Slowly pollute all tiles that are 1 level above it.\n" +
                "Cost: " + BM.factory_cost + " Metal";
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
        TurnManager TM = GameObject.Find("TurnManager").GetComponent<TurnManager>();
        TileClass destTile = null;
        yield return StartCoroutine(transform.parent.GetComponent<clickHandler>().getDestTile(tile => destTile = tile));
        if (destTile.getBuildable().Contains(buildingName))
        {
            BM.route_construction(buildingName, destTile, TM.current_player);
        }
        else
        {
            transform.parent.GetComponent<UIManager>().showPopup("The building can't be built here");
        }
    }

}

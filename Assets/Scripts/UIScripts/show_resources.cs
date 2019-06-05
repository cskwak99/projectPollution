using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class show_resources : MonoBehaviour
{
    // Start is called before the first frame update
    public Text waterText;
    public Text foodText;
    public Text metalText;

    private TurnManager TM;
    string waterPerTurn = "";
    string foodPerTurn = "";
    string metalPerTurn = "";
    string wastePerTurn = "";
    void Start()
    {
        TM = GameObject.Find("TurnManager").GetComponent<TurnManager>();
        UpdateResourcesPerTurn();
    }

    public void UpdateResourcesPerTurn()
    {
        GameObject current_player = TM.GetComponent<TurnManager>().Get_current_player();
        PlayerStats player = current_player.GetComponent<PlayerStats>();
        List<GameObject> buildings = player.buildings;

        //Gather reources
        Vector4 resourcesPerTurn = new Vector4(0, 0, 0, 0);
        BuildManager BM = GameObject.Find("_BuildManager").GetComponent<BuildManager>();
        foreach (GameObject building in buildings)
        {
            Building bd = building.GetComponent<Building>();
            GameObject parentTile = bd.parentTile;
            string buildingType = bd.buildingType;
            if (parentTile.GetComponent<TileClass>().isWorkerOn() && parentTile.GetComponent<TileClass>().thresholdLvl < 1)
            {
                if (buildingType == "Farm")
                {
                    resourcesPerTurn.y += BM.foodPerTurn;
                }
                else if (buildingType == "Mine")
                {
                    resourcesPerTurn.z += BM.metalPerTurn;
                }
                else if (buildingType == "Waterpump")
                {
                    resourcesPerTurn.x += BM.waterPerTurn;
                }
            }
           
        }
        float waterConsumed = (float)player.antivaxHP_present + player.worker_present;
        float foodConsumed = (float)player.antivaxHP_present + player.worker_present;
        resourcesPerTurn.x -= waterConsumed;
        resourcesPerTurn.y -= foodConsumed;
        waterPerTurn = resourcesPerTurn.x.ToString();
        waterPerTurn = resourcesPerTurn.x > 0 ?  "+" + waterPerTurn : waterPerTurn;
        foodPerTurn = resourcesPerTurn.y.ToString();
        foodPerTurn = resourcesPerTurn.y > 0 ? "+" + foodPerTurn : foodPerTurn;
        metalPerTurn = resourcesPerTurn.z.ToString();
        metalPerTurn = resourcesPerTurn.z > 0 ? "+" + metalPerTurn : metalPerTurn;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject currentPlayer = TM.Get_current_player();
        waterText.text = "Water: " + currentPlayer.GetComponent<PlayerStats>().Get_water()+" "+waterPerTurn+"/turn";
        foodText.text = "Food: " + currentPlayer.GetComponent<PlayerStats>().Get_food()+" " + foodPerTurn + "/turn";
        metalText.text = "Metal: " + currentPlayer.GetComponent<PlayerStats>().Get_metal()+" " + metalPerTurn + "/turn";
    }
}

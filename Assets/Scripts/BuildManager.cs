using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    private GameObject target_tile;
    private GameObject clone_farm;
    private GameObject clone_waterpump;
    private GameObject clone_landfill;
    private GameObject clone_factory;
    private GameObject clone_residential;
    private GameObject clone_mine;

    public GameObject farm;
    public GameObject waterpump;
    public GameObject landfill;
    public GameObject residental;
    public GameObject mine;
    public GameObject factory;
    public GameObject P1Border;
    public GameObject P2Border;

    public int farm_cost = 3;
    public int water_pump_cost = 3;
    public int landfill_cost = 8;
    public int residential_cost = 3;
    public int mine_cost = 5;
    public int factory_cost = 10;

    public int foodPerTurn = 3;
    public int waterPerTurn = 3;
    public int metalPerTurn = 2;
    public float factoryPolluRate = 5;
    public float landfillPolluRate = 10;

    public void route_construction(string buildingName, TileClass target_tile, GameObject currentPlayer)
    {
        int res;
        UIManager UIM = GameObject.Find("UI").GetComponent<UIManager>();
        switch (buildingName)
        {
            case "Farm": res = Init_Farm(target_tile, currentPlayer); break;
            case "Water Pump": res =Init_Waterpump(target_tile, currentPlayer); break;
            case "Landfill": res = Init_Landfill(target_tile, currentPlayer); break;
            case "Residential": res = Init_Residential(target_tile, currentPlayer); break;
            case "Mine": res = Init_Mine(target_tile, currentPlayer); break;
            case "Factory": res = Init_Factory(target_tile, currentPlayer); break;
            default: return;
        }
        GameObject border;
        if (res == 0)
        {
            if (currentPlayer.name == "Player1")
                border = Instantiate(P1Border, target_tile.gameObject.transform);
            else
                border = Instantiate(P2Border, target_tile.gameObject.transform);
            border.transform.position = target_tile.transform.position + new Vector3(0, 0.05f, 0);
            UIM.UpdateResourcesPerTurn();
        }
    }

    public int Init_Farm(TileClass target_tile, GameObject currentPlayer)
    {
        if (currentPlayer.GetComponent<PlayerStats>().resources.z < 1.0f)
        {
            GameObject.Find("UI").GetComponent<UIManager>().showPopup("Insuffecient resources");
            return -1;
        }
        clone_farm = Instantiate(farm);
        clone_farm.transform.parent = target_tile.transform;
        clone_farm.GetComponent<Building>().setInitial();
        clone_farm.GetComponent<Building>().playerOccupied = currentPlayer;
        currentPlayer.GetComponent<PlayerStats>().buildings.Add(clone_farm);
        currentPlayer.GetComponent<PlayerStats>().resources.z -= 1.0f;
        return 0;
    }
    public int Init_Waterpump(TileClass target_tile, GameObject currentPlayer)
    {
        if (currentPlayer.GetComponent<PlayerStats>().resources.z < water_pump_cost)
        {
            GameObject.Find("UI").GetComponent<UIManager>().showPopup("Insuffecient resources");
            return -1;
        }
        clone_waterpump = Instantiate(waterpump);
        clone_waterpump.transform.parent = target_tile.transform;
        clone_waterpump.GetComponent<Building>().setInitial();
        clone_waterpump.GetComponent<Building>().playerOccupied = currentPlayer;
        currentPlayer.GetComponent<PlayerStats>().buildings.Add(clone_waterpump);
        currentPlayer.GetComponent<PlayerStats>().resources.z -= water_pump_cost;
        return 0;
    }
    public int Init_Landfill(TileClass target_tile, GameObject currentPlayer)
    {
        if (currentPlayer.GetComponent<PlayerStats>().resources.z < landfill_cost)
        {
            GameObject.Find("UI").GetComponent<UIManager>().showPopup("Insuffecient resources");
            return -1;
        }
        clone_landfill = Instantiate(landfill);
        clone_landfill.transform.parent = target_tile.transform;
        clone_landfill.GetComponent<Building>().setInitial();
        clone_landfill.GetComponent<Building>().playerOccupied = currentPlayer;
        currentPlayer.GetComponent<PlayerStats>().buildings.Add(clone_landfill);
        currentPlayer.GetComponent<PlayerStats>().resources.z -= landfill_cost;
        return 0;
    }
    public int Init_Factory(TileClass target_tile, GameObject currentPlayer)
    {
        if (currentPlayer.GetComponent<PlayerStats>().resources.z < factory_cost)
        {
            GameObject.Find("UI").GetComponent<UIManager>().showPopup("Insuffecient resources");
            return -1;
        }
        clone_factory = Instantiate(factory);
        clone_factory.transform.parent = target_tile.transform;
        clone_factory.GetComponent<Building>().setInitial();
        clone_factory.GetComponent<Building>().playerOccupied = currentPlayer;
        currentPlayer.GetComponent<PlayerStats>().buildings.Add(clone_factory);
        currentPlayer.GetComponent<PlayerStats>().resources.z -= factory_cost;
        return 0;
    }
    public int Init_Residential(TileClass target_tile, GameObject currentPlayer)
    {
        if (currentPlayer.GetComponent<PlayerStats>().resources.z < residential_cost)
        {
            GameObject.Find("UI").GetComponent<UIManager>().showPopup("Insuffecient resources");
            return -1;
        }
        clone_residential = Instantiate(residental);
        clone_residential.transform.parent = target_tile.transform;
        clone_residential.GetComponent<Building>().setInitial();
        clone_residential.GetComponent<Building>().playerOccupied = currentPlayer;
        currentPlayer.GetComponent<PlayerStats>().buildings.Add(clone_residential);
        currentPlayer.GetComponent<PlayerStats>().resources.z -= residential_cost;
        return 0;
    }
    public int Init_Mine(TileClass target_tile, GameObject currentPlayer)
    {
        if (currentPlayer.GetComponent<PlayerStats>().resources.z < mine_cost)
        {
            GameObject.Find("UI").GetComponent<UIManager>().showPopup("Insuffecient resources");
            return -1;
        }
        clone_mine = Instantiate(mine);
        clone_mine.transform.parent = target_tile.transform;
        clone_mine.GetComponent<Building>().setInitial();
        clone_mine.GetComponent<Building>().playerOccupied = currentPlayer;
        currentPlayer.GetComponent<PlayerStats>().buildings.Add(clone_mine);
        currentPlayer.GetComponent<PlayerStats>().resources.z -= mine_cost;
        return 0;
    }
}

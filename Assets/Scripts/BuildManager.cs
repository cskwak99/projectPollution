﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    private GameObject target_tile;
    private GameObject clone_farm;
    private GameObject clone_waterpump;
    private GameObject clone_landfill;
    private GameObject clone_residental;
    private GameObject clone_mine;
    public GameObject farm;
    public GameObject waterpump;
    public GameObject landfill;
    public GameObject residental;
    public GameObject mine;
    public GameObject TurnManager;
    public GameObject currentPlayer;
    public GameObject P1Border;
    public GameObject P2Border;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setCurrent(){
        currentPlayer = TurnManager.GetComponent<TurnManager>().current_player;
    }

    public void route_construction(string buildingName, TileClass target_tile)
    {
        setCurrent();
        int res;
        switch (buildingName)
        {
            case "Farm": res = Init_Farm(target_tile); break;
            case "Water Pump": res =Init_Waterpump(target_tile); break;
            case "Landfill": res = Init_Landfill(target_tile); break;
            case "Residential": res = Init_Residental(target_tile); break;
            case "Mine": res = Init_Mine(target_tile); break;
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
            GameObject.Find("UI").GetComponentInChildren<show_resources>().calcResourcePerTurn(currentPlayer.GetComponent<PlayerStats>());
        }
    }

    public int Init_Farm(TileClass target_tile)
    {
        if (currentPlayer.GetComponent<PlayerStats>().resources.z < 1.0f)
        {
            return -1;
        }
        clone_farm = Instantiate(farm);
        clone_farm.transform.parent = target_tile.transform;
        clone_farm.GetComponent<Building>().setInitial();
        currentPlayer.GetComponent<PlayerStats>().buildings.Add(clone_farm);
        currentPlayer.GetComponent<PlayerStats>().resources.z -= 1.0f;
        return 0;
    }
    public int Init_Waterpump(TileClass target_tile)
    {
        if (currentPlayer.GetComponent<PlayerStats>().resources.z < 1.0f)
        {
            return -1;
        }
        clone_waterpump = Instantiate(waterpump);
        clone_waterpump.transform.parent = target_tile.transform;
        clone_waterpump.GetComponent<Building>().setInitial();
        currentPlayer.GetComponent<PlayerStats>().buildings.Add(clone_waterpump);
        currentPlayer.GetComponent<PlayerStats>().resources.z -= 1.0f;
        return 0;
    }
    public int Init_Landfill(TileClass target_tile)
    {
        if (currentPlayer.GetComponent<PlayerStats>().resources.z < 1.0f)
        {
            return -1;
        }
        clone_landfill = Instantiate(landfill);
        clone_landfill.transform.parent = target_tile.transform;
        clone_landfill.GetComponent<Building>().setInitial();
        currentPlayer.GetComponent<PlayerStats>().buildings.Add(clone_landfill);
        currentPlayer.GetComponent<PlayerStats>().resources.z -= 1.0f;
        return 0;
    }
    public int Init_Residental(TileClass target_tile)
    {
        if (currentPlayer.GetComponent<PlayerStats>().resources.z < 1.0f)
        {
            return -1;
        }
        clone_residental = Instantiate(residental);
        clone_residental.transform.parent = target_tile.transform;
        clone_residental.GetComponent<Building>().setInitial();
        currentPlayer.GetComponent<PlayerStats>().buildings.Add(clone_residental);
        currentPlayer.GetComponent<PlayerStats>().resources.z -= 1.0f;
        return 0;
    }
    public int Init_Mine(TileClass target_tile)
    {
        if (currentPlayer.GetComponent<PlayerStats>().resources.z < 1.0f)
        {
            return -1;
        }
        clone_mine = Instantiate(mine);
        clone_mine.transform.parent = target_tile.transform;
        clone_mine.GetComponent<Building>().setInitial();
        currentPlayer.GetComponent<PlayerStats>().buildings.Add(clone_mine);
        currentPlayer.GetComponent<PlayerStats>().resources.z -= 1.0f;
        return 0;
    }
}

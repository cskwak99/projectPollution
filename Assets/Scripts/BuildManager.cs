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
    // Start is called before the first frame update
    void Start()
    {
        target_tile = GameObject.Find("0400Plain_tile"); // we should implement it
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void route_construction(string buildingName, TileClass target_tile)
    {
        switch (buildingName)
        {
            case "Farm": Init_Farm(target_tile); break;
            case "Water Pump": Init_Waterpump(target_tile); break;
            case "Landfill": Init_Landfill(target_tile); break;
            case "Residential": Init_Residental(target_tile); break;
            case "Mine": Init_Mine(target_tile); break;
        }
    }

    public void Init_Farm(TileClass target_tile)
    {
        clone_farm = Instantiate(farm);
        clone_farm.transform.parent = target_tile.transform;
        clone_farm.GetComponent<Building>().setInitial();
    }
    public void Init_Waterpump(TileClass target_tile)
    {
        clone_waterpump = Instantiate(waterpump);
        clone_waterpump.transform.parent = target_tile.transform;
        clone_waterpump.GetComponent<Building>().setInitial();
    }
    public void Init_Landfill(TileClass target_tile)
    {
        clone_landfill = Instantiate(landfill);
        clone_landfill.transform.parent = target_tile.transform;
        clone_landfill.GetComponent<Building>().setInitial();
    }
    public void Init_Residental(TileClass target_tile)
    {
        clone_residental = Instantiate(residental);
        clone_residental.transform.parent = target_tile.transform;
        clone_residental.GetComponent<Building>().setInitial();
    }
    public void Init_Mine(TileClass target_tile)
    {
        clone_mine = Instantiate(mine);
        clone_mine.transform.parent = target_tile.transform;
        clone_mine.GetComponent<Building>().setInitial();
    }
}

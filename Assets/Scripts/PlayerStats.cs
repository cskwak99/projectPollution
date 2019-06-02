using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public Vector4 resources; //water, food, metal, waste
    public int antivaxHP_max;
    public int antivaxHP_present;
    public int worker_max;
    public int worker_present;
    public int support_rate;
    public string[] unlocked_buildings = new string[] {"Farm", "Factory", "Waterpump", "Landfill", "Residential_area", "Mine" };
    public int player_number;
    public List<GameObject> workers;
    public List<GameObject> buildings; //saves building class instances about product to this array, and use it for resource production calculation
    public GameObject dome_tile;
    public GameObject workerManager;
    private void Start() {
        initSet();
    }
    public void initSet(){
        antivaxHP_max = 5;
        antivaxHP_present = 5;
        worker_max = 3;
        worker_present = 2;
        support_rate = 50;
        resources = new Vector4(10,10,10,0);
        //add worker list
        //add building list
        //buildings = new List<GameObject>();
        workerManager.GetComponent<WorkerManager>().init();
        foreach (GameObject building in buildings){
            building.GetComponent<Building>().setInitial();
        }
        //add dome tile
    }

    public int updateWorkerMax(){
        Debug.Log("updateWorkerMax");
        int temp = 0;
        foreach(GameObject building in buildings){
            if(building.GetComponent<Building>().isitResidential()){
                temp += 1;
            }
        }
        this.worker_max = temp/2;
        workerManager.GetComponent<WorkerManager>().worker_max = worker_max;
        return worker_max;
    }

    public PlayerStats()
    {
        player_number = 1;
    }

    public PlayerStats(int number)
    {
        player_number = number;
    }
    
    public void Update_resources(Vector4 vec4)
    {
        
        resources = resources + vec4;
    }

    public void Update_antivaxHP(int value)
    {
        antivaxHP_present = antivaxHP_present + value;
        if (antivaxHP_present >= antivaxHP_max)
        {
            antivaxHP_present = antivaxHP_max;
        }
    }

    public void Update_support(int value)
    {
        support_rate = support_rate + value;
    }

    public int Get_metal()
    {
        return (int)resources[2];
    }
    public int Get_water()
    {
        return (int)resources[0];
    }
    public int Get_food()
    {
        return (int)resources[1];
    }
    public int Get_waste()
    {
        return (int)resources[3];
    }
    public int Get_worker()
    {
        return worker_present;
    }
    public int Get_antivaxHP()
    {
        return antivaxHP_present;
    }
    public int Get_support()
    {
        return support_rate;
    }
    /*
    public void Resource_produce_perturn()
    {
        Vector4 production = new Vector4(0, 0, 0, 0);
        foreach (Building build in product_buildings)
        {
            //production = production + build.product_resource;
        }
        this.Update_resources(production);
    }
    */
}

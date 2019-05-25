using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public string buildingType; //To specify building type
    public float efficiency; //To get efficiecny of worker so that evaluate the building work //Maybe change to float
    public GameObject assignedWorker; //To check whether building can do something
    public int wasteMk; //Amount of waste that building makes
    public int airPoMk; //Amount of airPollution that factory makes
    public GameObject parentTile; //parent tile that building attached
    public string tileType; // parentTile type

    //For resource Vector -> (water, food, metal, waste)
    public void setBuildingType(){ //set Building type form its name
        string name = this.gameObject.name;
        this.buildingType = name;
    }

    public void setParentTile(){
        parentTile = this.transform.parent.gameObject;
        string tileName = parentTile.name;
        tileType = tileName.Substring(4);
    }

    public void assignWorker(GameObject worker){
        this.assignedWorker = worker;
    }

    public void setInitial(){
        this.wasteMk = 1;
        this.airPoMk = 1;

        this.setBuildingType();
        this.setParentTile();

        this.fixBuilding();
    }

    public Vector4 getResources() //For every building, return Vec4 info about resources that player get
    {
        Vector4 resources = new Vector4(0,0,0,0);
        if(assignedWorker == null){
            return new Vector4(-1,-1,-1,-1);
        }else{
            int polMeter = parentTile.GetComponent<TileClass>().thresholdLevel();
            //efficiency = this.assiagnedWorker.GetComponent<Worker>().getEfficiency(polMeter);
            if(buildingType == "Farm"){
                int food = 1;
                Vector4 required = new Vector4(0,0,0,0);
                required.x = food * efficiency;
                resources = this.parentTile.GetComponent<Plain_tile>().getResources(required);

                resources.w = wasteMk;

                return resources;
            }else if(buildingType == "Mine"){
                int metal = 1;
                Vector4 required = new Vector4(0,0,0,0);
                required.z = metal * efficiency;
                resources = this.parentTile.GetComponent<Mine_tile>().getResources(required);

                resources.w = wasteMk;

                return resources;
            }else if(buildingType == "Waterpump"){
                int water = 1;
                Vector4 required = new Vector4(0,0,0,0);
                required.y = water * efficiency;
                resources = this.parentTile.GetComponent<Water_tile>().getResources(required);

                resources.w = wasteMk;

                return resources;
            }else{
                resources.w = wasteMk;
                return resources;
            }
        }
        
    }

     public void fixBuilding(){
        string name = this.buildingType;
        Transform tile = this.parentTile.GetComponent<Transform>();

        Vector3 position = tile.position;
        if (name == "Farm" || name == "Factory" || name == "Waterpump") {
            position.y = position.y + (float)0.052;
            position.x = position.x + (float)0.03;
            position.z = position.z - (float)0.03;
        }else if(name == "Landfill"|| name == "Residential_area"|| name == "Gather"){
            position.y = position.y + (float)0.052;

        }else if(name == "Mine"){
            position.y = position.y + (float)0.052;
            position.x = position.x + (float)0.027;
        }
        this.gameObject.transform.position = position;
    }

    public int makeAirPo() //For factory building, make fixed amout of air pollution
    {
        return 5; //dummy value
    }
    
    private void Start() {
        setInitial();    
    }
    private void Update() {
        Vector4 test = this.getResources();
        Debug.Log(test);
    }
}

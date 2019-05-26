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

    public float wasteCapacity;// For landfill
    public float nowWaste; //For landfill

    //For resource Vector -> (water, food, metal, waste)
    public void setBuildingType(){ //set Building type form its name
        string name = this.gameObject.name;
        string[] temp = name.Split('(');
        this.buildingType = temp[0];
    }

    public void setParentTile(){
        parentTile = this.transform.parent.gameObject;
        string tileName = parentTile.name;
        tileType = tileName.Substring(4);
    }

    public void assignWorker(GameObject worker){
        this.assignedWorker = worker;
    }

    public void unassignWorker(){
        this.assignedWorker = null;
    }

    public void setInitial(){
        this.wasteMk = 1;
        this.airPoMk = 1;
        this.wasteCapacity = 1000f;
        this.nowWaste = 0f;

        this.setBuildingType();
        this.setParentTile();

        this.fixBuilding();
    }
    public float giveWaste(){
        float waste = 0;
        if(buildingType == "Farm"){
            waste = parentTile.GetComponent<Plain_tile>().resources.w; 
            parentTile.GetComponent<Plain_tile>().resources.w = 0;
        }else if(buildingType == "Mine"){
            waste = parentTile.GetComponent<Mine_tile>().resources.w;
            parentTile.GetComponent<Mine_tile>().resources.w = 0; 
        }else if(buildingType == "Waterpump"){
            waste = parentTile.GetComponent<Water_tile>().resources.w;
            parentTile.GetComponent<Water_tile>().resources.w = 0; 
        }
        return waste;
    }

    public float saveWaste(float waste){
        if(buildingType == "Landfill"){
            if(wasteCapacity > nowWaste + waste){
                nowWaste += waste;
                return (float)-1.0;
            }else{
                float remain = nowWaste + waste - wasteCapacity;
                nowWaste = wasteCapacity;
                return remain;
            }
        }else{
            return (float)-11.0;
        }
    }

    public Vector4 getResources() //For every building, return Vec4 info about resources that player get
    {
        Vector4 resources = new Vector4(0,0,0,0);
        if(assignedWorker == null){
            return new Vector4(0,0,0,0);
        }else{
            int polMeter = parentTile.GetComponent<TileClass>().thresholdLevel();
            //efficiency = this.assiagnedWorker.GetComponent<Worker>().getEfficiency(polMeter);
            efficiency = (float)1.0;
            if(buildingType == "Farm"){
                int food = 10;
                Vector4 required = new Vector4(0,0,0,0);
                required.y = food * efficiency;
                Debug.Log(required);
                resources = this.parentTile.GetComponent<Plain_tile>().getResources(required);
                Debug.Log(resources);

                parentTile.GetComponent<Plain_tile>().resources.w += wasteMk; 

                return resources;
            }else if(buildingType == "Mine"){
                int metal = 1;
                Vector4 required = new Vector4(0,0,0,0);
                required.z = metal * efficiency;
                resources = this.parentTile.GetComponent<Mine_tile>().getResources(required);

                parentTile.GetComponent<Mine_tile>().resources.w += wasteMk; 

                return resources;
            }else if(buildingType == "Waterpump"){
                int water = 10;
                Vector4 required = new Vector4(0,0,0,0);
                required.x = water * efficiency;
                resources = this.parentTile.GetComponent<Water_tile>().getResources(required);

                parentTile.GetComponent<Water_tile>().resources.w += wasteMk; 

                return resources;
            }else{
                return resources;
            }
        }
        
    }

    public bool isitResidential(){
        if(buildingType == "Residential_area"){
            return true;
        }else return false;
    }

    public string[] getBuildingFunc(){
        if(buildingType == "Residential_area"){
            string[] answer = {};
            return answer;
        }else if(assignedWorker == null){
            string[] answer = new string[] {"Assign Worker"};
            return answer;
        }else{
            string[] answer = new string[] {"Unassign Worker"};
            return answer;
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
        //setInitial();    
    }
    private void Update() {
        //Vector4 test = this.getResources();
        //Debug.Log(test);
    }
}

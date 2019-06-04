using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public GameObject playerOccupied;
    public string buildingType; //To specify building type
    public float efficiency; //To get efficiecny of worker so that evaluate the building work //Maybe change to float
    public int wasteMk; //Amount of waste that building makes
    public int airPoMk; //Amount of airPollution that factory makes
    public GameObject parentTile; //parent tile that building attached
    public string tileType; // parentTile type

    public float wasteCapacity;// For landfill
    public float nowWaste; //For landfill

    [SerializeField]
    private int foodPerTurn = 2;
    private int waterPerTurn = 2;
    private int metalPerTurn = 1;
    private float factoryPolluRate = 5;
    private float landfillPolluRate = 10;
    //For resource Vector -> (water, food, metal, waste)
    public void setBuildingType(){ //set Building type form its name
        string name = this.gameObject.name;
        string[] temp = name.Split('(');
        this.buildingType = temp[0];
        transform.name = temp[0];
    }

    public void setParentTile(){
        parentTile = this.transform.parent.gameObject;
        string tileName = parentTile.name;
        tileType = tileName.Substring(4);
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
        if (parentTile.GetComponent<TileClass>().isWorkerOn())
        {
            if (buildingType == "Farm")
            {
                waste = parentTile.GetComponent<Plain_tile>().resources.w;
                parentTile.GetComponent<Plain_tile>().resources.w = 0;
            }
            else if (buildingType == "Mine")
            {
                waste = parentTile.GetComponent<Mine_tile>().resources.w;
                parentTile.GetComponent<Mine_tile>().resources.w = 0;
            }
            else if (buildingType == "Waterpump")
            {
                waste = parentTile.GetComponent<Water_tile>().resources.w;
                parentTile.GetComponent<Water_tile>().resources.w = 0;
            }
        }
        return waste;
    }

    public float saveWaste(float waste){
        if (parentTile.GetComponent<TileClass>().isWorkerOn())
        {
            if (buildingType == "Landfill")
            {
                if (wasteCapacity > nowWaste + waste)
                {
                    nowWaste += waste;
                    return (float)-1.0;
                }
                else
                {
                    float remain = nowWaste + waste - wasteCapacity;
                    nowWaste = wasteCapacity;
                    return remain;
                }
            }
            else
            {
                return (float)-11.0;
            }
        }
        return (float)-11.0;
    }

    public void pollute()
    {
        if(parentTile.GetComponent<TileClass>().isWorkerOn())
        {
            print(buildingType);
            if (buildingType == "Factory")
            {
                foreach (Transform child in GameObject.Find("Hexagon_Map").transform)
                {
                    TileClass tile = child.GetComponent<TileClass>();
                    if (tile.h == parentTile.GetComponent<TileClass>().h + 1)
                        tile.UpdatePolluAmount(tile.polluAmount + factoryPolluRate);
                }
            }
            else if (buildingType == "Landfill")
            {
                foreach (TileClass tile in parentTile.GetComponent<TileClass>().getNeighbor())
                {
                    tile.UpdatePolluAmount(tile.polluAmount + landfillPolluRate);
                }
            }
            else if(buildingType == "Farm")
            {

            }

        }
       
    }

    public Vector4 getResources() //For every building, return Vec4 info about resources that player get
    {
        Vector4 resources = new Vector4(0,0,0,0);
        if(!parentTile.GetComponent<TileClass>().isWorkerOn())
        {
            return new Vector4(0,0,0,0);
        }else{
            if(buildingType == "Farm")
            {
                Vector4 required = new Vector4(0,0,0,0);
                required.y += foodPerTurn;
                //Debug.Log(required);
                resources = this.parentTile.GetComponent<Plain_tile>().getResources(required);
                //Debug.Log(resources);
                parentTile.GetComponent<Plain_tile>().resources.w += wasteMk; 
                return resources;
            }
            else if(buildingType == "Mine")
            {
                Vector4 required = new Vector4(0,0,0,0);
                required.z = metalPerTurn * efficiency;
                resources = this.parentTile.GetComponent<Mine_tile>().getResources(required);
                parentTile.GetComponent<Mine_tile>().resources.w += wasteMk; 

                return resources;
            }
            else if(buildingType == "Waterpump")
            {
                Vector4 required = new Vector4(0,0,0,0);
                required.x = waterPerTurn * efficiency;
                resources = this.parentTile.GetComponent<Water_tile>().getResources(required);
                parentTile.GetComponent<Water_tile>().resources.w += wasteMk; 
                return resources;
            }
            else
            {
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
        return new string[1] { "Info" };
        if(buildingType == "Residential_area"){
            string[] answer = {};
            return answer;
        }else if(parentTile.GetComponent<TileClass>().isWorkerOn()){
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
    
    private void Update() {
        //Vector4 test = this.getResources();
        //Debug.Log(test);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public GameObject playerOccupied;
    public string buildingType; //To specify building type
    public int wasteMk; //Amount of waste that building makes
    public int airPoMk; //Amount of airPollution that factory makes
    public GameObject parentTile; //parent tile that building attached
    public string tileType; // parentTile type

    public float wasteCapacity;// For landfill
    public float nowWaste; //For landfill

    //For resource Vector -> (water, food, metal, waste)
    private int foodPerTurn;
    private int waterPerTurn;
    private int metalPerTurn;
    private float factoryPolluRate;
    private float landfillPolluRate ;

    private void Start()
    {
        BuildManager BM = GameObject.Find("_BuildManager").GetComponent<BuildManager>();
        foodPerTurn = BM.foodPerTurn;
        waterPerTurn = BM.waterPerTurn;
        metalPerTurn = BM.metalPerTurn;
        factoryPolluRate = BM.factoryPolluRate;
        landfillPolluRate = BM.landfillPolluRate;
    }
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

    public void pollute()
    {
        if(parentTile.GetComponent<TileClass>().isWorkerOn() && parentTile.GetComponent<TileClass>().thresholdLvl < 1)
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
                    if(tile.h <= parentTile.GetComponent<TileClass>().h)
                        tile.UpdatePolluAmount(tile.polluAmount + landfillPolluRate);
                }
            }
            else if(buildingType == "Farm" || buildingType == "Mine")
            {
                parentTile.GetComponent<TileClass>().UpdatePolluAmount(parentTile.GetComponent<TileClass>().polluAmount + 5);
            }

        }
       
    }
    public List<TileClass> getAffectedArea()
    {
        List <TileClass> area = new List<TileClass>();
        if (buildingType == "Farm" || buildingType == "Mine")
        {
            area.Add(parentTile.GetComponent<TileClass>());
        }
        else if(buildingType == "Landfill")
        {
            foreach (TileClass tile in parentTile.GetComponent<TileClass>().getNeighbor())
            {
                if (tile.h <= parentTile.GetComponent<TileClass>().h)
                    area.Add(tile);
            }
        }
        else if(buildingType == "Factory")
        {
            foreach (Transform child in GameObject.Find("Hexagon_Map").transform)
            {
                TileClass tile = child.GetComponent<TileClass>();
                if (tile.h == parentTile.GetComponent<TileClass>().h + 1)
                    area.Add(tile);
            }
        }
        return area;
    }
    public Vector4 getResources() //For every building, return Vec4 info about resources that player get
    {
        Vector4 resources = new Vector4(0,0,0,0);
        if(!parentTile.GetComponent<TileClass>().isWorkerOn() || parentTile.GetComponent<TileClass>().thresholdLvl >=1)
        {
            return new Vector4(0,0,0,0);
        }
        else
        {
            if(buildingType == "Farm")
            {
                Vector4 required = new Vector4(0,0,0,0);
                required.y += foodPerTurn;
                Debug.Log(required);
                resources = this.parentTile.GetComponent<Plain_tile>().getResources(required);
                //Debug.Log(resources);
                return resources;
            }
            else if(buildingType == "Mine")
            {
                Vector4 required = new Vector4(0,0,0,0);
                required.z = metalPerTurn;
                resources = this.parentTile.GetComponent<Mine_tile>().getResources(required);
                return resources;
            }
            else if(buildingType == "Waterpump")
            {
                Vector4 required = new Vector4(0,0,0,0);
                required.x = waterPerTurn;
                resources = this.parentTile.GetComponent<Water_tile>().getResources(required);
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

}

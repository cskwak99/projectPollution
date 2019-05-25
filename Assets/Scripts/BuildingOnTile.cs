using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BuildingOnTile : MonoBehaviour
{   
    public GameObject building; //Give the building object to this variable, then if will automatically fit on the tile
    private Transform tile;

    public void fixBuilding(){
        string name = building.name;
        tile = this.gameObject.GetComponent<Transform>();
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

        building.transform.position = position;
    }

    void Start(){
        if(building != null)
            fixBuilding();
    }

}

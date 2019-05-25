using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public Vector4 resources; //metal, water, food, waste
    public int antivaxHP_max;
    public int antivaxHP_present;
    public int worker_max;
    public int worker_present;
    public int support_rate;
    public Building[] unlocked_buildings;
    public int player_number;
    public Building[] product_buildings; //saves building class instances about product to this array, and use it for resource production calculation


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
        return (int)resources[0];
    }
    public int Get_water()
    {
        return (int)resources[1];
    }
    public int Get_food()
    {
        return (int)resources[2];
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
    
    public void Resource_produce_perturn()
    {
        Vector4 production = new Vector4(0, 0, 0, 0);
        foreach (Building build in product_buildings)
        {
            production = production + build.product_resource;
        }
        this.Update_resources(production);
    }
    
}

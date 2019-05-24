using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats
{
    public Vector4 resources; //metal, water, food, waste
    public int antivaxHP_max;
    public int antivaxHP_present;
    public int worker_max;
    public int worker_present;
    public int support_rate;
    public int[] unlocked_buildings;
    public int player_number;


    public PlayerStats()
    {
        player_number = 1;
    }

    public PlayerStats(int number)
    {
        player_number = number;
    }

    public void update_resources(Vector4 vec4)
    {
        resources = resources + vec4;
    }

    public void update_antivaxHP(int value)
    {
        antivaxHP_present = antivaxHP_present + value;
        if (antivaxHP_present >= antivaxHP_max)
        {
            antivaxHP_present = antivaxHP_max;
        }
    }

    public void update_support(int value)
    {
        support_rate = support_rate + value;
    }

    public int get_metal()
    {
        return (int)resources[0];
    }
    public int get_water()
    {
        return (int)resources[1];
    }
    public int get_food()
    {
        return (int)resources[2];
    }
    public int get_waste()
    {
        return (int)resources[3];
    }
    public int get_worker()
    {
        return worker_present;
    }
    public int get_antivaxHP()
    {
        return antivaxHP_present;
    }
    public int get_support()
    {
        return support_rate;
    }
    

}

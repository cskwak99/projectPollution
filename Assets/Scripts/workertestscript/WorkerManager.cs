using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class WorkerManager : MonoBehaviour
{
    List<worker> worker_list;
    int worker_max;
    int worker_present;
    PlayerStats player;
    // Start is called before the first frame update
    public WorkerManager() { }
    public WorkerManager(PlayerStats player)
    {
        this.player = player;
        worker_max = 5;
        worker_present = 2;
        List<worker> worker_list = new List<worker>();
        for (int i = 0; i < worker_present; i++)
        {
            worker_list.Add(new worker(player,"worker"+"i"));
        }
    }
    //call by ui or something else
    //action can be move, assignment, abort
    //if you click on worker menu, you select action first and then select destination tile
    //information can be sent to the workermanager.
    public int Update_Worker(string name,int action,TileClass dest)
    {
        worker selected = new worker();
        foreach(worker candidate in worker_list)
        {
            if(candidate.name == name)
            {
                selected = candidate;
            }
        }
        //for each turn check for the left turn or to abort.
        return 0;
    }
    //calculate path from current to destination. 
    //called every turn by all moving workers
    //if there is a obstacle blocks way, calls player.
    public Vector2Int Calc_Path(TileClass curr, TileClass dest)
    {
        Vector2Int res = new Vector2Int();
        /*
        int cur_x = Convert.ToInt32(curr.gameObject.name.Substring(0, 2));
        int cur_y = Convert.ToInt32(curr.gameObject.name.Substring(2, 4));
        int dest_x = Convert.ToInt32(dest.gameObject.name.Substring(0, 2));
        int dest_y = Convert.ToInt32(dest.gameObject.name.Substring(2, 4));
        */
        //visited list
        //adjacent list, from the start, make the list.
        return res;
    } 
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

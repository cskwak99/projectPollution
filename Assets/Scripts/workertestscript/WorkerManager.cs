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
    Boolean is_updated;
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
    //Action can be only work, build, move.
    //End of the each turn, update the workers
    //Update worker by ui inputs
    public int Update_Worker(string name,worker.Action Action,GameObject dest)
    {
        worker selected = new worker();
        foreach(worker candidate in worker_list)
        {
            if(candidate.name == name)
            {
                selected = candidate;
                break;
            }
        }
        selected.cur_queue = (worker.Action)Action;
        if (Action == worker.Action.aborting)
        {
            //cancel worker action
            selected.cur_action = worker.Action.idle;
            selected.cur_queue = worker.Action.idle;
        }
        else if (!(System.Object.ReferenceEquals(selected.location, dest)))
        {
            if (Action != worker.Action.moving)
            {
                selected.cur_action = worker.Action.moving;
            }
            selected.location = Calc_Path(selected.location, dest);
            //suddenly move
            selected.worker_obj.transform.Translate(selected.location.transform.position);

        }
        else {
            if(selected.cur_action == worker.Action.moving)
            {
                selected.cur_queue = worker.Action.idle;
                selected.cur_action = worker.Action.idle;
            }
            //the worker is on the same tile player assigned
            else if (Action == worker.Action.constructing)
            {
                if (selected.turn_left > 0)
                {
                    selected.turn_left -= 1;
                }
                else {
                    selected.cur_queue = worker.Action.idle;
                    selected.cur_action = worker.Action.idle;
                    //call player it finished constructing
                    //build the actuall building
                }

            }
        }
        //for each turn check for the left turn or to abort.
        /*
        if (is_updated = false)
            is_updated = true;
        */
        return 0;
    }
    //calculate path from current to destination. 
    //called every turn by all moving workers
    //if there is a obstacle blocks way, calls player.
    public GameObject Calc_Path(GameObject curr, GameObject dest)
    {
        GameObject result = null;
        string fmt = "00";
        int cur_x = Convert.ToInt32(curr.name.Substring(0, 2));
        int cur_y = Convert.ToInt32(curr.name.Substring(2, 4));
        int dest_x = Convert.ToInt32(dest.name.Substring(0, 2));
        int dest_y = Convert.ToInt32(dest.name.Substring(2, 4));
        Vector2 dest_dir = new Vector2((float)(dest_x - cur_x), (float)(dest_y - cur_y));
        Vector2[] dir = new Vector2[6] { new Vector2(1.0f, 0.0f), new Vector2(0.5f, (float)Math.Sqrt(3) * 0.5f), new Vector2(-0.5f, (float)Math.Sqrt(3) * 0.5f), new Vector2(-1.0f, 0.0f), new Vector2(-0.5f, -1.0f*(float)Math.Sqrt(3) * 0.5f), new Vector2(0.5f, -1.0f*(float)Math.Sqrt(3) * 0.5f)};
        List<float> dot_product = new List<float>();
        Vector2Int[] tile_dir = new Vector2Int[6] { new Vector2Int(1, 0), new Vector2Int(0, 1), new Vector2Int(-1, +1), new Vector2Int(-1, 0), new Vector2Int(-1, -1), new Vector2Int(0, -1) };
        float temp;
        float max=0;
        int max_idx = 0;
        for (int i = 0; i < 6; i++)
        {
            temp = Vector2.Dot(dest_dir, dir[i]);
            dot_product.Add(temp);
        }
        while (dot_product.Count>0)
        {
            max = 0;
            max_idx = 0;
            for (int i = 0; i < dot_product.Count; i++)
            {
                if (max < dot_product[i])
                {
                    max = dot_product[i];
                    max_idx = i;
                }
            }
            string next_x = (cur_x + tile_dir[max_idx].x).ToString(fmt);
            string next_y = (cur_y + tile_dir[max_idx].y).ToString(fmt);
            result = GameObject.FindGameObjectsWithTag(next_x + next_y)[0];
            if (result.name.Substring(4).Equals("Water_Tile"))
            {
                dot_product.RemoveAt(max_idx);
                continue;
            }
            break;
        }
        //visited list
        //adjacent list, from the start, make the list.
        return result;
    } 
    void Start()
    {
        is_updated = false;
    }

    // Update is called once per frame
    void Update()
    {
    }
}

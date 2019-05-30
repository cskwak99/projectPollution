using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class WorkerManager : MonoBehaviour
{
    public List<GameObject> worker_list;
    public GameObject worker_prefab;
    int worker_max;
    int worker_present;
    int worker_num;
    public PlayerStats player;
    int worker_increase;
    int worker_acc;
    // Start is called before the first frame update
    Boolean is_updated;
    //call by ui or something else
    //action can be move, assignment, abort
    //if you click on worker menu, you select action first and then select destination tile
    //information can be sent to the workermanager.
    //Action can be only work, build, move.
    //End of the each turn, update the workers
    //Update worker by ui inputs
    public void Turn_Update_Worker()
    {
        foreach (GameObject obj in worker_list)
        {
            if (!obj.GetComponent<worker>().is_updated)
                Update_Worker(obj, obj.GetComponent<worker>().cur_action, obj.GetComponent<worker>().destination);
            Update_hp(obj);
        }
        //condition for worker increase
        Debug.Log("addworker");
        if (worker_max > worker_present)
        {
            if (worker_acc >= 100)
            {
                worker_acc = 0;
                worker_list.Add(Create_Worker(player.player_number + "worker" + worker_num));
            }
            else
            {
                worker_acc += worker_increase;
            }
        }
    }
    public void Update_hp(GameObject obj)
    {
        //update worker hp.
        /*
        if (obj.location.GetComponent<TileClass>.polluAmount)
        {

        }
        */
    }
    public GameObject Create_Worker(string name)
    {
        GameObject new_worker = Instantiate(worker_prefab);
        new_worker.name = name;
        if (player.player_number == 2)
        {
            Vector3 rot = new_worker.transform.rotation.eulerAngles;
            rot = new Vector3(rot.x, rot.y + 180, rot.z);
            new_worker.transform.rotation = Quaternion.Euler(rot);
        }
        new_worker.transform.parent = player.dome_tile.GetComponent<TileClass>().transform;
        new_worker.GetComponent<worker>().init_worker(player, name, worker_prefab);
        worker_list.Add(new_worker);
        player.dome_tile.GetComponent<TileClass>().tile_worker.Add(new_worker);
        worker_num++;
        return new_worker;
    }
    //when selected by UI
    public void Update_Worker(GameObject obj, worker.Action Action, GameObject dest)
    {
        worker selected = obj.GetComponent<worker>();
        //assigned by UI, build somewhere, move somewhere, work somewhere, Abort something
        selected.cur_queue = (worker.Action)Action;
        if (dest != selected.destination)
        {
            selected.destination = dest;
        }
        selected.is_updated = true;
        if (Action == worker.Action.aborting)
        {
            //cancel worker action
            selected.cur_action = worker.Action.idle;
            selected.cur_queue = worker.Action.idle;
            selected.destination = selected.location;
            selected.action_list = new string[4] { "idle", "move", "build", "work" };
        }
        //if not on same tile 
        else if (!(System.Object.ReferenceEquals(selected.location, dest)))
        {
            //if action was not moving, make it moving.
            if (Action != worker.Action.moving)
            {
                selected.cur_action = worker.Action.moving;
            }
            //move worker to certain place.
            selected.location.GetComponent<TileClass>().tile_worker.Remove(obj);
            selected.location = Calc_Path(selected.location, dest);
            selected.location.GetComponent<TileClass>().tile_worker.Add(obj);
            selected.action_list = new string[1] { "abort" };
            //selected.worker_obj.transform.Translate(selected.location.transform.position);
            StartCoroutine("move_worker");
        }
        else
        {
            //worker arrived to the destination
            if (selected.cur_action == worker.Action.moving)
            {
                if (selected.cur_queue == worker.Action.working)
                    selected.is_updated = false;
                selected.cur_action = selected.cur_queue;
                selected.action_list = new string[1] { "abort" };
            }
            //the worker is on the same tile player assigned
            else if (Action == worker.Action.constructing)
            {
                if (selected.turn_left > 0)
                {
                    selected.turn_left -= 1;
                    selected.action_list = new string[1] { "abort" };
                }
                else
                {
                    selected.is_updated = false;
                    selected.cur_queue = worker.Action.idle;
                    selected.cur_action = worker.Action.idle;
                    selected.action_list = new string[4] { "idle", "move", "build", "work" };
                    //call player it finished constructing
                    //build the actuall building
                }

            }
            else if (Action == worker.Action.working)
            {
                //produce resources;
                selected.action_list = new string[1] { "abort" };
            }
        }
    }

    public IEnumerator move_worker(worker obj/*, GameObject dest*/)
    {
        //Vector3 workerPos = obj.worker_obj.transform.position;
        //Vector3 destination = dest.transform.position;
        //Vector3 direction = (destination - workerPos) / 10;
        TileClass destTile = null;
        yield return StartCoroutine(GameObject.Find("UI").GetComponent<clickHandler>().getDestTile(tile => destTile=tile));
        print(destTile);
        /*
        for (int i = 0; i < 9; i++)
        {
            obj.worker_obj.transform.Translate(direction.x, direction.y, direction.z);
            //yield return new WaitForSeconds(0.05f);
        }
        */
        //obj.worker_obj.transform.position = destination;
        //yield return new WaitForSeconds(0.05f);
    }
    //calculate path from current to destination. 
    //called every turn by all moving workers
    //if there is a obstacle blocks way, calls player
    public GameObject Calc_Path(GameObject curr, GameObject dest)
    {
        GameObject result = null;
        string fmt = "00";
        int cur_x = Convert.ToInt32(curr.name.Substring(0, 2));
        int cur_y = Convert.ToInt32(curr.name.Substring(2, 4));
        int dest_x = Convert.ToInt32(dest.name.Substring(0, 2));
        int dest_y = Convert.ToInt32(dest.name.Substring(2, 4));
        Vector2 dest_dir = new Vector2((float)(dest_x - cur_x), (float)(dest_y - cur_y));
        Vector2[] dir = new Vector2[6] { new Vector2(1.0f, 0.0f), new Vector2(0.5f, (float)Math.Sqrt(3) * 0.5f), new Vector2(-0.5f, (float)Math.Sqrt(3) * 0.5f), new Vector2(-1.0f, 0.0f), new Vector2(-0.5f, -1.0f * (float)Math.Sqrt(3) * 0.5f), new Vector2(0.5f, -1.0f * (float)Math.Sqrt(3) * 0.5f) };
        List<float> dot_product = new List<float>();
        Vector2Int[] tile_dir = new Vector2Int[6] { new Vector2Int(1, 0), new Vector2Int(0, 1), new Vector2Int(-1, +1), new Vector2Int(-1, 0), new Vector2Int(-1, -1), new Vector2Int(0, -1) };
        float temp;
        float max = 0;
        int max_idx = 0;
        for (int i = 0; i < 6; i++)
        {
            temp = Vector2.Dot(dest_dir, dir[i]);
            dot_product.Add(temp);
        }
        while (dot_product.Count > 0)
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
    public void init()
    {
        Debug.Log("workermanager");
        worker_max = 3;
        worker_present = 2;
        List<GameObject> worker_list = new List<GameObject>();
        for (int i = 1; i <= worker_present; i++)
        {
            Create_Worker(player.player_number + "worker" + i);
        }
        worker_num = 3;
        worker_acc = 0;
        worker_increase = 50;
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}


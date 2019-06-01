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
            if (!obj.GetComponent<worker>().is_assigned)
                Update_Worker(obj, obj.GetComponent<worker>().cur_action, obj.GetComponent<worker>().destination);       
            Update_hp(obj);
            if (obj.GetComponent<worker>().cur_action == worker.Action.collect || obj.GetComponent<worker>().cur_action == worker.Action.dump)
            {
                //only take 1 turn to collect and dump
                obj.GetComponent<worker>().cur_action = worker.Action.idle;
            }            
            obj.GetComponent<worker>().is_assigned = false;
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
        int hp = obj.GetComponent<worker>().hp;
        if (obj.GetComponent<worker>().cur_action == worker.Action.idle)
        {
            hp = Math.Min(100, hp + 5);
        }
        hp = Math.Max(0, hp - 10 * obj.GetComponent<worker>().location.GetComponent<TileClass>().thresholdLevel());
        obj.GetComponent<worker>().hp = hp;
        if (obj.GetComponent<worker>().hp == 0)
            Kill_Worker(obj);
    }
    public void Kill_Worker(GameObject obj)
    {
        worker_list.Remove(obj);
        obj.GetComponent<worker>().location.GetComponent<TileClass>().tile_worker.Remove(obj);
        //remove from the building
        Destroy(obj);
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
        //selected.cur_queue = (worker.Action)Action;
        if (dest != selected.destination && dest !=selected.location)
        {
            selected.destination = dest;
        }
        //worker has been updated by the player
        selected.is_assigned = true;
        if (Action == worker.Action.abort)
        {
            //cancel worker action
            selected.cur_action = worker.Action.idle;
            selected.destination = selected.location;
        }
        //if not on same tile 
        else if (!(System.Object.ReferenceEquals(selected.location, dest)))
        {
            //if action was not moving, make it moving.
            if (Action == worker.Action.move)
            {
                //move worker to certain place.               
                StartCoroutine(move_action(selected));            
            }
        }
        else
        {
            //worker arrived to the destination
            if (Action == worker.Action.move)
            {
                selected.cur_action = worker.Action.idle;
                //selected.action_list = new string[1] { "abort" };
            }
            else if(Action == worker.Action.collect)
            {
                selected.cur_action = worker.Action.collect ;
                //StartCoroutine(collect_action(selected));
            }
            else if(Action == worker.Action.dump)
            {
                selected.cur_action = worker.Action.dump;
                //StartCoroutine(dump_action(selected));
            }
            else if (Action == worker.Action.work)
            {
                //produce resources
            }
            else if(Action == worker.Action.idle)
            {
                //restore worker hp
            }
        }
    }

    public IEnumerator move_worker(worker obj/*, GameObject dest*/)
    {
        //Vector3 workerPos = obj.worker_obj.transform.position;
        //Vector3 destination = dest.transform.position;
        //Vector3 direction = (destination - workerPos) / 10;
        TileClass destTile = null;
        yield return StartCoroutine(GameObject.Find("UI").GetComponent<clickHandler>().getDestTile(tile => destTile=tile,obj));
        if (destTile != null)
        {
            obj.destination = destTile.gameObject;
            StartCoroutine(move_action(obj));
            Debug.Log("Destination set : " + destTile);
        }
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
    public IEnumerator move_action(worker obj)
    {
        GameObject next = Calc_Path(obj.location, obj.destination);
        Debug.Log(next.name);
        StartCoroutine(move_animation(obj,next));
        obj.location.GetComponent<TileClass>().tile_worker.Remove(obj.gameObject);
        obj.location = next;
        obj.is_assigned = true;
        next.GetComponent<TileClass>().tile_worker.Add(obj.gameObject);
        yield return null;
    }
    public IEnumerator move_animation(worker obj,GameObject dest)
    {
        Vector3 workerPos = obj.gameObject.transform.position;
        Debug.Log(dest.name);
        Vector3 destination = dest.transform.position;
        Debug.Log(workerPos.x + ", " + workerPos.z);
        Debug.Log(destination.x + ", " + destination.z);
        Vector3 direction = (destination - workerPos) / 10;
        Debug.Log(direction.x + ", " + direction.z);
        for (int i = 0; i < 10; i++)
        {
            obj.gameObject.transform.position = obj.gameObject.transform.position + new Vector3(direction.x, 0, direction.z);
            yield return new WaitForSeconds(0.05f);
        }
        obj.worker_obj.transform.position = new Vector3(destination.x,workerPos.y,destination.z);
        yield return new WaitForSeconds(0.05f);
    }

    public GameObject Calc_Path(GameObject curr, GameObject dest)
    {
        Debug.Log("Calculating path");
        GameObject result = null;
        string fmt = "00";
        Debug.Log(curr.name);
        Debug.Log(curr.name.Substring(0, 2));
        int cur_x = Convert.ToInt32(curr.name.Substring(0, 2));
        Debug.Log(curr.name.Substring(2, 2));
        int cur_y = Convert.ToInt32(curr.name.Substring(2, 2));
        int dest_x = Convert.ToInt32(dest.name.Substring(0, 2));
        int dest_y = Convert.ToInt32(dest.name.Substring(2, 2));
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
        Debug.Log(dest_dir.x + ", " + dest_dir.y);
        Debug.Log(dot_product[0] + ", " + dot_product[1] + ", " + dot_product[2] + ", " + dot_product[3] + ", " + dot_product[4] + ", " + dot_product[5]);
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


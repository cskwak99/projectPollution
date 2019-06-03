using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class WorkerManager : MonoBehaviour
{
    public List<GameObject> worker_list;
    public GameObject worker_prefab;
    public int worker_max;
    //public int worker_present;
    public int worker_num;
    public PlayerStats player;
    public int worker_increase;
    public int worker_acc;
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
        Debug.Log("Turn Update Worker");
        int i = 0;
        List<int> killed = new List<int>();
        foreach (GameObject obj in new List<GameObject>(worker_list))
        {        
            Debug.Log(obj.name);
            Debug.Log(obj.GetComponent<worker>().is_assigned);
            Debug.Log(obj.GetComponent<worker>().cur_action);
            //if (!obj.GetComponent<worker>().is_assigned)
            Update_Worker(obj, obj.GetComponent<worker>().cur_action, obj.GetComponent<worker>().destination);
            if (Update_hp(obj) == -1)
            {
                killed.Add(i);
            }
            if (obj.GetComponent<worker>().cur_action == worker.Action.collect || obj.GetComponent<worker>().cur_action == worker.Action.dump)
            {
                //only take 1 turn to collect and dump
                obj.GetComponent<worker>().cur_action = worker.Action.idle;
            }            
            
            if(obj.GetComponent<worker>().cur_action == worker.Action.idle && obj.GetComponent<worker>().is_assigned == true)
                obj.GetComponent<worker>().is_assigned = false;
            i++;
        }
        for(int j = 0; j < killed.Count; j++)
        {
            GameObject worker_died = worker_list[killed[j]];
            worker_died.GetComponent<worker>().location.GetComponent<TileClass>().tile_worker.Remove(worker_died);
            //remove from the building
            worker_list.Remove(worker_died);
            Destroy(worker_died);
        }
        //condition for worker increase
        Debug.Log("addworker");
        if (worker_max > worker_num)
        {
            if (worker_acc >= 100)
            {
                worker_acc = 0;
                Debug.Log("new worker : "+player.player_number + "worker" + (worker_num + 1));
                Create_Worker(player.player_number + "worker" + (worker_num+1));
            }
            else
            {
                worker_acc += worker_increase;
            }
        }
    }
    public int Update_hp(GameObject obj)
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
        {
            return -1;
        }
        return 0;
    }
    /*
    public void Kill_Worker(GameObject obj)
    {
        worker_list.Remove(obj);
        obj.GetComponent<worker>().location.GetComponent<TileClass>().tile_worker.Remove(obj);
        //remove from the building
        Destroy(obj);
    }
    */
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
        //worker_present++;
        worker_num++;
        player.worker_present++;
        return new_worker;
    }
    //when selected by UI
    public void Update_Worker(GameObject obj, worker.Action Action, GameObject dest)
    {
        Debug.Log("Update worker");
        worker selected = obj.GetComponent<worker>();
        //assigned by UI, build somewhere, move somewhere, work somewhere, Abort something
        //selected.cur_queue = (worker.Action)Action;
        if (dest != selected.destination && dest !=selected.location)
        {
            selected.destination = dest;
        }
        //worker has been updated by the player
        if (Action == worker.Action.abort)
        {
            //cancel worker action
            if(selected.cur_action == worker.Action.work)
            {
                dismiss_worker(selected);
            }
            selected.cur_action = worker.Action.idle;
            selected.destination = selected.location;
            selected.is_assigned = true;
        }
        //if not on same tile 
        if (!(System.Object.ReferenceEquals(selected.location, dest)))
        {
            //if the player set a worker to move then keep move
            if (Action == worker.Action.move)
            {
                //move worker to certain place.    
                selected.is_assigned = true;
                StartCoroutine(move_action(selected));            
            }
        }
        else
        {
            //worker arrived to the destination
            if (Action == worker.Action.move)
            {
                Debug.Log("arrived");
                selected.cur_action = worker.Action.idle;
                selected.is_assigned = false;
                //selected.action_list = new string[1] { "abort" };
            }
            else if(Action == worker.Action.collect)
            {
                selected.is_assigned = true;
                selected.cur_action = worker.Action.idle ;
                collect_waste(selected);
            }
            else if(Action == worker.Action.dump)
            {
                selected.is_assigned = true;
                selected.cur_action = worker.Action.idle;
                dump_waste(selected);
            }
            else if (Action == worker.Action.work)
            {
                selected.is_assigned = true;
                selected.cur_action = worker.Action.work;
                work_worker(selected);                
            }
        }
    }
    public void collect_waste(worker obj)
    {
        if (obj.location.name.Substring(4) == "Dome_tile")
        {
            TurnManager turn_manager = GameObject.Find("TurnManager").GetComponent<TurnManager>();
            PlayerStats player;
            if (obj.location.name.Substring(0, 1) == "0")
            {
                player = turn_manager.player1.GetComponent<PlayerStats>();
            }
            else
                player = turn_manager.player2.GetComponent<PlayerStats>();
            if (player.resources.w<= (obj.capacity-obj.waste_on_worker))
            {
                obj.waste_on_worker += (int)player.resources.w;
                player.resources.w = (float)0;
            }
            else
            {
                obj.waste_on_worker = obj.capacity;
                player.resources.w -= (float)(obj.capacity-obj.waste_on_worker);
            }
            return;
        }
        Building BuildingOnTile = obj.location.GetComponent<TileClass>().GetComponentInChildren<Building>();
        if (BuildingOnTile != null)
        {
            if (BuildingOnTile.name == "Landfill(Clone)")
            {
                if (BuildingOnTile.nowWaste <= obj.capacity)
                {
                    obj.waste_on_worker = (int)BuildingOnTile.nowWaste;
                    BuildingOnTile.nowWaste = (float)0;
                }
                else
                {
                    obj.waste_on_worker = obj.capacity;
                    BuildingOnTile.nowWaste -= (float)obj.capacity;
                }
                return;
            }
        }
        if (obj.location.GetComponent<TileClass>().resources.w <= obj.capacity)
        {
            obj.waste_on_worker = (int)obj.location.GetComponent<TileClass>().resources.w;
            obj.location.GetComponent<TileClass>().resources.w = (float)0;
        }
        else
        {
            obj.waste_on_worker = obj.capacity;
            obj.location.GetComponent<TileClass>().resources.w -= (float)obj.capacity;
        }
    }
    public void dump_waste(worker obj)
    {
        if (obj.location.name.Substring(4) == "Dome_tile") {
            TurnManager turn_manager = GameObject.Find("TurnManager").GetComponent<TurnManager>();
            PlayerStats player;
            if (obj.location.name.Substring(0, 1) == "0")
            {
                player = turn_manager.player1.GetComponent<PlayerStats>();
            }
            else
                player = turn_manager.player2.GetComponent<PlayerStats>();
            player.resources.w += (float)obj.waste_on_worker;
            obj.waste_on_worker = 0;
            return;
        }
        Building BuildingOnTile = obj.location.GetComponent<TileClass>().GetComponentInChildren<Building>();
        if (BuildingOnTile != null)
        {
            if (BuildingOnTile.name == "Landfill(Clone)")
            {
                BuildingOnTile.nowWaste += (float)obj.waste_on_worker;
                obj.waste_on_worker = 0;
            }
            return;
        }
        obj.location.GetComponent<TileClass>().resources.w += (float)obj.waste_on_worker;
        obj.waste_on_worker = 0;
    }
    public void work_worker(worker obj)
    {
        Building buildingOnTile = obj.location.GetComponent<TileClass>().transform.GetComponentInChildren<Building>();
        buildingOnTile.assignedWorker = obj.gameObject;
    }
    public void dismiss_worker(worker obj)
    {
        Building buildingOnTile = obj.location.GetComponent<TileClass>().transform.GetComponentInChildren<Building>();
        buildingOnTile.assignedWorker = null;
    }
    public IEnumerator move_worker(worker obj/*, GameObject dest*/)
    {
        //Vector3 workerPos = obj.worker_obj.transform.position;
        //Vector3 destination = dest.transform.position;
        //Vector3 direction = (destination - workerPos) / 10;
        TileClass destTile = null;
        obj.cur_action = worker.Action.move;
        yield return StartCoroutine(GameObject.Find("UI").GetComponent<clickHandler>().getDestTile(tile => destTile = tile));
        if (destTile != null)
        {
            if (System.Object.ReferenceEquals(destTile, obj.location))
                yield break;
            if (destTile.name.Substring(4) == "Water_Tile")
            {
                if(destTile.transform.GetComponentInChildren<Building>().name.Substring(0, destTile.transform.GetComponentInChildren<Building>().name.Length-7) != "Waterpump")
                    yield break;
            }
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
        Debug.Log("move action "+next.name);
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
            if(obj!=null)
                obj.gameObject.transform.position = obj.gameObject.transform.position + new Vector3(direction.x, 0, direction.z);
            yield return new WaitForSeconds(0.05f);
        }
        if(obj!=null)
            obj.worker_obj.transform.position = new Vector3(destination.x,workerPos.y,destination.z);
        yield return new WaitForSeconds(0.05f);
    }

    public GameObject Calc_Path(GameObject curr, GameObject dest)
    {
        Debug.Log("Calculating path");
        GameObject result = null;
        string fmt = "00";
        Debug.Log(curr.name);
        //Debug.Log(curr.name.Substring(0, 2));
        int cur_x = Convert.ToInt32(curr.name.Substring(0, 2));
        //Debug.Log(curr.name.Substring(2, 2));
        int cur_y = Convert.ToInt32(curr.name.Substring(2, 2));
        int dest_x = Convert.ToInt32(dest.name.Substring(0, 2));
        int dest_y = Convert.ToInt32(dest.name.Substring(2, 2));
        Vector2 dest_dir = new Vector2((float)(dest.transform.position.x - curr.transform.position.x), (float)(dest.transform.position.z - curr.transform.position.z));
        Vector2[] dir = new Vector2[6] { new Vector2(1.0f, 0.0f), new Vector2(0.5f, (float)Math.Sqrt(3) * 0.5f), new Vector2(-0.5f, (float)Math.Sqrt(3) * 0.5f), new Vector2(-1.0f, 0.0f), new Vector2(-0.5f, -1.0f * (float)Math.Sqrt(3) * 0.5f), new Vector2(0.5f, -1.0f * (float)Math.Sqrt(3) * 0.5f) };
        List<float> dot_product = new List<float>();
        Vector2Int[] tile_dir;
        if (cur_y % 2 == 0)
        {
            tile_dir = new Vector2Int[6] { new Vector2Int(-1, 0), new Vector2Int(0, +1), new Vector2Int(1, +1), new Vector2Int(+1, 0), new Vector2Int(1, -1), new Vector2Int(0, -1) };
        }
        else
        {
            tile_dir = new Vector2Int[6] { new Vector2Int(-1, 0), new Vector2Int(-1, +1), new Vector2Int(0, +1), new Vector2Int(+1, 0), new Vector2Int(0, -1), new Vector2Int(-1, -1) };
        }
        float temp;

        float min = 0;
        int min_idx = 0;
        for (int i = 0; i < 6; i++)
        {
            temp = (float)Math.Acos((double)(dest_dir.x * dir[i].x + dest_dir.y * dir[i].y)/Math.Sqrt(dest_dir.x*dest_dir.x+dest_dir.y*dest_dir.y));
            dot_product.Add(temp);
        }
        //Debug.Log(dest_dir.x + ", " + dest_dir.y);
        //Debug.Log(dot_product[0] + ", " + dot_product[1] + ", " + dot_product[2] + ", " + dot_product[3] + ", " + dot_product[4] + ", " + dot_product[5]);
        while (dot_product.Count > 0)
        {
            min = (float) Double.PositiveInfinity;
            min_idx = 0;
            for (int i = 0; i < dot_product.Count; i++)
            {
                if (min >= dot_product[i])
                {
                    min = dot_product[i];
                    min_idx = i;
                }
            }
            string next_x = (cur_x + tile_dir[min_idx].x).ToString(fmt);
            string next_y = (cur_y + tile_dir[min_idx].y).ToString(fmt);
            result = GameObject.FindGameObjectsWithTag(next_x + next_y)[0];
            Debug.Log(result.name);
            if (result.name.Substring(4).Equals("Water_tile"))
            {
                if(dest.GetComponent<TileClass>().transform.GetComponentInChildren<Building>()==null)
                //if (dest.GetComponent<TileClass>().transform.GetComponentInChildren<Building>().name.Substring(0, dest.GetComponent<TileClass>().transform.GetComponentInChildren<Building>().name.Length - 7) != "Waterpump")
                {
                    dot_product.RemoveAt(min_idx);
                    Debug.Log("result was water tile");
                    continue;
                }
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
        worker_num =0;
        List<GameObject> worker_list = new List<GameObject>();
        for (int i = 1; i <= 2; i++)
        {
            Create_Worker(player.player_number + "worker" + i);
        }
        //worker_num = 3;
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


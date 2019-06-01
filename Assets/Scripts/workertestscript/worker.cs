using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class worker : MonoBehaviour
{
    public PlayerStats player;
    public int hp;
    public int support;
    public GameObject location;
    public GameObject destination;
    public GameObject worker_obj;
    public string worker_name;
    public string[] action_list;
    public int turn_left;
    public Action cur_action;
    public Action cur_queue;
    // if worker has been updated by player
    public bool is_updated = false;
    // if worker has been assigned by player
    public bool is_assigned = false;
    public int capacity;
    public int waste_on_worker;
    public enum Action
    {
        idle,
        move,
        collect,
        dump,
        work,
        abort
    }
    // Start is called before the first frame update
    public void init_worker(PlayerStats player, string name, GameObject worker_prefab)
    {
        this.player = player;
        worker_name = name;
        hp = 100;
        support = 100;
        capacity = 100;
        location = this.transform.parent.gameObject;
        cur_action = Action.idle;
        turn_left = 0;
        is_updated = false;
        is_assigned = false;
        action_list = new string[3] { "idle", "move", "collect" };
        waste_on_worker = 0;
        destination = this.transform.parent.gameObject;
        this.gameObject.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
        this.gameObject.transform.position = location.transform.position + new Vector3(0f, 0.05f, 0f);
        Debug.Log("new worker");

    }
    public string[] get_action()
    {
        List<string> result = new List<string>();
        if(cur_action == worker.Action.move || cur_action == worker.Action.work)
        {
            result.Add("abort");
            return result.ToArray();
        }
        if (waste_on_worker < capacity)
        {
            result.Add("collect");
        }
        if (waste_on_worker > 0)
        {
            result.Add("dump");
        }
        else if (location.GetComponent<TileClass>().transform.GetComponentInChildren<Building>() != null)
        {
            result.Add("work");
        }
        return result.ToArray();
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (is_move)
        {
            //StartCoroutine();
            is_move = false;
        }
        */
    }
}

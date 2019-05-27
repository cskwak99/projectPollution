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
    public bool is_updated = false;
    public enum Action
    {
        idle,
        moving,
        constructing,
        working,
        aborting
    }
    // Start is called before the first frame update
    public void init_worker(PlayerStats player, string name, GameObject worker_prefab)
    {
        this.player = player;
        worker_name = name;
        hp = 100;
        support = 100;
        location = this.transform.parent.gameObject;
        cur_action = Action.idle;
        turn_left = 0;
        is_updated = false;
        action_list = new string[4] { "idle", "move", "build", "work" };
        destination = this.transform.parent.gameObject;
        this.gameObject.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
        this.gameObject.transform.position = location.transform.position + new Vector3(0f, 0.05f, 0f);
        Debug.Log("new worker");

    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}

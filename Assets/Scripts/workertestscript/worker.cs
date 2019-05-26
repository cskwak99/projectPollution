using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class worker : MonoBehaviour
{
    public PlayerStats player;
    public int hp;
    public int support;
    public TileClass location;
    public GameObject worker_asset;
    public GameObject worker_obj;
    public string worker_name;
    public int turn_left;
    public Action cur_action;
    public enum Action
    {
        idle,
        moving,
        constructing,
        working,
        aborting
    }
    public worker() { }
    // Start is called before the first frame update
    public worker(PlayerStats player,string name)
    {
        this.player = player;
        worker_name = name;
        hp = 100;
        support = 100;
        location = player.dome;
        cur_action = Action.idle;
        turn_left = 0;
    }
    void Start()
    {
        worker_obj = (GameObject)Instantiate(worker_asset, location.transform.position+new Vector3(0f,0.05f,0f), Quaternion.identity);
        worker_obj.name = this.name;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

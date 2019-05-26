using System.Collections;
using System.Collections.Generic;
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
            worker_list.add(new worker(player,"worker"+"i"));
        }
    }
    //call by ui or something else
    //action can be move, assignment, abort
    //if you click on worker menu, you select action first and then select destination tile
    //information can be sent to the workermanager.
    public int Update_Worker(string name,int action,TileClass dest)
    {
        foreach(worker in worker_list)
        {
            if(worker.name == name)
            {

            }
        }
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

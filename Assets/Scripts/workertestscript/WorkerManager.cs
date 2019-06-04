using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class WorkerManager : MonoBehaviour
{
    public GameObject worker_prefab;
    public int worker_max;
    //public int worker_present;
    // Start is called before the first frame update
    Boolean is_updated;
    //call by ui or something else
    //action can be move, assignment, abort
    //if you click on worker menu, you select action first and then select destination tile
    //information can be sent to the workermanager.
    //Action can be only work, build, move.
    //End of the each turn, update the workers
    //Update worker by ui inputs
    public GameObject Create_Worker(string name, PlayerStats player)
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
        player.workers.Add(new_worker);
        player.worker_present++;
        return new_worker;
    }
}


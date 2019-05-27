using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class TileClass : MonoBehaviour
{
    // Start is called before the first frame update
    public string tileDescription;
    public float polluAmount = 0;
    public float maxPolluAmount = 100;
    public float thresholdSafe = 30;
    public float thresholdKill = 60;
    public float thresholdDeadLand = 90;
    public List<GameObject> tile_worker = new List<GameObject>();
    public Vector4 resources = new Vector4(); // water, food, metal, waste
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void UpdatePolluAmount ()
    {
        polluAmount = resources.w/10;
    }

    public void AddWaste(float waste)
    {
        resources.w = waste;
    }

    public Vector4 getResources(Vector4 resourcesTaken)
    {
        Debug.Log(resources);
        Vector4 resourcesTrulyTaken = Vector4.Min(resourcesTaken, resources);
        resources = resources - resourcesTrulyTaken;
        return resourcesTrulyTaken;
    }

    public int thresholdLevel()
    {
        int thresholdLvl = 0;
        return thresholdLvl = ((polluAmount >= thresholdSafe) ? 1 : 0) + ((polluAmount >= thresholdKill) ? 1 : 0) + ((polluAmount >= thresholdDeadLand) ? 1 : 0);
    }

    public virtual string[] getBuildable()
    {
        string[] buildable = { "" };
        return buildable;
    }
    public string[] getWorker()
    {
        string[] result = new string[tile_worker.Count];
        int i = 0;
        foreach (GameObject worker in tile_worker)
        {
            result[i] = worker.GetComponent<worker>().worker_name;
            i++;
        }
        return result;
    }

}


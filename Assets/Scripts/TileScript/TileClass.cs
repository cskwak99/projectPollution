using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TileClass : MonoBehaviour
{
    // Start is called before the first frame update
    public string tileDescription;
    public float polluAmount = 0;
    public float maxPolluAmount = 100;
    public float thresholdSafe = 30;
    public float thresholdKill = 60;
    public float thresholdDeadLand = 90;
    public Vector4 resources = new Vector4(); // food, water, metal, waste
    public TileClass[] adjacent;
    void Start()
    {
        tileDescription = "A Description";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdatePolluAmount (float pollu)
    {
        polluAmount = resources.w/10;
    }

    public void AddWaste(float waste)
    {
        resources.w = waste;
    }

    public Vector4 getResources(Vector4 resourcesTaken)
    {
        Vector4 resourcesTrulyTaken = Vector4.Min(resourcesTaken, resources);
        resources = resources - resourcesTrulyTaken;
        return resourcesTrulyTaken;
    }

    public int thresholdLevel()
    {
        int thresholdLvl = 0;
        return thresholdLvl = ((polluAmount >= thresholdSafe) ? 1 : 0) + ((polluAmount >= thresholdKill) ? 1 : 0) + ((polluAmount >= thresholdDeadLand) ? 1 : 0);
    }

    virtual public string[] getBuildable()
    {
        string[] buildable = { "SA" };
        return buildable;
    }

}


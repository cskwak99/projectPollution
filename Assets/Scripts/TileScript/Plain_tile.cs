using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plain_tile : TileClass
{

    // Start is called before the first frame update
    void Start()
    {
        resources = new Vector4(0,float.PositiveInfinity, 0, 0); //initiate resources, second place is food
        tileDescription = "A plain, useful to build something on.";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override string[] getBuildable()
    {
        string[] buildable = { "Residential", "Farm", "Landfill" };
        return buildable;
    }
}

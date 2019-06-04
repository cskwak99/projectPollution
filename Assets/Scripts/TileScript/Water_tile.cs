using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water_tile : TileClass
{

    Water_tile previousWater;
    Water_tile nextWater;

    // Start is called before the first frame update
    void Start()
    {
        resources = new Vector4(float.PositiveInfinity,0, 0, 0); //initiate resources, first place is water
        tileDescription = "A river, source of life";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override string[] getBuildable()
    {
        string[] buildable = { "Water Pump", "Landfill"};
        return buildable;
    }
}

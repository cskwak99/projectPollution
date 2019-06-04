using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine_tile : TileClass
{

    // Start is called before the first frame update
    void Start()
    {
        resources = new Vector4(0, 0, 100, 0); //initiate resources, third place is metal
        tileDescription = "Some kind of metal deposit, useful for building and research";
    }

    public override string[] getBuildable()
    {
        string[] buildable = { "Mine", "Residential", "Factory" };
        return buildable;
    }
}

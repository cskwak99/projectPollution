using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ruin_tile : TileClass
{

    // Start is called before the first frame update
    void Start()
    {
        resources = new Vector4(Random.Range(25, 100), Random.Range(25, 100), Random.Range(25, 100), Random.Range(25, 100)); //initiate resources
        tileDescription = "Ruin of the ancient civilisation. They contain all kind of resources";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override string[] getBuildable()
    {
        string[] buildable = { "" };
        return buildable;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlainTileClass : TileClass
{

    // Start is called before the first frame update
    void Start()
    {
        resources = new Vector4(float.PositiveInfinity, 0, 0, 0); //initiate resources, first place is food
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

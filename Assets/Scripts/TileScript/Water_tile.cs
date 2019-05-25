using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water_tile : TileClass
{

    // Start is called before the first frame update
    void Start()
    {
        resources = new Vector4(0, float.PositiveInfinity, 0, 0); //initiate resources, second place is water
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

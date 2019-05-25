using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plain_tile : TileClass
{
    // Start is called before the first frame update
    Vector4 resources = new Vector4();
    void Start()
    {
        this.tileDescription = "Plain tile that can be polluted easily";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

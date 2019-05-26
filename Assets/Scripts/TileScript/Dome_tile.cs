using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dome_tile : TileClass
{

    int antiVaxxerPop;
    int maxAntiVaxxerPop;

    // Start is called before the first frame update
    void Start()
    {
        tileDescription = "Headquarter of the mighty antivaxxer ! They have generously taken upon them the curse of staying in the dome all their life to avoid autism and be intellectually superior";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    new public string[] getBuildable()
    {
        string[] buildable = { "" };
        return buidable;
    }
}

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
        tileDescription = "Headquarter of the mighty antivaxxer !\n " +
            "They have generously taken upon them the curse of staying in the dome all their life\n\n" + //to avoid autism and be intellectually superior\n" +
            "Spawn worker here every turn if workers number does not max out\n" +
            "You can not spawn if the worker is on the tile!\n"+
            "Height: "+h;
        resources = new Vector4(0,0,0,0);
    }

    public override string[] getBuildable()
    {
        string[] buildable = { };
        return buildable;
    }
}

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
        resources = new Vector4(0, float.PositiveInfinity, 0, 0); //initiate resources, second place is water
        tileDescription = "A river, source of life";
        string[] waterFlow = { "0203", "0204", "0304", "0404", "0504", "0604", "0704", "0805", "0905", "1005", "1105", "1205" };
        string name = this.gameObject.name;
        string coordinate = name.Substring(0, 4);
        string x = coordinate.Substring(0, 2);
        string y = coordinate.Substring(2, 2);
        if (y.Equals("01"))
        {
            if (x.Equals("12"))
            {
                previousWater = null;
                nextWater = (Water_tile) GameObject.Find("1301Water_tile").GetComponent("Water_tile");
            }
            else
            {
                previousWater = (Water_tile) GameObject.Find("1201Water_tile").GetComponent("Water_tile"); ;
                nextWater = null;
            }
        }
        else if (y.Equals("O7"))
        {
            if (x.Equals("01"))
            {
                previousWater = null;
                nextWater = (Water_tile) GameObject.Find("0207Water_tile").GetComponent("Water_tile"); ;
            }
            else
            {
                previousWater = (Water_tile) GameObject.Find("0107Water_tile").GetComponent("Water_tile"); ;
                nextWater = null;
            }
        }
        else
        {
            int index = System.Array.IndexOf(waterFlow, coordinate);
            if (index == 0)
            {
                previousWater = null;
                nextWater = (Water_tile) GameObject.Find(waterFlow[index + 1] + "Water_tile").GetComponent("Water_tile"); ;
            }
            else if (index == waterFlow.Length)
            {
                previousWater = (Water_tile) GameObject.Find(waterFlow[index - 1] + "Water_tile").GetComponent("Water_tile"); ;
                nextWater = null;
            }
            else
            {
                previousWater = (Water_tile) GameObject.Find(waterFlow[index - 1] + "Water_tile").GetComponent("Water_tile"); ;
                nextWater = (Water_tile) GameObject.Find(waterFlow[index + 1] + "Water_tile").GetComponent("Water_tile"); ;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void UpdatePolluAmount(float pollu)
    {
        polluAmount = resources.w / 5;
    }

    public void UpdateWasteFlow()
    {
        if (nextWater != null)
        {
            float wasteFlowing = Mathf.Min(resources.w, 15);
            nextWater.AddWaste(wasteFlowing);
            resources.w -= wasteFlowing;
        }
        else
        {
            float wasteFlowing = Mathf.Min(resources.w, 15);
            resources.w -= wasteFlowing;
        }
        if (previousWater != null)
        {
            resources += previousWater.getResources(new Vector4(0, 0, 0, 15));
        }
    }

    public override string[] getBuildable()
    {
        string[] buildable = { "Water Pump" };
        return buildable;
    }
}

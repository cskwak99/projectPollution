using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building
{
    public int pollution_rate;
    public Vector4 required_resource;
    public Vector4 product_resource;
    public int player_number;

    public void Update_pollution_rate(int value)
    {
        pollution_rate = pollution_rate + value;
        if (pollution_rate > 100)
        {
            pollution_rate = 100;
        }
    }

    public void Update_produce(Vector4 vec4)
    {
        product_resource = product_resource + vec4;
    }

    
}

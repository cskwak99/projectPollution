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
        tileDescription =
            "Water, source of life, source of faraway wastes. Pollution dumped will flow to all tiles\n\n" +
            "Can build: Water Pump\n"+
            "Height: "+h;
    }

    public override bool[,] spread_pollution()  //Return a speaded tile map.
    {
        generateTileMap GT = GameObject.Find("Hexagon_Map").GetComponent<generateTileMap>();
        List<TileClass> adj = new List<TileClass>(); //List of adjacent water tiles.
        bool[,] isChecked = new bool[GT.mapWidth,GT.mapHeight];
        Stack<TileClass> stack = new Stack<TileClass>();
        TileClass t;
        List<TileClass> nb;
        float sumPollu = 0;
        stack.Push(this);
        while (stack.Count > 0)  //Breadth-First through all adjacent tiles to get adjacent water tile.
        {
            t = stack.Pop();
            sumPollu += t.polluAmount;
            nb = t.getNeighbor();
            foreach (TileClass n in nb)
            {
                if (isChecked[n.x,n.y] == false && n.h<=t.h)
                {
                    if(n.tileType == "Water_tile")
                    {
                        isChecked[n.x, n.y] = true;
                        adj.Add(n);
                        stack.Push(n);
                    }
                }
            }
        }
        int num_adj = adj.Count; //Number of all adjacent water tiles.
        foreach (TileClass wt in adj)   //For all water tiles adjacent to each other, update pollution
        {
            wt.UpdatePolluAmount(sumPollu / num_adj);   //Equally spread pollution to all adjacent water tiles.
        }
        return isChecked;
    }
    public override string[] getBuildable()
    {
        string[] buildable = { "Water Pump"};
        return buildable;
    }
}

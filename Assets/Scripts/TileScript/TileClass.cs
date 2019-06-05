using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;
public class TileClass : MonoBehaviour
{
    // Start is called before the first frame update
    public int x;
    public int y;
    public Vector3 cubeCoor;
    public int h;
    public string tileDescription;
    public string tileType;
    public float polluAmount = 0;
    public float maxPolluAmount = 100;
    public float thresholdShut = 60;
    public float thresholdDeadLand = 100;
    public int thresholdLvl = 0;
    public Vector4 resources = new Vector4(); // water, food, metal, waste
    public Texture[] textures;

    public void changeModel()
    {
        Renderer rend = this.GetComponent<Renderer>();
        rend.material.mainTexture = textures[thresholdLvl];
    }
    public int calcDist(TileClass destTile)
    {
        int x1, y1, x2, y2;
        x1 = x;
        y1 = y;
        y2 = destTile.y;
        x2 = destTile.x;
        Vector3 cube1 = oddr_to_cube(y1, x1);
        Vector3 cube2 = oddr_to_cube(y2, x2);
        return cube_distance(cube1, cube2);
    }
    private int cube_distance(Vector3 a, Vector3 b)
    {
        return (int)(Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y) + Mathf.Abs(a.z - b.z)) / 2;
    }
    public Vector3 oddr_to_cube(int row, int col)
    {
        int x = col - (row - (row & 1)) / 2;
        int z = row;
        int y = -x - z;
        return new Vector3(x, y, z);
    }
    public List<TileClass> getNeighbor()
    {
        List<TileClass> tiles = new List<TileClass>();
        generateTileMap GT = GameObject.Find("Hexagon_Map").GetComponent<generateTileMap>();
        GameObject[,] tileMap = GT.TileMap;
        int[,,] oddr_directions =
        {
            { 
                { +1, 0 }, {0, -1 }, {+1, +1 },
                { -1, 0 }, {-1, +1 }, {0, +1 }
            },
            {
                { +1,  0 }, {+1, -1 }, { 0, -1},
                { -1,  0 }, { 0, +1 }, { -1, -1}
            }
        };
        int parity = (y+1) % 2;
        for(int i = 0;i < 6; i++)
        {
            if(0 <= x + oddr_directions[parity, i, 1] && x + oddr_directions[parity, i, 1] < GT.mapWidth)
                if (0 <= y + oddr_directions[parity, i, 0] && y + oddr_directions[parity, i, 0] < GT.mapHeight)
                    tiles.Add(tileMap[x+oddr_directions[parity, i, 1], y+oddr_directions[parity, i, 0]].GetComponent<TileClass>());
        }
        return tiles;
    }
    public virtual bool[,] spread_pollution()  ///ABSTRACTED FOR WATER TILE
    {
        return new bool[1,1];
    }
    
    public virtual void UpdatePolluAmount (float amount)
    {
        //polluAmount = resources.w/10;
        polluAmount = Mathf.Clamp(amount, 0, maxPolluAmount);
        UpdateThresholdLevel();
    }

    public Vector4 getResources(Vector4 resourcesTaken)
    {
        //Debug.Log(resources);
        Vector4 resourcesTrulyTaken = Vector4.Min(resourcesTaken, resources);
        resources = resources - resourcesTrulyTaken;
        return resourcesTrulyTaken;
    }

    public void UpdateThresholdLevel()
    {
        thresholdLvl = ((polluAmount >= thresholdShut) ? 1 : 0) + ((polluAmount >= thresholdDeadLand) ? 1 : 0);
    }
    public bool isWorkerOn()
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.GetComponent<worker>() != null)
            {
                return true;
            }
        }
        return false;
    }
    public bool isPlayerWorkerOn(PlayerStats player)
    {
        worker w;
        foreach (Transform child in transform)
        {
            w = child.gameObject.GetComponent<worker>();
            if (w != null)
            {
                if (w.player.player_number == player.player_number)
                    return true;
                else
                    return false;
            }
        }
        return false;
    }
    public bool isBuildingOn()
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.GetComponent<Building>() != null)
            {
                return true;
            }
        }
        return false;
    }
    public bool isPlayerBuildingOn(PlayerStats player)
    {
        Building b;
        foreach (Transform child in transform)
        {
            b = child.gameObject.GetComponent<Building>();
            if (b != null)
            {
                if (b.playerOccupied.GetComponent<PlayerStats>().player_number == player.player_number)
                    return true;
                else
                    return false;
            }
        }
        return false;
    }
    public virtual string[] getBuildable()
    {
        string[] buildable = { "ASD" };
        return buildable;
    }

    public GameObject getWorker()
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.GetComponent<worker>() != null)
            {
                return child.gameObject;
            }
        }
        return null;
    }
    public Building getBuilding()
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.GetComponent<Building>() != null)
            {
                return child.gameObject.GetComponent<Building>();
            }
        }
        return null;
    }

}


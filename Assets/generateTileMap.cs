using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class generateTileMap : MonoBehaviour
{
    // Start is called before the first frame update
    public int mapHeight;
    public int mapWidth;
    public float tileXOffset;
    public float tileZOffset;
    public float tileYOffset;
    public GameObject plainPrefab;
    public GameObject waterPrefab;
    public GameObject domePrefab;
    public GameObject minePrefab;
    public GameObject[,] TileMap;
    t[,] typeMap;
    int[,] heightMap;
    b[,] buildMap;
    enum t
    {
        P, //Plain
        W, //Water
        M, //Mine
        D, //Dome
    }
    enum b
    {
        H1, //Home for p1
        H2, //Home for p2
        d1, //dome for p1
        d2, //dome for p2
        F, //Farm
        L, //Landfill
        N, //None
    }
    void Start()
    {
        t P = t.P;
        t W = t.W;
        t M = t.M;
        t D = t.D;
        b H1 = b.H1;
        b H2 = b.H2;
        b F = b.F;
        b d1 = b.d1;
        b d2 = b.d2;
        b L = b.L;
        b N = b.N;
        t[,] typeMap = new t[,] // P = plain tile, W = water, M = mine, D = dome
            { {M, M, M, M, M, P, P, W, W, P, P, P},
              {  M, D, M, M, P, M, P, W, W, W, P, P},
              {M, M, M, M, P, M, P, P, P, W, W, W},
              {  W, W, M, P, P, P, P, P, P, W, W, W},
              {P, W, P, P, P, P, P, P, P, P, W, W},
              {  P, W, W, P, P, P, P, P, P, P, P, P},
              {W, P, W, W, P, P, P, P, P, P, D, P},
              {  P, P, W, W, W, W, W, P, P, P, P, P},
            };
        int[,] heightMap = new int[,] //0 = Ground level
            { {4, 3, 3, 2, 2, 2, 2, 2, 1, 0, 0, 0},
              {  3, 3, 3, 2, 1, 2, 2, 1, 1, 0, 0, 1},
              {2, 2, 2, 2, 1, 1, 1, 1, 0, 0, 0, 0},
              {  1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0},
              {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
              {  1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
              {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
              {  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            };
        b[,] buildMap = new b[,]  //N = None, d1 = dome for player 1, H1 = residential for player 1
            { {N, H2, N, N, N, N, N, N, N, N, N, N},
              {  N, d2,N, N, N, N, N, N, N, N, N, N},
              {N, N, N, N, N, N, N, N, N, N, N, N},
              {  N, N, N, N, N, N, N, N, N, N, N, N},
              {N, N, N, N, N, N, N, N, N, N, N, N},
              {  N, N, N, N, N, N, N, N, N, N, N, N},
              {N, N, N, N, N, N, N, N, N, N, d1, N},
              {  N, N, N, N, N, N, N, N, N, N, N, H1},
            };
        TileMap = new GameObject[mapWidth, mapHeight];
        
        for (int i = 0; i < mapWidth; i++)
        {
            for(int j = 0; j < mapHeight; j++)
            {
                GameObject TempTile = getTilePrefab(typeMap[j,i], j.ToString(),i.ToString());
                TempTile.GetComponent<TileClass>().x = i;
                TempTile.GetComponent<TileClass>().y = j;
                TempTile.GetComponent<TileClass>().h = heightMap[j,i];
                TempTile.GetComponent<TileClass>().cubeCoor = TempTile.GetComponent<TileClass>().oddr_to_cube(j,i);
                TempTile.GetComponent<TileClass>().tileType = TempTile.name.Substring(4, TempTile.name.Length-4);
                if (j % 2 == 0)
                {
                    TempTile.transform.position = new Vector3(i * tileXOffset, heightMap[j,i]*tileYOffset, j * tileZOffset);
                }
                else
                {
                    TempTile.transform.position = new Vector3(i * tileXOffset + tileXOffset/2, heightMap[j, i] * tileYOffset, j * tileZOffset);
                }
                TileMap[i, j] = TempTile;
                plantBuilding(buildMap[j,i],TempTile.GetComponent<TileClass>());
            }
        }
    }
    private void plantBuilding(b building,TileClass atTile)
    {
        BuildManager BM = GameObject.Find("_BuildManager").GetComponent<BuildManager>();
        TurnManager TM = GameObject.Find("TurnManager").GetComponent<TurnManager>(); 
        if (building == b.d1)
        {
            TM.player1.GetComponent<PlayerStats>().dome_tile = atTile.gameObject;
        }
        else if (building == b.d2)
        {
            TM.player2.GetComponent<PlayerStats>().dome_tile = atTile.gameObject;
        }
        else if (building == b.H1)
        {
            BM.Init_Residential(atTile, TM.player1);
        }
        else if (building == b.H2)
        {
            BM.Init_Residential(atTile, TM.player2);
        }

    }
    private GameObject getTilePrefab(t at, string i, string j)
    {
        GameObject temp;
        if (i.Length == 1)
            i = "0" + i;
        if (j.Length == 1)
            j = "0" + j;
        if (at == t.P)
        {
            temp = Instantiate(plainPrefab, transform);
            temp.name = i + j + "Plain_tile";
        }
        else if (at == t.D)
        {
            temp = Instantiate(domePrefab, transform);
            temp.name = i + j + "Dome_tile";
        }
        else if (at == t.M)
        {
            temp = Instantiate(minePrefab, transform);
            temp.name = i + j + "Mine_tile";
        }  
        else if (at == t.W)
        {
            temp = Instantiate(waterPrefab, transform);
            temp.name = i + j + "Water_tile";
        }
        else
        {
            temp = null;
        }
        return temp;
    }
    // Update is called once per frame
}

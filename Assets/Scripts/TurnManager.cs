using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player1;
    public GameObject player2;
    public GameObject current_player;
    public int totalWaste;
    public int turnNum;
    private UIManager UIM;
    private WorkerManager WM;
    private GameObject currentArrow;
    void Start()
    {
        Debug.Log("GameStart");
        UIM = GameObject.Find("UI").GetComponent<UIManager>();
        WM = GameObject.Find("WorkerManager").GetComponent<WorkerManager>();
        player1 = GameObject.Find("Player1");
        player2 = GameObject.Find("Player2");
        current_player = player1;
        turnNum = 1;
        WM.Create_Worker("P1worker1",player1.GetComponent<PlayerStats>());
        WM.Create_Worker("P2worker1",player2.GetComponent<PlayerStats>());
        currentArrow = Instantiate(new GameObject(), UIM.transform);
        currentArrow.transform.position = player1.GetComponent<PlayerStats>().dome_tile.transform.position + new Vector3(0, 0.4f, 0); ;
        GameObject arrow = Instantiate(UIM.arrowP1Prefab, currentArrow.transform);
    }

    public void Swap_player()
    {
        if (current_player == player1)
        {
            current_player = player2;

            Destroy(currentArrow);
            currentArrow = Instantiate(new GameObject(), UIM.transform);
            currentArrow.transform.position = player2.GetComponent<PlayerStats>().dome_tile.transform.position + new Vector3(0, 0.5f, 0);
            GameObject arrow = Instantiate(UIM.arrowP2Prefab, currentArrow.transform);
            

        }
        else
        {
            current_player = player1;

            Destroy(currentArrow);
            currentArrow = Instantiate(new GameObject(), UIM.transform);
            currentArrow.transform.position = player1.GetComponent<PlayerStats>().dome_tile.transform.position + new Vector3(0, 0.5f, 0); ;
            GameObject arrow = Instantiate(UIM.arrowP1Prefab, currentArrow.transform);
            
        }
        turnNum+=1;
        Debug.Log("turn:" + turnNum);
        initiateTurn();
    }

    public GameObject Get_current_player()
    {
        return current_player;
    }

    public void initiateTurn(){
        //Updating waste flow every turn 
        /*
        if (turnNum%2 == 1)
        {
            string[] waterFlow = { "0107", "0207", "0203", "0204", "0304", "0404", "0504", "0604", "0704", "0805", "0905", "1005", "1105", "1205", "1201", "1301" };
            foreach (string coordinate in waterFlow)
            {
                Water_tile tile = (Water_tile) GameObject.FindWithTag(coordinate).GetComponent("Water_tile");
                tile.UpdateWasteFlow();
            }
        }
          //Calculate resource per turn
        //Resource Gathering Phase
          //Check gather buildings and get resources from it
          //check tile that has player's building and get waste from it
          //waste saves on the landfill, but if it is over capacity, save waste on the dome tile
        //Worker Check Phase
          //Check the number of player's Residential area in the map
          //Current worker < max worker -> pop up new worker on the  one of residential area
        //Consuming Resource Phase
          // Consume water and food proportional to number of worker + fixed amount from anti vaxxer
          // Not enough -> kill some of anti vaxxer, decrease every anti vaxxer's efficiency
        //Pollution Check Phase
          //Calculate pollution on the tiles
          foreach (Transform child in GameObject.Find("Hexagon_Map").transform)
        {
            TileClass tile = child.GetComponent<TileClass>();
            tile.UpdatePolluAmount();
        }
          //Calculate worker efficiency
          //Pollution meter high in dome tile -> kill anti vaxxer
        //Support rate Check Phase
          //increase it fixed rate
          //Calculate decreased support rate proportional to worker efficiency and current/max number
        //Check game end Phase
          //support rate & anti vaxxer -> 0 -> game end
        //random events
        //player do action
        //turn end
        */
        ResourceGatheringPhase();
        WorkerPhase();
        if(turnNum != 2) ConsumePhase();
        CheckLosePhase();
        randomEvents();
    }
    public void Turn_end()
    {
        print("End Turn");
        PollutionPhase();
        TilesPhase();
        Swap_player();
    }
    public void TilesPhase()
    {
        print("TilePhase");
        generateTileMap GM = GameObject.Find("Hexagon_Map").GetComponent<generateTileMap>();
        bool[,] spreadMap = new bool[GM.mapWidth, GM.mapHeight];
        ////Spread water tile pollution////
        foreach (Transform child in GameObject.Find("Hexagon_Map").transform)
        {
            TileClass tile = child.GetComponent<TileClass>();
            if(tile.tileType == "Water_tile" && spreadMap[tile.x,tile.y]==false)
            {
                bool[,] sp = tile.spread_pollution();
                for(int i = 0;i< GM.mapWidth;i++)
                {
                    for (int j = 0; j < GM.mapHeight; j++)
                        spreadMap[i, j] = spreadMap[i, j] || sp[i, j];
                }
            }
        }
        ////CHECK TILE CONDITION/////
        foreach (Transform child in GameObject.Find("Hexagon_Map").transform)
        {
            TileClass tile = child.GetComponent<TileClass>();
            tile.changeModel();
            if(tile.thresholdLvl == 2)
            {

            }
        }
    }
    public void ResourceGatheringPhase()
    {
        Debug.Log("Gather Resource");
        PlayerStats player = current_player.GetComponent<PlayerStats>();
        List<GameObject> buildings = player.buildings;

        //Gather reources
        Vector4 resources = new Vector4(0,0,0,0); 
        foreach (GameObject building in buildings)
        {
            Vector4 gathered = building.GetComponent<Building>().getResources();
            resources += gathered;
        }
        player.resources += resources;
    }

    public void WorkerPhase(){
        WorkerManager WM = GameObject.Find("WorkerManager").GetComponent<WorkerManager>();
        PlayerStats player = current_player.GetComponent<PlayerStats>();
        Debug.Log(player.player_number + ": workerphase");  
        int maxWorker = player.updateWorkerMax();
        int currentWorker = player.worker_present;
        //Current worker < max worker -> pop up new worker on the one of residential area
        if (currentWorker<maxWorker)
        {
            print(currentWorker + " " + maxWorker);
            if (!player.dome_tile.GetComponent<TileClass>().isWorkerOn())
                WM.Create_Worker("worker", player);
            else
                UIM.showPopup("Worker can't spawn!, get the one out of dome");
        }
        ///Reseting AP for all workers
        foreach(GameObject w in player.workers)
        {
            w.GetComponent<worker>().actionLeft = player.worker_ap;
        }
    }

    public void ConsumePhase(){
        Debug.Log("Consume Phase");
        PlayerStats player = current_player.GetComponent<PlayerStats>();
        float water = (float)player.antivaxHP_present + player.worker_present;
        float food = (float)player.antivaxHP_present + player.worker_present;
        if(player.resources.x >= water && player.resources.y >= food)
        {
            player.resources.x -= water;
            player.resources.y -= food;
        }else
        {
            player.antivaxHP_present -= 1;
            UIM.showPopup("An anti vaxxer dies!");
            //Debug.Log("anti vaxxer dies");
            foreach (GameObject worker in player.workers)
            {
                //decrease efficiency? support rate?
            }
        }
    }

    public void PollutionPhase(){
        //Calc pollution meter for every tile that players worker & building is on
        //Calc efficiency different in worker on tile
        //Calc dome tile pollution and kill anti vaxxer
        Debug.Log("Pollution Phase");
        PlayerStats player = current_player.GetComponent<PlayerStats>();
        foreach(GameObject building in player.buildings)
        {
            building.GetComponent<Building>().pollute();
        }
        //player.dome_tile.GetComponent<TileClass>().UpdatePolluAmount();

        //UpdatePolluAmount
    }

    public void SupportRatePhase(){
        Debug.Log("Support Rate Phase");
        PlayerStats player = current_player.GetComponent<PlayerStats>();
        //Calc about it
    }

    public void CheckLosePhase(){
        Debug.Log("Check Lose Phase");
        PlayerStats player = current_player.GetComponent<PlayerStats>();
        if (player.antivaxHP_present <= 0){
            //game end -> do something
            UIM.showGameEnd();
            Debug.Log("Game End");
        }
    }

    public void randomEvents(){
        Debug.Log("Random Events");
        if(turnNum == 6 || turnNum == 7){
            //Debug.Log("Anti vaxxer want water memory");
            UIM.showPopup("Anti vaxxer want water memory, we lose some water");
            PlayerStats player = current_player.GetComponent<PlayerStats>();
            player.resources.x -= 3;
        }
    }

}

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


    void Start()
    {
        player1 = GameObject.Find("Player1");
        player2 = GameObject.Find("Player2");
        current_player = player1;
        turnNum = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Swap_player()
    {
        if (current_player == player1)
        {
            current_player = player2;
        }
        else
        {
            current_player = player1;
        }
        turnNum+=1;
    }

    public GameObject Get_current_player()
    {
        return current_player;
    }

    public void initiateTurn(){
        //Updating waste flow every turn 
        if (turnNum%2 == 1)
        {
            string[] waterFlow = { "0107", "0207", "0203", "0204", "0304", "0404", "0504", "0604", "0704", "0805", "0905", "1005", "1105", "1205", "1201", "1301" };
            foreach (string coordinate in waterFlow)
            {
                Water_tile tile = (Water_tile) GameObject.FindWithTag(coordinate).GetComponent("Water_tile");
                tile.UpdateWasteFlow();
            }
        }
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
        ResourceGatheringPhase();
        WorkerPhase();
        ConsumePhase();
        PollutionPhase();
        SupportRatePhase();
        CheckLosePhase();
        randomEvents();
    }

    public void ResourceGatheringPhase(){
        PlayerStats player = current_player.GetComponent<PlayerStats>();
        GameObject[] buildings = player.buildings;

        //Gather reources
        Vector4 resources = new Vector4(0,0,0,0); 
        float waste = 0;
        foreach (GameObject building in buildings){
            Vector4 gathered = building.GetComponent<Building>().getResources();
            resources += gathered;
            float gatheredWaste = building.GetComponent<Building>().giveWaste();
            waste += gatheredWaste;
        }
        float remain = waste;
        foreach (GameObject building in buildings){
            if (building.GetComponent<Building>().buildingType == "Landfill"){
                remain = building.GetComponent<Building>().saveWaste(remain);
                if(remain <= -1){
                    break;
                }
            }
        }
        if(remain > 0){
            //add remain Waste on player's dome tile
            player.dome_tile.GetComponent<Dome_tile>().resources.w += remain;
        }
        resources.w = waste;
        player.resources += resources;
    }

    public void WorkerPhase(){
        //do something with worker
        //Check the number of player's Residential area in the map
        PlayerStats player = current_player.GetComponent<PlayerStats>();
        int maxWorker = player.updateWorkerMax();
        int currentWorker = player.worker_present;
        //Current worker < max worker -> pop up new worker on the  one of residential area
    }

    public void ConsumePhase(){
        PlayerStats player = current_player.GetComponent<PlayerStats>();
        float water = (float)player.antivaxHP_present + player.worker_present;
        float food = (float)player.antivaxHP_present + player.worker_present;
        if(player.resources.x >= water && player.resources.y >= food){
            player.resources.x -= water;
            player.resources.y -= food;
        }else{
            player.antivaxHP_present -= 1;
            foreach (GameObject worker in player.workers){
                //decrease efficiency? support rate?
            }
        }
    }

    public void PollutionPhase(){
        //Calc pollution meter for every tile that players worker & building is on
        //Calc efficiency different in worker on tile
        //Calc dome tile pollution and kill anti vaxxer
    }

    public void SupportRatePhase(){
        PlayerStats player = current_player.GetComponent<PlayerStats>();
        //Calc about it
    }

    public void CheckLosePhase(){
        PlayerStats player = current_player.GetComponent<PlayerStats>();
        if (player.antivaxHP_present <= 0 || player.support_rate <= 0){
            //game end -> do something
            Debug.Log("Game End");
        }
    }

    public void randomEvents(){
        if(turnNum == 6 || turnNum == 7){
            Debug.Log("Anti vaxxer want water memory");
            PlayerStats player = current_player.GetComponent<PlayerStats>();
            player.resources.y -= 3;
        }
    }

}

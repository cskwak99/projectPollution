using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class show_resources : MonoBehaviour
{
    // Start is called before the first frame update
    public Text playerText;
    public Text waterText;
    public Text foodText;
    public Text metalText;
    public Text wasteText;

    private GameObject currentPlayer;
    private GameObject TM;
    string waterPerTurn;
    string foodPerTurn;
    string metalPerTurn;
    string wastePerTurn;
    void Start()
    {
        TM = GameObject.Find("TurnManager");
        currentPlayer = TM.GetComponent<TurnManager>().Get_current_player();
    }

    public void calcResourcePerTurn(PlayerStats current_player)
    {
        PlayerStats player = current_player.GetComponent<PlayerStats>();
        List<GameObject> buildings = player.buildings;

        //Gather reources
        Vector4 resources = new Vector4(0, 0, 0, 0);
        float waste = 0;
        foreach (GameObject building in buildings)
        {
            Vector4 gathered = building.GetComponent<Building>().getResources();
            resources += gathered;
            float gatheredWaste = building.GetComponent<Building>().giveWaste();
            waste += gatheredWaste;
        }
        float waterConsumed = (float)player.antivaxHP_present + player.worker_present;
        float foodConsumed = (float)player.antivaxHP_present + player.worker_present;
        resources.w += waste;
        resources.x -= waterConsumed;
        resources.y -= foodConsumed;
        waterPerTurn = resources.x.ToString();
        waterPerTurn = resources.x > 0 ?  "+" + waterPerTurn : waterPerTurn;
        foodPerTurn = resources.y.ToString();
        foodPerTurn = resources.y > 0 ? "+" + foodPerTurn : foodPerTurn;
        metalPerTurn = resources.z.ToString();
        metalPerTurn = resources.z > 0 ? "+" + metalPerTurn : metalPerTurn;
        wastePerTurn = resources.w.ToString();
        wastePerTurn = resources.w > 0 ? "+" + wastePerTurn : wastePerTurn;
    }

    // Update is called once per frame
    void Update()
    {
        currentPlayer = TM.GetComponent<TurnManager>().Get_current_player();
        playerText.text = currentPlayer.name;
        waterText.text = "Water: " + currentPlayer.GetComponent<PlayerStats>().Get_water()+" "+waterPerTurn+"/turn";
        foodText.text = "Food: " + currentPlayer.GetComponent<PlayerStats>().Get_food()+" " + foodPerTurn + "/turn";
        metalText.text = "Metal: " + currentPlayer.GetComponent<PlayerStats>().Get_metal()+" " + metalPerTurn + "/turn";
        wasteText.text = "Waste: " + currentPlayer.GetComponent<PlayerStats>().Get_waste()+" " + wastePerTurn + "/turn";
    }
}

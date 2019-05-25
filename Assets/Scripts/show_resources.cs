using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class show_resources : MonoBehaviour
{
    // Start is called before the first frame update
    public Text waterText;
    public Text foodText;
    public Text metalText;
    public Text wasteText;

    private GameObject currentPlayer;
    private GameObject TM;

    void Start()
    {
        TM = GameObject.Find("TurnManager");
        currentPlayer = TM.GetComponent<TurnManager>().Get_current_player();
    }

    // Update is called once per frame
    void Update()
    {
        currentPlayer = TM.GetComponent<TurnManager>().Get_current_player();
        waterText.text = "Water: " + currentPlayer.GetComponent<PlayerStats>().Get_water();
        foodText.text = "Food: " + currentPlayer.GetComponent<PlayerStats>().Get_food();
        metalText.text = "Metal: " + currentPlayer.GetComponent<PlayerStats>().Get_metal();
        wasteText.text = "Waste: " + currentPlayer.GetComponent<PlayerStats>().Get_waste();
    }
}

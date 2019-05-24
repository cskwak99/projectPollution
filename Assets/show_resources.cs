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
    public PlayerStats currentPlayer;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        waterText.text = "Water: " + currentPlayer.water;
        foodText.text = "Food: " + currentPlayer.food;
        metalText.text = "Metal: " + currentPlayer.metal;
        wasteText.text = "Waste: " + currentPlayer.waste;
    }
}

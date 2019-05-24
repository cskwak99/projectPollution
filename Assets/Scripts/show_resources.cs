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
        currentPlayer = new PlayerStats();
        currentPlayer.resources = new Vector4(100, 100, 100, 100);
    }

    // Update is called once per frame
    void Update()
    {
        waterText.text = "Water: " + currentPlayer.get_water();
        foodText.text = "Food: " + currentPlayer.get_food();
        metalText.text = "Metal: " + currentPlayer.get_metal();
        wasteText.text = "Waste: " + currentPlayer.get_waste();
    }
}

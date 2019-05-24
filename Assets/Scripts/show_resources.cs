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
        waterText.text = "Water: " + currentPlayer.Get_water();
        foodText.text = "Food: " + currentPlayer.Get_food();
        metalText.text = "Metal: " + currentPlayer.Get_metal();
        wasteText.text = "Waste: " + currentPlayer.Get_waste();
    }
}

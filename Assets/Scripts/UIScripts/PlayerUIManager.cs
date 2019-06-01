using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    private TurnManager TM;
    private GameObject currentPlayer;
    void Start()
    {
        TM = GameObject.Find("TurnManager").GetComponent<TurnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        currentPlayer = TM.Get_current_player();
        this.GetComponent<Text>().text = currentPlayer.name;
    }
}

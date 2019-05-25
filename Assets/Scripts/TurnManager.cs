using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player1;
    public GameObject player2;
    public GameObject current_player;


    void Start()
    {
        player1 = GameObject.Find("Player1");
        player2 = GameObject.Find("Player2");
        current_player = player1;
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
    }

    public GameObject Get_current_player()
    {
        return current_player;
    }

}

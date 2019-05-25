using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndturnClick : MonoBehaviour
{
    private GameObject TM;
    private GameObject current_player;
    // Start is called before the first frame update
    void Start()
    {
        TM = GameObject.Find("TurnManager");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Endturn_click()
    {
        TM.GetComponent<TurnManager>().Swap_player();
    }
}

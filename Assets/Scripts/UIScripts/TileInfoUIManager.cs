﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TileInfoUIManager : MonoBehaviour
{
    // Start is called before the first frame update
    public void onTileSelected(TileClass tile)
    {
        this.GetComponent<CanvasGroup>().alpha = 1f;
        transform.Find("TileInfo_Text").GetComponent<Text>().text = "\t"+tile.tileDescription;
        if(tile.polluAmount > tile.maxPolluAmount)
        {
            transform.Find("PollutionMeter/PollutionMeter_Bar").GetComponent<RectTransform>().localScale = new Vector3(tile.maxPolluAmount / tile.maxPolluAmount, 1.0f, 1.0f);
        }
        else
        {
            transform.Find("PollutionMeter/PollutionMeter_Bar").GetComponent<RectTransform>().localScale = new Vector3(tile.polluAmount / tile.maxPolluAmount, 1.0f, 1.0f);
        } 
    }
    void onTileUnSelected()
    {
        this.GetComponent<CanvasGroup>().alpha = 0f;
    }

}

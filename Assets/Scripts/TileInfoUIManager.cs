using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TileInfoUIManager : MonoBehaviour
{
    // Start is called before the first frame update
    void onTileSelected(TileClass tile)
    {
        this.enabled = true;
        transform.Find("TileInfo_Text").GetComponent<Text>().text = "\t"+tile.tileDescription;
        transform.Find("PollutionMeter/PollutionMeter_Bar").GetComponent<RectTransform>().localScale = new Vector3(tile.polluAmount/tile.maxPolluAmount, 1.0f, 1.0f);
    }
    void onTileUnSelected()
    {
        this.enabled = false;
    }
}

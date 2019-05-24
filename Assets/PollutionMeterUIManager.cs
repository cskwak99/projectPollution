using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PollutionMeterUIManager : MonoBehaviour
{
    // Start is called before the first frame update
    void onTileSelected(TileClass tile)
    {
        print("Check the scale!");
        this.transform.localScale.Set(tile.polluAmount/tile.maxPolluAmount,1.0f,1.0f);
    }
}

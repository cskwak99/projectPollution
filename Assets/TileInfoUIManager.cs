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
    }
    void onTileUnSelected()
    {
        this.enabled = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPUIManager : MonoBehaviour
{
    void onTileSelected(TileClass tile)
    {
        this.transform.position.Set(304.9f, 12.8f, 0.0f);
    }
    void onTileUnSelected(TileClass tile)
    {
        this.transform.position.Set(42.5f, 10.9f, 0.0f);
    }
}

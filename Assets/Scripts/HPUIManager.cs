using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPUIManager : MonoBehaviour
{
    void onTileSelected(TileClass tile)
    {
        this.GetComponent<RectTransform>().anchoredPosition = new Vector3(304.9f, 12.8f, 0.0f);
    }
    void onTileUnSelected()
    {
        print("ASD");
        this.GetComponent<RectTransform>().anchoredPosition = new Vector3(42.5f, 12.8f, 0.0f);
    }
}

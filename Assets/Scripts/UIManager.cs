using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    private GUI tileInfo;
    public void manageUI(TileClass tile)
    {
        
        if (tile!=null)
        {
            print(tile.gameObject.name);
            BroadcastMessage("onTileSelected", tile);
        }
        else
        {
            print("SPACE");
            BroadcastMessage("onTileUnSelected");
        }
    }
}

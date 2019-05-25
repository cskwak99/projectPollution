using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    private GUI tileInfo;
    private GameObject currentOptionList;
    public void manageUI(TileClass tile)
    {
        if (tile!=null)
        {
            print(tile.gameObject.name);
            BroadcastMessage("onTileSelected", tile);

            Destroy(currentOptionList);
            //string[] optionList = tile.getOptions();
            string[] optionList = { "A", "B" };
            GameObject panel = new GameObject("Panel");
            panel.AddComponent<CanvasRenderer>();
            Image i = panel.AddComponent<Image>();
            i.color = Color.red;
            foreach (string option in optionList)
            {
                GameObject opt = new GameObject(option + " option");
                Button b = opt.AddComponent<Button>();
                Image img = opt.AddComponent<Image>();
                opt.transform.SetParent(panel.transform, false);
            }
            panel.transform.SetParent(this.transform, false);
            currentOptionList = panel;
        }
        else
        {
            print("SPACE");
            BroadcastMessage("onTileUnSelected");
        }
        
    }
}

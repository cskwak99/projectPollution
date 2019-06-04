using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPUIManager : MonoBehaviour
{
    public void onTileSelected(TileClass tile)
    {
        this.GetComponent<RectTransform>().anchoredPosition = new Vector3(284.9f, 12.8f, 0.0f);
    }
    public void onTileUnSelected()
    {
        this.GetComponent<RectTransform>().anchoredPosition = new Vector3(12.5f, 12.8f, 0.0f);
    }
    private void Update()
    {
        PlayerStats player = GameObject.Find("TurnManager").GetComponent<TurnManager>().Get_current_player().GetComponent<PlayerStats>();
        transform.Find("AntiVaxxer'sHP").Find("AntiVaxxer's_Bar").gameObject.GetComponent<RectTransform>().localScale = new Vector3((float)(player.antivaxHP_present / player.antivaxHP_max), 1, 1);
        //print(player.worker_max);
        transform.Find("Worker'sHP").Find("Worker's_Bar").gameObject.GetComponent<RectTransform>().localScale = new Vector3((float)(player.worker_present / player.worker_max), 1, 1);

    }
}

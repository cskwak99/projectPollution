using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPUIManager : MonoBehaviour
{
    public void onTileSelected(TileClass tile)
    {
        this.GetComponent<RectTransform>().anchoredPosition = new Vector3(320.9f, 12.8f, 0.0f);
    }
    public void onTileUnSelected()
    {
        this.GetComponent<RectTransform>().anchoredPosition = new Vector3(12.5f, 12.8f, 0.0f);
    }
    private void Update()
    {
        PlayerStats player = GameObject.Find("TurnManager").GetComponent<TurnManager>().Get_current_player().GetComponent<PlayerStats>();
        // print(player.antivaxHP_present / player.antivaxHP_max);
        float AntivaxScale = Mathf.Clamp((float)player.antivaxHP_present / player.antivaxHP_max, 0, 1);
        float WorkerScale = Mathf.Clamp((float) player.worker_present / player.worker_max, 0, 1);
        transform.Find("AntiVaxxer'sHP").Find("AntiVaxxer's_Bar").gameObject.GetComponent<RectTransform>().localScale = new Vector3(AntivaxScale, 1, 1);
        transform.Find("Worker'sHP").Find("Worker's_Bar").gameObject.GetComponent<RectTransform>().localScale = new Vector3(WorkerScale, 1, 1);
    }
}

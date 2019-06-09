using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class worker : MonoBehaviour
{
    public PlayerStats player;
    public GameObject worker_obj;
    public UIManager UIM;
    public string worker_name;
    public int actionLeft = 2;
    public int maxMoveRange = 2;
    public int maxPurifyRange = 1;
    public int purifyPower = 30;
    // Start is called before the first frame update
    public void init_worker(PlayerStats player, string name, GameObject worker_prefab)
    {
        this.player = player;
        worker_name = name;
        this.gameObject.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
        this.gameObject.transform.position = transform.parent.transform.position + new Vector3(0f, 0.05f, 0f);
        Debug.Log("new worker");
    }
    public string[] get_action()
    {
        return new string[] {"Move", "Purify", "Build"};
    }
    public void build(string name, TileClass tile, GameObject current_player)
    {
        if (actionLeft <= 0)
        {
            UIM.showPopup("This worker have no action point left!");
            return;
        }
        bool isBuildSuccess = GameObject.Find("_BuildManager").GetComponent<BuildManager>().route_construction(name, tile, current_player);
        if(isBuildSuccess) actionLeft -= 1;
    }
    public IEnumerator move_worker()
    {
        if(actionLeft <= 0)
        {
            UIM.showPopup("This worker have no action point left!");
            yield break;
        }
        TurnManager TM = GameObject.Find("TurnManager").GetComponent<TurnManager>();
        UIM = GameObject.Find("UI").GetComponent<UIManager>();
        TileClass destTile = null;
        clickHandler CLK = GameObject.Find("UI").GetComponent<clickHandler>();
        yield return StartCoroutine(CLK.getDestTile(tile => destTile = tile));
        float range = transform.parent.GetComponent<TileClass>().calcDist(destTile);
        if (range == 0)
            yield break;
        else if (range > maxMoveRange)
        {
            UIM.showPopup("Too far to reach!");
            yield break;
        }
        if (destTile.isWorkerOn())
        {
            UIM.showPopup("Can't move to existings worker!");
            yield break;
        }
        if (player.player_number == 1)
        {
            if (destTile.isPlayerBuildingOn(TM.player2.GetComponent<PlayerStats>()))
            {
                UIM.showPopup("Can't move to another player's building!");
                yield break;
            }
        }
        else
        {
            if (destTile.isPlayerBuildingOn(TM.player1.GetComponent<PlayerStats>()))
            {
                UIM.showPopup("Can't move to another player's building!");
                yield break;
            }
        }
        transform.parent = destTile.gameObject.transform;
        transform.position = transform.parent.transform.position + new Vector3(0f, 0.05f, 0f);
        UIM.UpdateResourcesPerTurn();
        actionLeft -= 1;
    }
    public IEnumerator purify_tile()
    {
        if (actionLeft <= 0)
        {
            UIM.showPopup("This worker have no action point left!");
            yield break;
        }
        TurnManager TM = GameObject.Find("TurnManager").GetComponent<TurnManager>();
        UIM = GameObject.Find("UI").GetComponent<UIManager>();
        TileClass destTile = null;
        clickHandler CLK = GameObject.Find("UI").GetComponent<clickHandler>();
        yield return StartCoroutine(CLK.getDestTile(tile => destTile = tile));
        float range = transform.parent.GetComponent<TileClass>().calcDist(destTile);
        if (range == 0)
            yield break;
        else if (range > maxPurifyRange)
        {
            UIM.showPopup("Too far to reach!");
            yield break;
        }
        TileClass t = destTile.GetComponent<TileClass>();
        t.UpdatePolluAmount(t.polluAmount - purifyPower);
        player.workers.Remove(gameObject);
        player.worker_present -= 1;
        Destroy(this.gameObject);
        actionLeft -= 1;
    }
}

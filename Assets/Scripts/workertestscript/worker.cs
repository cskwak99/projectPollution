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
    public int maxMoveRange = 1;
    public int maxPurifyRange = 2;
    public int purifyPower = 10;
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
    public IEnumerator move_worker()
    {
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
        actionLeft -= 1;
    }
    public IEnumerator purify_tile()
    {
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
        TileClass t = destTile.GetComponent<TileClass>();
        t.UpdatePolluAmount(t.polluAmount - purifyPower);
        actionLeft -= 1;
    }
}

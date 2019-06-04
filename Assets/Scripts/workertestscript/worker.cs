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
    private int maxMoveRange = 1;
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
        UIM = GameObject.Find("UI").GetComponent<UIManager>();
        TileClass destTile = null;
        clickHandler CLK = GameObject.Find("UI").GetComponent<clickHandler>();
        yield return StartCoroutine(CLK.getDestTile(tile => destTile = tile));
        if(transform.parent.GetComponent<TileClass>().calcDist(destTile) > maxMoveRange)
        {
            UIM.showPopup("Too far to reach!");
            yield break;
        }
        if (destTile.isWorkerOn())
        {
            UIM.showPopup("Can't move to existings worker!");
            yield break;
        }   
        transform.parent = destTile.gameObject.transform;
        transform.position = transform.parent.transform.position + new Vector3(0f, 0.05f, 0f);
        actionLeft -= 1;
    }
}

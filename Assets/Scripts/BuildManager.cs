using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    private GameObject target_tile;
    private GameObject clone_farm;
    private GameObject clone_waterpump;
    private GameObject clone_landfill;
    private GameObject clone_residental;
    private GameObject clone_mine;
    public GameObject farm;
    public GameObject waterpump;
    public GameObject landfill;
    public GameObject residental;
    public GameObject mine;
    // Start is called before the first frame update
    void Start()
    {
        target_tile = GameObject.Find("0400Plain_tile"); // we should implement it
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init_Farm()
    {
        clone_farm = Instantiate(farm);
        clone_farm.transform.parent = target_tile.transform;
        clone_farm.GetComponent<Building>().setInitial();
    }
    public void Init_Waterpump()
    {
        clone_waterpump = Instantiate(waterpump);
        clone_waterpump.transform.parent = target_tile.transform;
        clone_waterpump.GetComponent<Building>().setInitial();
    }
    public void Init_Landfill()
    {
        clone_landfill = Instantiate(landfill);
        clone_landfill.transform.parent = target_tile.transform;
        clone_landfill.GetComponent<Building>().setInitial();
    }
    public void Init_Residental()
    {
        clone_residental = Instantiate(residental);
        clone_residental.transform.parent = target_tile.transform;
        clone_residental.GetComponent<Building>().setInitial();
    }
    public void Init_Mine()
    {
        clone_mine = Instantiate(mine);
        clone_mine.transform.parent = target_tile.transform;
        clone_mine.GetComponent<Building>().setInitial();
    }
}

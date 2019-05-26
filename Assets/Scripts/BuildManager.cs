using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    private GameObject target_tile;
    private GameObject clone_factory;
    public GameObject farm;
    // Start is called before the first frame update
    void Start()
    {
        target_tile = GameObject.Find("0400Plain_tile");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init_Factory()
    {
        clone_factory = Instantiate(farm);
        clone_factory.transform.parent = target_tile.transform;
        clone_factory.GetComponent<Building>().setInitial();
    }
}

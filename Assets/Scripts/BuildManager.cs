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
        Vector3 addvec = new Vector3(0, 0.052f, 0);
        //clone_factory = Instantiate(farm, target_tile.transform.position, target_tile.transform.rotation);
        clone_factory = Instantiate(farm);
        clone_factory.transform.parent = target_tile.transform;
        //Vector3 prev_pos = clone_factory.transform.position;
        //clone_factory.transform.position = new Vector3(0, 0, 1)
        //clone_factory.transform.Translate(addvec,Space.Self);
        //clone_factory.transform.Rotate(new Vector3(90, 0, 0));

        //clone_factory.GetComponent<Building>().setInitial();
        //clone_factory.transform.Rotate(new Vector3(90, 0, 0));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TileClass : MonoBehaviour
{
    // Start is called before the first frame update
    public string tileDescription;
    public float polluAmount = 30;
    public float maxPolluAmount = 100;
    public Vector4 resources = new Vector4(); // food, water, metal, waste


    void Start()
    {
        tileDescription = "A Description";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdatePolluAmount (float pollu)
    {
        polluAmount += pollu;
    }

    Vector4 getResources(Vector4 resourcesTaken)
    {
        Vector4 resourcesTrulyTaken = Vector4.Min(resourcesTaken, resources);
        resources = resources - resourcesTrulyTaken;
        return resourcesTrulyTaken;
    }

}


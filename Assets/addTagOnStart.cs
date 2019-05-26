using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class addTagOnStart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        foreach(Transform t in transform)
        {
            t.gameObject.tag = t.name.Substring(0, 4);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

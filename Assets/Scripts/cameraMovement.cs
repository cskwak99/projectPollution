using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMovement : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 iniMousePos;
    private Vector3 iniCamPos;
    public float camSpeed = 0.05f;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(2))
        {
            
            if(iniMousePos == Vector3.zero)
            {
                iniMousePos = Input.mousePosition;
                iniCamPos = this.transform.position;
                print("ADSAD");
            }
            else
            {
                print("asd");
                this.transform.position = (iniCamPos + (iniMousePos - Input.mousePosition)*camSpeed);
            }
        }
        else
        {
            print("asd123");
            iniMousePos = Vector3.zero;
        }
    }
}

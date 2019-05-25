using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building
{
    public string buildingType; //To specify building type
    public double efficiency; //To get efficiecny of worker so that evaluate the building work //Maybe change to float
    public GameObject assignedWorker; //To check whether building can do something
    public int wasteMk;
    public int airPoMk;

    //public Vector4 getResources(Vector4 remainResources); //For every building, return Vec4 info about resources that player get
    //public int makeAirPo(); //For factory building, make fixed amout of air pollution
    
}

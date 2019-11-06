using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotorNode : MonoBehaviour
{
    public IParts parts;
    public List<MotorLink> lList;

    void Start()
    {
        lList = new List<MotorLink>();
    }

    public void AddLink(MotorLink link)
    {
        lList.Add(link);
    }

    public void DelLink(MotorLink link)
    {
        if(lList.Remove(link))
        {

        }
    }
}
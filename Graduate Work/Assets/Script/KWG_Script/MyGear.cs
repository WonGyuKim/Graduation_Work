using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGear : MyParts
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void LinkRotation(float F, float V)
    {
        throw new System.NotImplementedException();
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hi!");
        if (other.tag == "Gear")
        {
            //LinkParts.Add(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Gear")
        {
            //LinkParts.Remove(other.gameObject);
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collide : MonoBehaviour
{
    private Object ob;
    public bool close;
    // Start is called before the first frame update
    void Start()
    {
        ob = GameObject.Find(transform.parent.name).GetComponent<Object>();
        close = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.name != transform.parent.name)
        {
            if (transform.tag == "Conn_Hole")
            {
                if (other.tag == "Axle" || other.tag == "Connector")
                {
                    ob.tEnter = true;
                    close = true;
                }
            }
            else if (transform.tag == "Axle_Hole")
            {
                if (other.tag == "Axle")
                {
                    ob.tEnter = true;
                    close = true;
                }
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        
    }

    void OnTriggerExit(Collider other)
    {
        if (other.name != transform.parent.name)
        {
            ob.tEnter = false;
            close = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collide : MonoBehaviour
{
    Collider col;
    int Collide_num = 0;
    bool colli = false;
    // Start is called before the first frame update
    void Start()
    {
        col = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDrag()
    {
        if (col != null)
        {
            transform.position = col.transform.position;
            transform.rotation = col.transform.rotation;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(this.tag == "Axle")
        {
            col = other;
            colli = true;
        }
        else if(this.tag == "Connector")
        {
            if(other.tag == "Conn_Hole")
            {
                col = other;
                colli = true;
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (Collide_num != 30 && colli)
            Collide_num++;
        else
        {
            col = null;
            Collide_num = 0;
            colli = false;
        }
    }

    void OnTriggerExit(Collider other)
    {
        col = null;
        Collide_num = 0;
        colli = false;
    }
}

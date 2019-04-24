using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collide : MonoBehaviour
{
    private Object pnt;
    private bool close;
    private Transform Parent;
    private Vector3 dst;
    private Object ob;
    private GameObject gobj;
    
    void Start()
    {
        Parent = transform.parent;
        pnt = Parent.gameObject.GetComponent<Object>();
        close = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.Equals(Parent.gameObject) && (pnt.OnDrag || other.gameObject.GetComponent<Object>().OnDrag) && !close)
        {
            if ((transform.tag == "Conn_Hole" && (other.tag == "Axle" || other.tag == "Connector")) || (transform.tag == "Axle_Hole" && other.tag == "Axle"))
            {
                ob = other.gameObject.GetComponent<Object>();
                gobj = other.gameObject;
                pnt.tEnter = true;
                ob.tEnter = true;
                close = true;
                if (pnt.OnDrag)
                {
                    dst = other.transform.position - transform.position;
                    Parent.position = Parent.position + dst;
                    Parent.rotation = other.transform.rotation;
                    pnt.befoMouse = Input.mousePosition;
                }
                else if (ob.OnDrag)
                {
                    other.transform.position = transform.position;
                    other.transform.rotation = transform.rotation;
                    ob.befoMouse = Input.mousePosition;
                }
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (close && !other.gameObject.Equals(Parent.gameObject))
            pnt.tEnter = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.Equals(gobj))
        {
            pnt.tEnter = false;
            ob.tEnter = false;
            close = false;
        }
    }
}

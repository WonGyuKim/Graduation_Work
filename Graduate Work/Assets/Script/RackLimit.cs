using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RackLimit : MonoBehaviour
{
    RackGear rack;
    RotateMotor rotM;

    void Start()
    {
        Transform Parent = transform.parent.transform;
        rack = Parent.gameObject.GetComponent<RackGear>();
        rotM = GameObject.Find("RotateControl").GetComponent<RotateMotor>();
    }

    //void OnTriggerEnter(Collider other)
    //{
    //    if(rotM.motoring)
    //    {
    //        foreach (MotorLink link in rack.node.lList)
    //        {
    //            if(link.type == MotorLink.LinkType.Rack)
    //            {
    //                if (link.right.gameObj == other.gameObject || link.left.gameObj == other.gameObject)
    //                {
    //                    foreach (Motor m in rotM.motorList)
    //                    {
    //                        m.RotateSpeed = -m.RotateSpeed;
    //                    }
    //                }
    //            }
    //        }
    //    }
    //}

    void OnTriggerExit(Collider other)
    {
        if(rotM.motoring)
        {
            Vector3 gearPos = other.transform.position - transform.position;
            Vector3 dir = rack.transform.position - transform.position;

            if (Vector3.Dot(gearPos, dir) < 0)
            {
                foreach (MotorLink link in rack.node.lList)
                {
                    if (link.type == MotorLink.LinkType.Rack)
                    {
                        if (link.right.gameObj == other.gameObject || link.left.gameObj == other.gameObject)
                        {
                            foreach (Motor m in rotM.motorList)
                            {
                                m.RotateSpeed = -m.RotateSpeed;
                            }
                        }
                    }
                }
            }
        }
    }
}

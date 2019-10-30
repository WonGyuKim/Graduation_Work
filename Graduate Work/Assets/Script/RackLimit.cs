using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RackLimit : MonoBehaviour
{
    RackGear rack;
    RotateMotor rotM;
    public string Name;

    void Start()
    {
        Transform Parent = transform.parent.transform;
        rack = Parent.gameObject.GetComponent<RackGear>();
        rotM = GameObject.Find("RotateControl").GetComponent<RotateMotor>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (rotM.motoring)
        {
            foreach (MotorLink link in rack.node.lList)
            {
                if (link.type == MotorLink.LinkType.Rack)
                {
                    if (link.right.gameObj == other.gameObject || link.left.gameObj == other.gameObject)
                    {
                        Vector3 dir = rack.transform.position - transform.position;
                        //dir = dir.normalized;

                        if (Vector3.Dot(dir, rack.moveDir) > 0)
                        {
                            //foreach (MoveCell cell in rack.moveList)
                            //{
                            //    cell.Motor.RotateSpeed = -cell.Motor.RotateSpeed;
                            //}
                            rack.cell.Motor.RotateSpeed = -rack.cell.Motor.RotateSpeed;
                        }
                    }
                }
            }
        }
    }

    //void OnTriggerExit(Collider other)
    //{
    //    if (rotM.motoring)
    //    {
    //        foreach (MotorLink link in rack.node.lList)
    //        {
    //            if (link.type == MotorLink.LinkType.Rack)
    //            {
    //                if (link.right.gameObj == other.gameObject || link.left.gameObj == other.gameObject)
    //                {
    //                    Vector3 dir = rack.transform.position - transform.position;
    //                    dir = dir.normalized;
    //                    Vector3 rdir = new Vector3();
    //                    foreach (MoveCell mc in rack.moveList)
    //                    {
    //                        rdir = mc.Axis;
    //                    }
    //                    rdir = this.transform.TransformPoint(rdir);
    //                    rdir = rdir.normalized;
                        
    //                    if (Vector3.Dot(dir, rdir) > 0)
    //                    {
    //                        foreach (Motor m in rotM.motorList)
    //                        {
    //                            m.RotateSpeed = -m.RotateSpeed;
    //                        }
    //                        return;
    //                    }
    //                }
    //            }
    //        }
    //    }
    //}
}

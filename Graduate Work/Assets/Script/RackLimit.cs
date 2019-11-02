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
                    if (link.right.gameObj.Equals(other.gameObject) || link.left.gameObj.Equals(other.gameObject))
                    {
                        Vector3 dir = rack.transform.position - transform.position;

                        if (Vector3.Dot(dir, rack.cell.Axis) > 0)
                        {
                            rack.cell.Axis = -rack.cell.Axis;
                            rack.cell.Motor.RotateSpeed = -rack.cell.Motor.RotateSpeed;
                        }
                    }
                }
            }
        }
    }
}

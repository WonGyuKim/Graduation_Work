﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGear : MyParts
{
    public override void LinkRotation(MyParts parent, PowerData power)
    {
        if (power.RotationDirection == true)
            transform.Rotate(new Vector3(0, 0, 1), power.Velocity * -1);
        else // power.RotationDirection == false
            transform.Rotate(new Vector3(0, 0, 1), power.Velocity);
            
            /*
        if (power.RotationDirection == true)
            GetComponent<Rigidbody>().AddTorque(power.AngularVelocity * power.Velocity, ForceMode.Acceleration);
        else
            GetComponent<Rigidbody>().AddTorque(power.AngularVelocity * power.Velocity * -1, ForceMode.Acceleration);

        Debug.Log("Gear Angular Velocity : " + GetComponent<Rigidbody>().angularVelocity);
        */

        foreach (MyParts parts in LinkParts)
        {
            if (!parts.Equals(parent))
            {
                if (parts.tag == "Gear")
                    parts.LinkRotation(this, new PowerData(power.Force, power.Velocity, power.AngularVelocity, !power.RotationDirection));
                else
                    parts.LinkRotation(this, power);
            }
            
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hi!");

        if (other.tag == "Gear")
        {
            //LinkParts.Add(other.gameObject.GetComponent<MyParts>());

            Link(other.gameObject.GetComponent<MyParts>());
        }
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log("Bye!");
        if (other.tag == "Gear")
        {
            //LinkParts.Remove(other.gameObject);
            LinkExit(other.gameObject.GetComponent<MyParts>());
        }
    }

}

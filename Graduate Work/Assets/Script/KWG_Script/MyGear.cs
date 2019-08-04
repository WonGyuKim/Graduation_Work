using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGear : MyParts
{
    public override void LinkRotation(MyParts parent, PowerData power)
    {
        if (power.RotationDirection == true)
            transform.Rotate(new Vector3(0, 0, 1), power.Velocity * -1);
        else /* power.RotationDirection == false */
            transform.Rotate(new Vector3(0, 0, 1), power.Velocity);

        foreach (MyParts parts in LinkParts)
        {
            if (!parts.Equals(parent))
            {
                if (parts.tag == "Gear")
                    parts.LinkRotation(this, new PowerData(power.Force, power.Velocity, !power.RotationDirection));
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

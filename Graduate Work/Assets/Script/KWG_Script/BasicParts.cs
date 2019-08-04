using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicParts : MyParts
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
                parts.LinkRotation(this, power);
        }
    }

}

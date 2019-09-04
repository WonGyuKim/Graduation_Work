using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LooseLink : Link
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void LinkRotation(MyParts parent, PowerData power)
    {
        //if (!left.Equals(parent))
            //left.LinkRotation(this, power);
            //left.transform.RotateAround(parent.transform.position, new Vector3(0, 0, 1), power.Velocity * -1);
        //else if (!right.Equals(parent))
            //right.LinkRotation(this, power);
            //right.transform.RotateAround(parent.transform.position, new Vector3(0, 0, 1), power.Velocity * -1);
    }
}

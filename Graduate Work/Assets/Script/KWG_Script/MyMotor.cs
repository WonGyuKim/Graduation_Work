using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyMotor : MyParts
{
    protected override void StartOverride()
    {

    }
    private bool action = true;
    // Start is called before the first frame update
    /*void Start()
    {
        action = true;
    }
    */
    // Update is called once per frame
    void Update()
    {
        if (action)
            EnableRotation(new PowerData(1, 1, true));
    }

    public void EnableRotation(PowerData power)
    {
        
        if (power.RotationDirection == true)
            transform.Rotate(new Vector3(0, 0, 1), power.Velocity * -1);
        else // power.RotationDirection == false 
            transform.Rotate(new Vector3(0, 0, 1), power.Velocity);
            
        foreach (Link parts in LinkParts)
        {
            parts.LinkRotation(this, power);
        }
    }

    public override void LinkRotation(Link parent, PowerData power)
    {
        
        if (power.RotationDirection == true)
            transform.Rotate(new Vector3(0, 0, 1), power.Velocity * -1);
        else // power.RotationDirection == false
            transform.Rotate(new Vector3(0, 0, 1), power.Velocity);
        
        foreach (Link parts in LinkParts)
        {
            if (!parts.Equals(parent))
                parts.LinkRotation(this, power);
        }
    }

}

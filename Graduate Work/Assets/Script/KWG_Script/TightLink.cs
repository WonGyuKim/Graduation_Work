using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TightLink : Link
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
        foreach (MyParts parts in linking)
        {
            if (!parts.Equals(parent))
                parts.LinkRotation(this, power);
        }
    }
}

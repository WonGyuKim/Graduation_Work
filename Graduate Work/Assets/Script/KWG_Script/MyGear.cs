using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGear : MonoBehaviour, PartsAdapter
{
    GearPart parts;
    // Start is called before the first frame update
    void Start()
    {
        parts = new GearPart();
    }
 
    // Update is called once per frame
    void Update()
    {
        
    }

    public MyParts getParts()
    {
        return parts;
    }
}

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

    private void OnMouseDown()
    {
        
    }

    private void OnMouseDrag()
    {
        if (Input.GetKey(KeyCode.LeftControl))
            parts.ArcballMove(this.gameObject);
    }

    private void OnMouseUp()
    {
        
    }
}

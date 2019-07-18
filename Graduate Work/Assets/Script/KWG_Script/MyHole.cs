using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyHole : MonoBehaviour
{
    private GameObject parent;
    private GameObject target;
    private MyParts parentParts;
    private MyParts targetParts;
    private bool connecting;

    // Start is called before the first frame update
    void Start()
    {
        parent = transform.parent.gameObject;
        parentParts = parent.GetComponent<PartsAdapter>().getParts();
        Debug.Log(parentParts);
        connecting = false;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!connecting &&
            !other.gameObject.Equals(parent) &&
            (transform.tag == "Conn_Hole" && (other.tag == "Axle" || other.tag == "Connector")) ||
            (transform.tag == "Axle_Hole" && other.tag == "Axle"))
        {
            connecting = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (connecting && other.gameObject.Equals(target))
        {
            connecting = false;

            Destroy(target);
        }
    }
}

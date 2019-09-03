using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyHole : MonoBehaviour
{
    private GameObject body;
    private GameObject target;
    private MyParts bodyParts;
    private MyParts targetParts;
    private bool connecting;
    private Link link;

    // Start is called before the first frame update
    void Start()
    {
        body = transform.parent.gameObject;
        bodyParts = body.GetComponent<MonoBehaviour>() as MyParts;

        connecting = false;
    }
    
    private Link LinkFactory(string hole, string enter)
    {
        if (hole == "Conn_Hole" && enter == "Axle")
            return new LooseLink();
        else
            return new TightLink();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!connecting &&
            !other.gameObject.Equals(body) &&
            (transform.tag == "Conn_Hole" && (other.tag == "Axle" || other.tag == "Connector")) ||
            (transform.tag == "Axle_Hole" && other.tag == "Axle"))
        {
            target = other.transform.gameObject;
            targetParts = target.GetComponent<MyParts>();

            Debug.Log("body Parts : " + bodyParts);
            Debug.Log("target Parts : " + targetParts);

            link = LinkFactory(tag, other.tag);

            Debug.Log(bodyParts.child);
            Debug.Log(targetParts.child);

            if (bodyParts.child.Count > 0 && targetParts.child.Count == 0)
            {
                targetParts.transform.rotation = bodyParts.transform.rotation;
                targetParts.transform.position = bodyParts.transform.position;
            }
            else
            {
                bodyParts.transform.rotation = target.transform.rotation;
                bodyParts.transform.position = target.transform.position;
            }
            /*
            targetParts.Link(link);
            bodyParts.Link(link);
            */
            link.Connect(bodyParts, targetParts);

            connecting = true;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (connecting && other.gameObject.Equals(target))
        {
            //bodyParts.LinkExit(targetParts);
            //targetParts.LinkExit(bodyParts);
            link.Disconnect();
            target = null;
            targetParts = null;
            connecting = false;
        }
    }

    
}

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

    // Start is called before the first frame update
    void Start()
    {
        body = transform.parent.gameObject;
        bodyParts = body.GetComponent<MonoBehaviour>() as MyParts;

        connecting = false;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!connecting &&
            !other.gameObject.Equals(body) &&
            (transform.tag == "Conn_Hole" && (other.tag == "Axle" || other.tag == "Connector")) ||
            (transform.tag == "Axle_Hole" && other.tag == "Axle"))
        {
            Debug.Log("Hi!");
            target = other.transform.gameObject;
            targetParts = target.GetComponent<MonoBehaviour>() as MyParts;

            /* 
             * if [target.body == null], it means the object has No body 
             * the object should be child of this object
             * and Additionally,
             * if [target.ChildNum != 0], it means
             * they already have there Level before attaching this object
             * so we need to re-build them
             * 
             */
            if (targetParts.parent == null)
            {
                bodyParts.Link(targetParts);
                targetParts.parent = bodyParts;
            }
            /*
             * if [target.body != null], it means the object has body
             * maybe axle, 
             * So we should attach this object to target
             * 
             */
            else
            {
                bodyParts.parent = targetParts;
                targetParts.Link(bodyParts);
            }
            Debug.Log("body Object is : " + bodyParts);
            Debug.Log("child Object is : " + targetParts);

            connecting = true;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (connecting && other.gameObject.Equals(target))
        {
            Debug.Log("Bye!");
            if (targetParts.parent == bodyParts)
            {
                targetParts.parent = null;
                bodyParts.LinkExit(targetParts);
            }
            else
            {
                bodyParts.parent = null;
                targetParts.LinkExit(bodyParts);
            }
            
            target = null;

            connecting = false;

        }
    }
}

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
            Debug.Log("body Object is : " + bodyParts);
            Debug.Log("child Object is : " + targetParts);

            //if (bodyParts.tag == "Motor")
            {
                targetParts.Link(bodyParts);
                bodyParts.Link(targetParts);
            }
            /*else
            {
                bodyParts.parent = targetParts;
                targetParts.Link(bodyParts);
                
            }*/

            if (targetParts.child.Count != 0)
            {
                //bodyParts.transform.rotation = new Quaternion(target.transform.rotation.x, target.transform.rotation.y, target.transform.rotation.z, target.transform.rotation.w);
                bodyParts.transform.rotation = target.transform.rotation;
                bodyParts.transform.position = target.transform.position;
            }
            else
            {
                targetParts.transform.rotation = bodyParts.transform.rotation;
                targetParts.transform.position = bodyParts.transform.position;
                
            }



            connecting = true;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (connecting && other.gameObject.Equals(target))
        {
            Debug.Log("Bye!");
            /*if (targetParts.parent == bodyParts)
            {
                targetParts.parent = null;
                bodyParts.LinkExit(targetParts);
            }
            else
            {
                bodyParts.parent = null;
                targetParts.LinkExit(bodyParts);
            }*/

            bodyParts.LinkExit(targetParts);
            targetParts.LinkExit(bodyParts);
            target = null;
            connecting = false;

        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{
    private Transform Parent;
    private IParts iparts;
    private bool close;
    private IParts colIParts;
    private Transform DokObj;
    public MotorLink link;

    void Start()
    {
        Parent = transform.parent.transform;
        iparts = Parent.gameObject.GetComponent<IParts>();
        close = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!close && !other.gameObject.Equals(Parent.gameObject) && ((transform.tag == "Conn_Hole" && (other.tag == "Axle" || other.tag == "Connector")) || (transform.tag == "Axle_Hole" && other.tag == "Axle")))
        {
            DokObj = other.transform;
            iparts.Link(this.transform, DokObj);
            colIParts = DokObj.gameObject.GetComponent<IParts>();
            colIParts.Link(this.transform, Parent);
            close = true;

            if (iparts.OnDragCheck)
            {
                iparts.LinkMove(this.transform, DokObj);
            }
            else if (colIParts.OnDragCheck)
            {

                colIParts.LinkMove(this.transform, Parent);
            }

            link = transform.gameObject.GetComponent<MotorLink>();
            link.linkObject = this.gameObject;
            link.left = iparts;
            link.right = colIParts;

            if (this.tag == "Axle_Hole" && other.tag == "Axle")
            {
                link.type = MotorLink.LinkType.Tight;
            }

            else if(this.tag == "Conn_Hole" && other.tag == "Connector")
            {
                link.type = MotorLink.LinkType.Tight;
            }

            else if (this.tag == "Conn_Hole" && other.tag == "Axle")
            {
                link.type = MotorLink.LinkType.Loose;
            }

            link.left.node.AddLink(link);
            link.right.node.AddLink(link);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(close && other.gameObject.Equals(DokObj.gameObject))
        {
            iparts.LinkExit(this.transform, DokObj);
            colIParts.LinkExit(this.transform, Parent);
            close = false;
            DokObj = null;
            iparts.node.lList.Remove(link);
            colIParts.node.lList.Remove(link);
        }
    }
}

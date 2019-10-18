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
    public Transform parts;

    void Start()
    {
        Parent = transform.parent.transform;
        iparts = Parent.gameObject.GetComponent<IParts>();
        close = false;
    }

    public void HoleLink(Hole h)
    {
        close = true;
        iparts.Link(this.transform, DokObj);
        colIParts.Link(this.transform, Parent);

        link = transform.gameObject.GetComponent<MotorLink>();
        link.linkObject = this.gameObject;
        link.left = iparts;
        link.right = colIParts;

        Transform tmpObj;
        
        if (DokObj.childCount == 0)
        {
            tmpObj = DokObj;
        }
        else
        {
            tmpObj = parts;
        }

        if (this.tag == "Axle_Hole" && tmpObj.tag == "Axle")
        {
            link.type = MotorLink.LinkType.Tight;
        }

        else if (this.tag == "Conn_Hole" && tmpObj.tag == "Connector")
        {
            link.type = MotorLink.LinkType.Loose;
        }

        else if (this.tag == "Conn_Hole" && tmpObj.tag == "Axle")
        {
            link.type = MotorLink.LinkType.Loose;
        }
 
        link.left.node.AddLink(link);
        link.right.node.AddLink(link);

        if (h.gameObject.Equals(transform.gameObject))
        {
            MoveControl();
        }
    }

    void MoveControl()
    {
        if (iparts.OnDragCheck)
        {
            if (DokObj.childCount == 0)
            {
                iparts.LinkMove(this.transform, DokObj);
            }
            else
            {
                iparts.LinkMove(this.transform, parts);
            }
        }
        else if (colIParts.OnDragCheck)
        {
            if (DokObj.childCount == 0)
            {
                colIParts.LinkMove(this.transform, Parent);
            }
            else
            {
                colIParts.LinkMove(this.transform, parts);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!close && !other.gameObject.Equals(Parent.gameObject) && ((transform.tag == "Conn_Hole" && (other.tag == "Axle" || other.tag == "Connector")) || (transform.tag == "Axle_Hole" && other.tag == "Axle")))
        {
            if (iparts.Loaded)
            {
                if (other.transform.parent != null)
                {
                    DokObj = other.transform.parent;
                    parts = other.transform;
                }
                else
                {
                    DokObj = other.transform;
                }
                colIParts = DokObj.gameObject.GetComponent<IParts>();

                close = true;
                iparts.Link(this.transform, DokObj);
                colIParts.Link(this.transform, Parent);

                link = transform.gameObject.GetComponent<MotorLink>();
                link.linkObject = this.gameObject;
                link.left = iparts;
                link.right = colIParts;

                Transform tmpObj;

                if (DokObj.childCount == 0)
                {
                    tmpObj = DokObj;
                }
                else
                {
                    tmpObj = parts;
                }

                if (this.tag == "Axle_Hole" && tmpObj.tag == "Axle")
                {
                    link.type = MotorLink.LinkType.Tight;
                }

                else if (this.tag == "Conn_Hole" && tmpObj.tag == "Connector")
                {
                    link.type = MotorLink.LinkType.Loose;
                }

                else if (this.tag == "Conn_Hole" && tmpObj.tag == "Axle")
                {
                    link.type = MotorLink.LinkType.Loose;
                }

                link.left.node.AddLink(link);
                link.right.node.AddLink(link);

                return;
            }

            Vector3 cross = Vector3.Cross(other.transform.forward, transform.forward);
            if (cross == Vector3.zero)
            {
            
                if (other.transform.parent != null)
                {
                    DokObj = other.transform.parent;
                    parts = other.transform;
                }
                else
                {
                    DokObj = other.transform;
                }
                colIParts = DokObj.gameObject.GetComponent<IParts>();
                
                if (iparts.OnDragCheck)
                {
                    iparts.HoleInput(transform, DokObj);
                }
                else if (colIParts.OnDragCheck)
                {
                    colIParts.HoleInput(transform, Parent);
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        Transform otherTrans;

        if((other.tag == "Axle" || other.tag == "Connector"))
        {
            if (colIParts == null)
            {
                return;
            }

            if (other.transform.parent != null)
            {
                otherTrans = other.transform.parent;
            }
            else
            {
                otherTrans = other.transform;
            }
            
            if (iparts.OnDragCheck)
            {
                iparts.HoleOut(transform, DokObj);
            }
            else if (colIParts.OnDragCheck)
            {
                colIParts.HoleOut(transform, Parent);
            }

            if (close && otherTrans.gameObject.Equals(DokObj.gameObject))
            {
                iparts.LinkExit(this.transform, DokObj);
                colIParts.LinkExit(this.transform, Parent);
                iparts.node.lList.Remove(link);
                colIParts.node.lList.Remove(link);
                colIParts = null;
                DokObj = null;
                close = false;
            }
        }
    }
}

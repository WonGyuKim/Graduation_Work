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
    public bool Loaded;

    void Start()
    {
        Parent = transform.parent.transform;
        iparts = Parent.gameObject.GetComponent<IParts>();
        close = false;
        Loaded = iparts.Loaded;
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
                Vector3 cr = Vector3.Cross(other.transform.forward, transform.forward);
                //float vx = Mathf.Round(cr.x * 100f) / 100;
                //float vy = Mathf.Round(cr.y * 100f) / 100;
                //float vz = Mathf.Round(cr.z * 100f) / 100;

                //cr = new Vector3(vx, vy, vx);

                if (Mathf.Round(cr.x * 100f) / 100 == 0 && Mathf.Round(cr.y * 100f) / 100 == 0 && Mathf.Round(cr.z * 100f) / 100 == 0)
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
                    Loaded = false;
                    return;
                }
            }

            Vector3 cross = Vector3.Cross(other.transform.forward, transform.forward);

            if (Mathf.Round(cross.x * 100f) / 100 == 0 && Mathf.Round(cross.y * 100f) / 100 == 0 && Mathf.Round(cross.z * 100f) / 100 == 0)
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
        LinkCancel(other.gameObject);
    }

    public void LinkCancel(GameObject col)
    {
        Transform otherTrans;

        if ((col.tag == "Axle" || col.tag == "Connector"))
        {
            if (colIParts == null)
            {
                return;
            }

            if (col.transform.parent != null)
            {
                otherTrans = col.transform.parent;
            }
            else
            {
                otherTrans = col.transform;
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

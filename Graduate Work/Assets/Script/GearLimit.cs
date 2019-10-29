using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearLimit : MonoBehaviour
{
    IGear pnt;
    GearControl gc;
    public List<IGear> gList;

    void Start()
    {
        pnt = transform.parent.gameObject.GetComponent<IGear>();
        gc = GameObject.Find("snapControl").GetComponent<GearControl>();
        gList = new List<IGear>();
    }

    void OnTriggerEnter(Collider other)
    {
        if(!other.gameObject.Equals(pnt.gameObj) && (other.tag == "Gear" || other.tag == "BevelGear" || other.tag == "WormGear" || other.tag == "RackGear"))
        {
            IGear colG = other.gameObject.GetComponent<IGear>();
            gList.Add(colG);

            foreach(MotorLink lk in pnt.node.lList)
            {
                if(lk.left.gameObj.Equals(colG.gameObj))
                {
                    gc.deLinkGear(pnt, colG);
                    break;
                }
                else if(lk.right.gameObj.Equals(colG.gameObj))
                {
                    gc.deLinkGear(pnt, colG);
                    break;
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.Equals(pnt.gameObj) && (other.tag == "Gear" || other.tag == "BevelGear" || other.tag == "WormGear" || other.tag == "RackGear"))
        {
            IGear colG = other.gameObject.GetComponent<IGear>();
            if (gList.Remove(colG))
            {

            }
        }
    }
}

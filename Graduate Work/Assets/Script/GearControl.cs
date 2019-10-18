using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearControl : MonoBehaviour
{
    public List<IGear> gearList = new List<IGear>();

    public void AddGearList(IGear igear)
    {
        gearList.Add(igear);
    }

    public void linkGear(IGear gear1, IGear gear2)
    {
        foreach (MotorLink lk in gear1.node.lList)
        {
            if ((lk.left.gameObj.Equals(gear1.gameObj) && lk.right.gameObj.Equals(gear2.gameObj)) || (lk.left.gameObj.Equals(gear2.gameObj) && lk.right.gameObj.Equals(gear1.gameObj)))
            {
                return;
            }
        }

        GameObject gearLink = MonoBehaviour.Instantiate(Resources.Load("Models/Prefabs/GearLink") as GameObject, transform.position, Quaternion.identity) as GameObject;
        MotorLink link = gearLink.GetComponent<MotorLink>();

        link.left = gear1;
        link.right = gear2;
        link.linkObject = gearLink;
        GameObject g1 = gear1.gameObj;
        GameObject g2 = gear2.gameObj;

        Vector3 g1Vec = g1.transform.TransformDirection(g1.transform.forward);
        Vector3 g2Vec = g2.transform.TransformDirection(g2.transform.forward);
        Vector3 v = g1Vec - g2Vec;
        float angle = Mathf.Abs(Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg);

        if (g1.tag == "BevelGear" && g2.tag == "BevelGear" && angle >= 45)
        {
            link.type = MotorLink.LinkType.Bevel;
        }
        else if ((g1.tag == "WormGear" && (g2.tag == "Gear" || g2.tag == "BevelGear")) || ((g1.tag == "Gear" || g1.tag == "BevelGear") && g2.tag == "WormGear"))
        {
            link.type = MotorLink.LinkType.Worm;
        }
        else if ((g1.tag == "RackGear" && (g2.tag == "Gear" || g2.tag == "BevelGear")) || ((g1.tag == "Gear" || g1.tag == "BevelGear") && g2.tag == "RackGear"))
        {
            link.type = MotorLink.LinkType.Rack;
        }
        else if((g1.tag == "Gear" && g2.tag == "Gear" && angle < 45))
        {
            link.type = MotorLink.LinkType.Gear;
        }
        else
        {
            link.type = MotorLink.LinkType.None;
        }

        gear1.node.AddLink(link);
        gear2.node.AddLink(link);
    }

    public void deLinkGear(IGear gear1, IGear gear2)
    {
        foreach (MotorLink link1 in gear1.node.lList)
        {
            foreach (MotorLink link2 in gear2.node.lList)
            {
                if (link1 == link2)
                {
                    gear1.node.lList.Remove(link1);
                    gear2.node.lList.Remove(link2);
                    Destroy(link1.linkObject);
                    return;
                }
            }
        }
    }
}

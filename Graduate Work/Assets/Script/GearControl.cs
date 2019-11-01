﻿using System.Collections;
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
        GearLimit lim = gear1.gameObj.transform.GetChild(0).GetComponent<GearLimit>();
        foreach (IGear ge in lim.gList)
        {
            if (ge.gameObj.Equals(gear2.gameObj))
            {
                return;
            }
        }

        lim = gear2.gameObj.transform.GetChild(0).GetComponent<GearLimit>();
        foreach (IGear ge in lim.gList)
        {
            if (ge.gameObj.Equals(gear1.gameObj))
            {
                return;
            }
        }

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

        Vector3 g1Vec = /*g1.transform.TransformDirection*/(g1.transform.forward);
        Vector3 g2Vec = /*g2.transform.TransformDirection*/(g2.transform.forward);
        Vector3 v = g1Vec - g2Vec;
        //float angle = Mathf.Abs(Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg);
        //Debug.Log(g1.transform.forward.ToString() + " " + g2.transform.forward.ToString());
        //int angle1 = (int)Mathf.Round((Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg) * 100) / 100;
        //int angle2 = (int)Mathf.Round((Mathf.Atan2(v.z, v.x) * Mathf.Rad2Deg) * 100) / 100;
        //int angle3 = (int)Mathf.Round((Mathf.Atan2(v.y, v.z) * Mathf.Rad2Deg) * 100) / 100;

        //if (Mathf.Abs(angle1) > 90)
        //    angle1 = (Mathf.Abs(angle1) - 90) - 90;

        //if (Mathf.Abs(angle2) > 90)
        //    angle2 = (Mathf.Abs(angle2) - 90) - 90;

        //if (Mathf.Abs(angle3) > 90)
        //    angle3 = (Mathf.Abs(angle3) - 90) - 90;
        
        //Debug.Log("angle1 : " + angle1.ToString() + " angle2 : " + angle2.ToString() + " angle3 : " + angle3.ToString());
        Vector3 ag = Vector3.Cross(g1.gameObject.transform.forward, g2.gameObject.transform.forward);
        ag = new Vector3(Mathf.Round(ag.x * 100f) / 100f, (ag.y * 100f) / 100f, (ag.z * 100f) / 100f);
        bool isBevel = !(ag == Vector3.zero);

        if (g1.tag == "BevelGear" && g2.tag == "BevelGear")
        {
            if (isBevel)
                link.type = MotorLink.LinkType.Bevel;
            else
                link.type = MotorLink.LinkType.Gear;
        }
        else if ((g1.tag == "WormGear" && (g2.tag == "Gear" || g2.tag == "BevelGear")) || ((g1.tag == "Gear" || g1.tag == "BevelGear") && g2.tag == "WormGear") && isBevel)
        {
            link.type = MotorLink.LinkType.Worm;
        }
        else if ((g1.tag == "RackGear" && (g2.tag == "Gear" || g2.tag == "BevelGear")) || ((g1.tag == "Gear" || g1.tag == "BevelGear") && g2.tag == "RackGear") && !isBevel)
        {
            link.type = MotorLink.LinkType.Rack;
        }
        else if((g1.tag == "Gear" || g1.tag == "BevelGear") && (g2.tag == "Gear" || g2.tag == "BevelGear") && !(g1.tag == "BevelGear" && g2.tag == "BevelGear") && !isBevel)
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

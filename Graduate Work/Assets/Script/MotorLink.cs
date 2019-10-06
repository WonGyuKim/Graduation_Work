using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotorLink : MonoBehaviour
{
    public enum LinkType
    {
        None = 0,
        Tight,
        Loose,
        Gear,
        Bevel,
        Worm,
        Rack
    }
    public GameObject linkObject;

    public LinkType type;

    public IParts left;
    public IParts right;
}

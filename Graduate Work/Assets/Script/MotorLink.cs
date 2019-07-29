using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotorLink : MonoBehaviour
{
    public enum LinkType
    {
        Tight = 0,
        Loose,
        Gear,
        Bevel,
        Rack,
    }

    public LinkType type;

    public MotorNode left;
    public MotorNode right;
}

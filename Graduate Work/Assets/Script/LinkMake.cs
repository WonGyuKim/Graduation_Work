using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkMake : MonoBehaviour
{
    enum LinkType
    {
        Tight = 0,
        Loose,
        NormalGear,
        BevelGear,
        RackGear,
    }

    public class Node
    {
        GameObject parts;
        List<Link> lList;
    }

    public class Link
    {
        LinkType type;
        Node left;
        Node right;
    }
}

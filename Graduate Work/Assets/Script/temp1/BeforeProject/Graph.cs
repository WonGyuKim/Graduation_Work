using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

[Serializable]
public class Graph
{
    List<Parts> parts;

    Graph()
    {
        parts = new List<Parts>();
    }

    public void Add(Parts input)
    {
        parts.Add(input);
    }

    public List<Parts> GetParts()
    {
        return parts;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class Saver : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void saveGraph(List<Graph> graphs)
    {
        string Filename = "data.dat";
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Filename);
        
        bf.Serialize(file, graphs);

        file.Close();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    List<Graph> graphs;
    Saver saver;

    bool IsSave;
    // Start is called before the first frame update
    void Start()
    {
        graphs = new List<Graph>();
        saver = new Saver();
        IsSave = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsSave)
        {
            //saver.saveGraph(graphs);
            //foreach (Graph node in graphs)
            //{
            //    // saver.saveGraph(node);
            //}

            IsSave = false;
        }
    }
}

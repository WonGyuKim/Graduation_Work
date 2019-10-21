using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using MyObject;

public class builder : MonoBehaviour
{
	public GameObject[] ObjectPool;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(ObjectPool[0]);

        Vector3 move = ObjectPool[0].transform.position;
        
        move.x += 1;
        ObjectPool[1].transform.position = move;

        Instantiate(ObjectPool[1]);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void graphRepresent()
    {
        
    }
}

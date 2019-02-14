using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Create_Button : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] Pool;

    public void Action()
    {
        //Instantiate(Pool[0]);
        GameObject data = Instantiate(Resources.Load("\\Assets\\Models\\Prefabs\\15gear.prefab")) as GameObject;

    }
}

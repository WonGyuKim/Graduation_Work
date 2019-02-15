using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Create_Object : MonoBehaviour
{
    // Start is called before the first frame update
    //public GameObject[] Pool;
    private static int target = 0;

    public void Action()
    {
        // Instantiate(Pool[0]);
        string path = "Models/Prefabs/";
        UnityEngine.Object obj = Resources.Load(path + "15gear");
        Debug.Log("====================");
        Debug.Log(obj);
        Debug.Log(target);
        Debug.Log("====================");
        Instantiate((GameObject)obj);
        //GameObject data = Instantiate(Resources.Load(path + "15gear")) as GameObject;
    }

    public void DropDownList()
    {
        UnityEngine.UI.Dropdown dropdown = GetComponent<UnityEngine.UI.Dropdown>();
        target = dropdown.value;
        Debug.Log(target);
        Debug.Log(dropdown.value);
    }
}

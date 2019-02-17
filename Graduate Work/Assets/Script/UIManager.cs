using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    // UI Components
    BtnCreate BtnCreate;
    DDLCreate DDLCreate;

    // Variables
    private static int target;

    private void Start()
    {
        // Init UI Components
        BtnCreate = gameObject.AddComponent<BtnCreate>();
        DDLCreate = gameObject.AddComponent<DDLCreate>();

        // Init Variables
        //target = DDLCreate.Value;
        target = 0;
    }

    public void CreateObject()
    {
        BtnCreate.Action(target);
    }

    public void GetObjectValue()
    {
        target = DDLCreate.Value;
        Debug.Log(target);
    }
}

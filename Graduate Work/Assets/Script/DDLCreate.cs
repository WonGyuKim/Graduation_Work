using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DDLCreate : MonoBehaviour
{
    UnityEngine.UI.Dropdown dropdown;

    private void Start()
    {
        dropdown = GetComponent<UnityEngine.UI.Dropdown>();
    }

    public int Value
    {
        get
        {
            return dropdown.value;
        }

    }
}

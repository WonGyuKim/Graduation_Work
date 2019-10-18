using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll_Handle : MonoBehaviour
{
    private Vector3 transPos;
    public GameObject R_controller;
    public GameObject Handle;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Scroll");
    }

    public void OnMouseDrag()
    {
        transPos.x = R_controller.transform.position.x;
        transPos.y = 0;
        transPos.z = 0;

        Handle.GetComponent<RectTransform>().anchoredPosition = transPos;
    }
}

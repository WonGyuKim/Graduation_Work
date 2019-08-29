using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotate : MonoBehaviour
{
    public GameObject CurrentTouch;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hit);

            if (hit.collider != null)
            {
                CurrentTouch = hit.transform.gameObject;
            }
        }

        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            CurrentTouch.transform.Rotate(0, 90, 0, Space.World);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            CurrentTouch.transform.Rotate(0, -90, 0, Space.World);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            CurrentTouch.transform.Rotate(90, 0, 0, Space.World);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            CurrentTouch.transform.Rotate(-90, 0, 0, Space.World);
        }
    }
}

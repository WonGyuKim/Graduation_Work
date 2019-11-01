using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotate : MonoBehaviour
{
    public GameObject CurrentTouch;

    void Start()
    {
        CurrentTouch = null;
    }

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
            else
            {
                CurrentTouch = null;
            }
        }

        if(Input.GetKeyDown(KeyCode.LeftArrow) && CurrentTouch != null)
        {
            CurrentTouch.transform.Rotate(0, 90, 0, Space.World);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) && CurrentTouch != null)
        {
            CurrentTouch.transform.Rotate(0, -90, 0, Space.World);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) && CurrentTouch != null)
        {
            CurrentTouch.transform.Rotate(90, 0, 0, Space.World);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) && CurrentTouch != null)
        {
            CurrentTouch.transform.Rotate(-90, 0, 0, Space.World);
        }

        if (Input.GetKeyDown(KeyCode.D) && CurrentTouch != null)
        {
            IParts ip = CurrentTouch.GetComponent<IParts>();

            ip.ObjectDestroy();
        }
        if (Input.GetKeyDown(KeyCode.Z) && CurrentTouch != null)
        {
            CurrentTouch.transform.rotation = Quaternion.identity;
        }
    }
}

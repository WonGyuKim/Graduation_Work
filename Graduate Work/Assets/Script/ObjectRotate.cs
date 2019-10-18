using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotate : MonoBehaviour
{
    public GameObject CurrentTouch;
    public GameObject R_controller;
    private Ray ray = new Ray();
    private RaycastHit hit;

    void start()
    {
        R_controller = GameObject.Find("RightControllerAnchor");
    }

    void Update()
    {
        /*영빈 건드림
        ray.origin = R_controller.GetComponent<Transform>().position;
        ray.direction = R_controller.GetComponent<Transform>().forward;

        if (true == (Physics.Raycast(ray.origin, ray.direction, out hit)))
        {
            if (hit.collider != null)
            {
                CurrentTouch = hit.transform.gameObject;
            }

            if (OVRInput.Get(OVRInput.Touch.SecondaryThumbstick))
            {
                Vector2 coord = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);

                var absX = Mathf.Abs(coord.x);
                var absY = Mathf.Abs(coord.y);

                if (absX > absY)
                {
                    if(coord.x > 0)
                        CurrentTouch.transform.Rotate(0, -90, 0, Space.World);
                    else
                        CurrentTouch.transform.Rotate(0, 90, 0, Space.World);
                }
                else
                {
                    if(coord.y > 0)
                        CurrentTouch.transform.Rotate(90, 0, 0, Space.World);
                    else
                        CurrentTouch.transform.Rotate(-90, 0, 0, Space.World);
                }
            }
        }
        */

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
        
        
        //if(Input.GetKeyDown(KeyCode.D))
        //{
        //    Destroy(CurrentTouch);
        //}
        if(Input.GetKeyDown(KeyCode.Z))
        {
            CurrentTouch.transform.rotation = Quaternion.identity;
        }
    }
}

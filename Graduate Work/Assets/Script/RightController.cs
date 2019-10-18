using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightController : MonoBehaviour
{
    private GameObject target = null;
    private RaycastHit hit;
    private Ray ray = new Ray();
    public GameObject controller;
    public float ControllOrigin = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ray.origin = controller.GetComponent<Transform>().position;
        ray.direction = controller.GetComponent<Transform>().forward;

        //        if(OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
        if (true == (Physics.Raycast(ray.origin, ray.direction, out hit)))
        {
            target = hit.collider.gameObject;//problem is here

            Debug.Log(target);

            //if (true == (Physics.Raycast(ray.origin, ray.direction, out hit)))
            if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
            {                
                //target = hit.collider.gameObject;

                if(target.tag == "Create_Button")
                {
                    Debug.Log("Beam button hit");

                    GameObject.FindWithTag("Create_Button").GetComponent<UIManager>().CreateObject();
                }
            }
            else if (OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
            {
                ControllDrag();
                /*
                switch(true)
                {
                    case OVRInput.GetDown(OVRInput.Button.One):

                        break;
                }
                */
            }            
            else if(OVRInput.GetUp(OVRInput.Button.SecondaryIndexTrigger))
            {
                Debug.Log("Up");

                target = null;

                Debug.Log(target);
            }

            if (OVRInput.GetDown(OVRInput.Touch.SecondaryThumbstick))
            {
                Vector2 coord = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);

                var absX = Mathf.Abs(coord.x);
                var absY = Mathf.Abs(coord.y);

                if (absX > absY)
                {
                    if (coord.x > 0)
                        target.transform.Rotate(0, -90, 0, Space.World);
                    else
                        target.transform.Rotate(0, 90, 0, Space.World);
                }
                else
                {
                    if (coord.y > 0)
                        target.transform.Rotate(90, 0, 0, Space.World);
                    else
                        target.transform.Rotate(-90, 0, 0, Space.World);
                }
            }
        }
        //target = null;
    }

    public void ControllDrag()
    {
        switch (target.tag)
        {
            case "Axle":
                GameObject.FindWithTag("Axle").GetComponent<Axle>().OnMouseDrag();
                break;
            case "Menu_Scroll":
                Debug.Log("Scroll?");
                GameObject.FindWithTag("Menu_Scroll").GetComponent<Scroll_Handle>().OnMouseDrag();
                break;
        }

        target = null;
    }
}

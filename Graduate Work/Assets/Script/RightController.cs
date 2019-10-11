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
            target = hit.collider.gameObject;

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
            if (OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
            {
                ControllDrag();
            }
        }   
        /*
        else if(OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
        {
            
        }
        */
    }

    public void ControllDrag()
    {
        switch (target.tag)
        {
            case "Axle":
                GameObject.FindWithTag("Axle").GetComponent<Axle>().OnMouseDrag();
                break;
        }
    }
}

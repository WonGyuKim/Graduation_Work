﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour
{
    Vector3 origin;
    GameObject plane;
    Transform parent;

    // Start is called before the first frame update
    void Start()
    {
        origin = new Vector3();
        plane = GameObject.Find("Plane");
        parent = transform;

        if (parent.parent != null)
        {
            while (parent.parent != null)
                parent = parent.parent;
        }

        parent.position = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.transform.position.x + Screen.width / 2, Camera.main.transform.position.y + Screen.height / 2, plane.transform.position.z - Camera.main.transform.position.z));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            origin = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 5));

            origin.x -= transform.position.x;
            origin.y -= transform.position.y;
            origin.z -= transform.position.z;
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(90f, 0, 0);
        }
        else if (Input.GetKey(KeyCode.W))
        {
            transform.Rotate(0, 90f, 0);
        }
        else if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(0, 0, 90f);
        }
    }

    private void OnMouseDrag()
    {
        Vector3 click = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x - transform.position.x, Input.mousePosition.y - transform.position.y, 5 - transform.position.z));

        if (Input.GetKey(KeyCode.LeftControl))
        {

            // arcball rotation
            Debug.Log("Origin : (x, y, z) = (" + origin.x + ", " + origin.y + ", " + origin.z + ")");
            Debug.Log("Dynamic : (x, y, z) = (" + click.x + ", " + click.y + ", " + click.z + ")");


            // Step_1. 
            float V1 = Mathf.Sqrt(origin.x * origin.x + origin.y * origin.y + origin.z * origin.z);
            float V2 = Mathf.Sqrt(click.x * click.x + click.y * click.y + click.z * click.z);
            float Scale = V2 / V1;
            V1 *= Scale;


            // Step_2.
            Vector3 ScaledV1 = new Vector3(origin.x * Scale, origin.y * Scale, origin.z * Scale);
            //Inner Product
            float InPro = ScaledV1.x * click.x + ScaledV1.y * click.y + ScaledV1.z * click.z;
            InPro /= V1 * V2;
            float angle = Mathf.Acos(InPro);

            // Step_3.
            // Cross Product
            Vector3 CroPro = new Vector3(ScaledV1.y * click.z - ScaledV1.z * click.y,
                                            ScaledV1.z * click.x - ScaledV1.x * click.z,
                                            ScaledV1.x * click.y - ScaledV1.y * click.x);

            // Step_4. Now We can make conclusion Rotation by Quaternion
            Quaternion rotation = new Quaternion(CroPro.x, CroPro.y, CroPro.z, angle);
            //Debug.Log(rotation);
            parent.rotation = rotation;
           
        }

        else
        {
            parent.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, plane.transform.position.z - Camera.main.transform.position.z));
        }
            
        
    }

}

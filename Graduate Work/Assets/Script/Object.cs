using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour
{
    Vector3 origin;
    // Start is called before the first frame update
    void Start()
    {
        origin = new Vector3();

        if (transform.parent != null)
        {
            Transform parent = transform;

            while (parent.transform.parent != null)
            {
                parent = parent.transform.parent;
            }

            parent.position = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.transform.position.x + Screen.width / 2, Camera.main.transform.position.y + Screen.height / 2, 5));
        }
        else        
            transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.transform.position.x + Screen.width / 2, Camera.main.transform.position.y + Screen.height / 2, 5));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
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

    void OnMouseDrag()
    {
        Transform parent = transform;
        Vector3 click = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x - transform.position.x, Input.mousePosition.y - transform.position.y, 5 - transform.position.z));

        while (parent.transform.parent != null)
        {
            parent = parent.transform.parent;
        }

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
            Debug.Log(rotation);
            parent.transform.rotation = rotation;
            /*
            // arcball rotation
            Debug.Log("Origin : (x, y, z) = (" + origin.x + ", " + origin.y + ", " + origin.z + ")");
            Debug.Log("Dynamic : (x, y, z) = (" + parent.transform.position.x + ", " + parent.transform.position.y + ", " + parent.transform.position.z + ")");


            // Step_1. 
            float V1 = Mathf.Sqrt(origin.x * origin.x + origin.y * origin.y + origin.z * origin.z);
            float V2 = Mathf.Sqrt(parent.transform.position.x * parent.transform.position.x + parent.transform.position.y * parent.transform.position.y + parent.transform.position.z * parent.transform.position.z);
            float Scale = V2 / V1;
            V1 *= Scale;


            // Step_2.
            Vector3 ScaledV1 = new Vector3(origin.x * Scale, origin.y * Scale, origin.z * Scale);
            //Inner Product
            float InPro = ScaledV1.x * parent.transform.position.x + ScaledV1.y * parent.transform.position.y + ScaledV1.z * parent.transform.position.z;
            InPro /= V1 * V2;
            float angle = Mathf.Acos(InPro);
                
            // Step_3.
            // Cross Product
            Vector3 CroPro = new Vector3(ScaledV1.y * parent.transform.position.z - ScaledV1.z * parent.transform.position.y,
                                            ScaledV1.z * parent.transform.position.x - ScaledV1.x * parent.transform.position.z,
                                            ScaledV1.x * parent.transform.position.y - ScaledV1.y * parent.transform.position.x);

            // Step_4. Now We can make conclusion Rotation by Quaternion
            Quaternion rotation = new Quaternion(CroPro.x, CroPro.y, CroPro.z, angle);
            Debug.Log(rotation);
            parent.transform.rotation = rotation;
            */
        }

        else if (transform.parent != null)
        {
            parent.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 5));
        }
        else
            transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 5));

    }

    void OnMouseUp()
    {
    }

}

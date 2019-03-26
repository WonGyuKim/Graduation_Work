using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour
{
    Vector3 origin;
    GameObject plane;
    GameObject sphere;
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
            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayhit;
            if (Physics.Raycast(ray, out rayhit))
            {
                sphere = new GameObject("Sphere Collider");
                SphereCollider temp = sphere.AddComponent<SphereCollider>();
                origin = rayhit.point - parent.position;
                temp.radius = origin.magnitude;
                temp.transform.position = parent.transform.position;
                sphere.transform.parent = parent;
                sphere.transform.rotation = parent.rotation;

            }

            if (Physics.Raycast(ray, out rayhit) && rayhit.collider.gameObject.Equals(sphere))
            {
                origin = rayhit.point - parent.position;
            }

        }
        
    }

    private void OnMouseDrag()
    {

        if (Input.GetKey(KeyCode.LeftControl))
        {
            Vector3 click;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayhit;
            /*
            if (Physics.Raycast(ray, out rayhit) && rayhit.collider.gameObject.Equals(sphere))
            {
                click = rayhit.point - parent.position;
            }
            else
            {
                //click = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x - transform.position.x, Input.mousePosition.y - transform.position.y, plane.transform.position.z - transform.position.z));
                click = rayhit.point - parent.position;
            }
            */
            Physics.Raycast(ray, out rayhit);
            click = rayhit.point - parent.position;

            // arcball rotation
            // Step_1. 
            float Scale = click.magnitude / origin.magnitude;
            
            // Step_2.
            Vector3 ScaledV1 = new Vector3(origin.x * Scale, origin.y * Scale, origin.z * Scale);
            //Inner Product
            float InPro = ScaledV1.x * click.x + ScaledV1.y * click.y + ScaledV1.z * click.z;
            //InPro /= V1 * V2;
            float angle = Mathf.Acos(InPro);

            // Step_3.
            // Cross Product
            Vector3 CroPro = new Vector3(   ScaledV1.y * click.z - ScaledV1.z * click.y,
                                            ScaledV1.z * click.x - ScaledV1.x * click.z,
                                            ScaledV1.x * click.y - ScaledV1.y * click.x);

            // Step_4. Now We can make conclusion Rotation by Quaternion
            parent.rotation = new Quaternion(parent.rotation.x + CroPro.x * Mathf.Sin(angle / 2), parent.rotation.y + CroPro.y * Mathf.Sin(angle / 2), parent.rotation.z + CroPro.z * Mathf.Sin(angle / 2), parent.rotation.w + Mathf.Cos(angle));

        }

        else
        {
            parent.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, plane.transform.position.z - Camera.main.transform.position.z));
        }
            
        
    }

    private void OnMouseUp()
    {
        Destroy(sphere);
    }
}

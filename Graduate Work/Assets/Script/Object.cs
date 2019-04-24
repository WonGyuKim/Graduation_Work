using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour
{
    // kwg variable field
    
    Vector3 origin;
    GameObject plane;
    GameObject sphere;
    Quaternion temp_rotate;

    // kkh variable field
    Vector3 scrVec;
    private float speed;
    public bool OnDrag;
    public bool tEnter;
    public Vector3 befoMouse;
    

    // Start is called before the first frame update
    void Start()
    {
        OnDrag = false;
        tEnter = false;

        origin = new Vector3();
        plane = GameObject.Find("Plane");
        
        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.transform.position.x + Screen.width / 2, Camera.main.transform.position.y + Screen.height / 2, plane.transform.position.z - Camera.main.transform.position.z));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        scrVec = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        OnDrag = true;

        if (Input.GetKey(KeyCode.LeftControl))
        {
            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayhit;

            if (Physics.Raycast(ray, out rayhit))
            {
                float round = 0;
                float InPro, nearOrtho;
                Vector3 far = new Vector3(rayhit.point.x, rayhit.point.y, rayhit.point.z);
                Vector3 asspnt;

                MeshCollider mesh = rayhit.collider as MeshCollider;

                foreach (Vector3 vertex in mesh.sharedMesh.vertices)
                {
                    if (((transform.rotation * vertex + transform.position) - rayhit.point).magnitude > (far - rayhit.point).magnitude)
                    {
                        far = (transform.rotation * vertex + transform.position);
                    }
                }
                nearOrtho = far.x * far.x + far.y * far.y + far.z * far.z;
                foreach (Vector3 vertex in mesh.sharedMesh.vertices)
                {
                    asspnt = (transform.rotation * vertex + transform.position);
                    InPro = far.x * asspnt.x + far.y * asspnt.y + far.z * asspnt.z;
                    if (round < Mathf.Sqrt((far - rayhit.point).magnitude * (far - rayhit.point).magnitude + (asspnt - rayhit.point).magnitude * (asspnt - rayhit.point).magnitude) && Mathf.Abs(InPro) < Mathf.Abs(nearOrtho))
                    {
                        round = Mathf.Sqrt((far - rayhit.point).magnitude * (far - rayhit.point).magnitude + (asspnt - rayhit.point).magnitude * (asspnt - rayhit.point).magnitude);
                        nearOrtho = InPro;
                    }
                }

                sphere = new GameObject("Sphere Collider");
                sphere.transform.position = transform.position;
                sphere.transform.parent = transform;
                sphere.transform.rotation = transform.rotation;

                SphereCollider temp = sphere.AddComponent<SphereCollider>();
                temp.radius = round / 2;
                temp_rotate = transform.rotation;

                


                //MeshCollider mesh = rayhit.collider as MeshCollider;
                //origin = rayhit.point - transform.position;

                //sphere = new GameObject("Sphere Collider");
                //sphere.transform.position = transform.position;
                //sphere.transform.parent = transform;
                //sphere.transform.rotation = transform.rotation;

                //SphereCollider temp = sphere.AddComponent<SphereCollider>();
                //temp.radius = origin.magnitude;
                //temp_rotate = transform.rotation;

                ///*foreach (Vector3 vertex in mesh.sharedMesh.vertices)
                //    Debug.Log(vertex);*/
            }

            if (Physics.Raycast(ray, out rayhit) && rayhit.collider.gameObject.Equals(sphere))
            {
                origin = rayhit.point - transform.position;
            }

        }
        
    }

    private void OnMouseDrag()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            ArcballRotation();
        }

        else if (tEnter)
        {
            VerticalMove();
        }

        else
        {
            transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x - scrVec.x, Input.mousePosition.y - scrVec.y, plane.transform.position.z - Camera.main.transform.position.z));
        }
            
        
    }

    private void OnMouseUp()
    {
        Destroy(sphere);
        OnDrag = false;
    }

    private void ArcballRotation()
    {
        Vector3 click;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit rayhit;

        if (Physics.Raycast(ray, out rayhit) && rayhit.collider.gameObject.Equals(sphere))
        {
            click = rayhit.point - transform.position;

            // arcball rotation
            // Step_1. 
            //float Scale = click.magnitude / origin.magnitude;
            click = Vector3.Normalize(click);
            origin = Vector3.Normalize(origin);

            // Step_2.
            //Inner Product
            float InPro = origin.x * click.x + origin.y * click.y + origin.z * click.z;
            //InPro /= V1 * V2;
            float angle = Mathf.Acos(InPro) / 2;

            // Step_3.
            // Cross Product
            Vector3 CroPro = new Vector3(
                origin.y * click.z - origin.z * click.y,
                origin.z * click.x - origin.x * click.z,
                origin.x * click.y - origin.y * click.x);

            // Step_4. Now We can make conclusion Rotation by Quaternion
            Quaternion result = new Quaternion(Mathf.Sin(angle) * CroPro.x, Mathf.Sin(angle) * CroPro.y, Mathf.Sin(angle) * CroPro.z, Mathf.Cos(angle));
            transform.rotation = result * temp_rotate;

        }
    }

    void VerticalMove()
    {
        Vector3 vec = Input.mousePosition - befoMouse;
        Vector3 forW = (Camera.main.WorldToScreenPoint(transform.position) - Camera.main.WorldToScreenPoint(transform.position + transform.forward)).normalized;
        speed = Vector3.Dot(forW, vec);
        if (speed > 10)
            speed = 10;
        if (speed < -10)
            speed = -10;
        transform.position -= transform.forward * (speed / 100f);
        befoMouse = Input.mousePosition;
    }
}

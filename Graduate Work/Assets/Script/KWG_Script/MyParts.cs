using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MyParts : MonoBehaviour
{
    private Vector3 origin;
    private GameObject plane;
    private GameObject sphere;
    private Quaternion temp_rotate;

    private bool onDrag;
    private bool tEnter;
    private float speed;
    private Vector3 scrSpace;
    private Vector3 befoMouse;
    private float xf;
    private float yf;
    private Vector3 dst;
    private Vector3 Vec;

    public bool search; //탐색 확인 변수
    public GameObject emptyObject;//프리팹에서 empty오브젝트를 받아올 변수
    public GameObject Parent;//부모 개체
    public List<GameObject> AllList;//연결된 모든 파츠 리스트

    protected List<MyParts> LinkParts;

    void Start()
    {
        origin = new Vector3();
        plane = GameObject.Find("Plane");
        LinkParts = new List<MyParts>();

        scrSpace = Camera.main.WorldToScreenPoint(transform.position);
        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.transform.position.x + Screen.width / 2, Camera.main.transform.position.y + Screen.height / 2, scrSpace.z));
        onDrag = false;
        tEnter = false;
        emptyObject = Resources.Load("Models/Prefabs/Parent") as GameObject;
        search = false;
        
    }

    private void MouseMove()
    {
        scrSpace = Camera.main.WorldToScreenPoint(transform.position);
        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x - xf, Input.mousePosition.y - yf, scrSpace.z));
    }

    public void VerticalMove()
    {
        Vector3 vec = Input.mousePosition - befoMouse;
        Vector3 forW = (Camera.main.WorldToScreenPoint(transform.position) - Camera.main.WorldToScreenPoint(transform.position + transform.forward)).normalized;
        speed = Vector3.Dot(forW, vec);
        if (speed > 0.01f)
            speed = 0.01f;
        if (speed < -0.01f)
            speed = -0.01f;
        transform.position -= transform.forward * speed;
        befoMouse = Input.mousePosition;
    }

    private void SetArcballData()
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
        }

        if (Physics.Raycast(ray, out rayhit) && rayhit.collider.gameObject.Equals(sphere))
        {
            origin = rayhit.point - transform.position;
        }
    }

    private void ArcballMove()
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

    public bool OnDragCheck
    {
        get
        {
            return onDrag;
        }
    }

    public abstract void LinkRotation(float F, float V);

    void OnMouseDown()
    {
        scrSpace = Camera.main.WorldToScreenPoint(transform.position);
        xf = Input.mousePosition.x - scrSpace.x;
        yf = Input.mousePosition.y - scrSpace.y;
        onDrag = true;

        if (Input.GetKey(KeyCode.A))//A키를 누른 상태에서 마우스 클릭
        {
            //AllList = LinkSearch();
            Parent = MonoBehaviour.Instantiate(emptyObject, transform.position, Quaternion.identity) as GameObject;
            foreach (GameObject gobj in AllList)
            {
                gobj.transform.parent = Parent.transform;
            }
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            SetArcballData();
        }
    }

    void OnMouseDrag()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            ArcballMove();
        }
        else if (Input.GetKey(KeyCode.A))
        {
            //LinkAllMove();
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            foreach (GameObject gobj in AllList)
            {
                gobj.transform.parent = null;
                gobj.GetComponent<IParts>().SearchReset();
            }
            AllList.Clear();
            Destroy(Parent);
        }
        else
        {
            if (tEnter)
                VerticalMove();
            else
                MouseMove();
        }
    }

    void OnMouseUp()
    {
        onDrag = false;
        if (Input.GetKey(KeyCode.A))
        {
            foreach (GameObject gobj in AllList)
            {
                gobj.transform.parent = null;
                gobj.GetComponent<IParts>().SearchReset();
            }
            AllList.Clear();
            Destroy(Parent);
        }
        Destroy(sphere);
    }

    // if gear is entered... //
    
    public void SearchReset()
    {
        search = false;
    }
}

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
   
    protected GameObject parentComponent;
    protected List<Link> LinkParts;

    void Start()
    {
        origin = new Vector3();
        plane = GameObject.Find("Plane");
        LinkParts = new List<Link>();
        parentComponent = null;

        scrSpace = Camera.main.WorldToScreenPoint(transform.position);
        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.transform.position.x + Screen.width / 2, Camera.main.transform.position.y + Screen.height / 2, scrSpace.z));
        onDrag = false;
        tEnter = false;

        StartOverride();
    }

    protected abstract void StartOverride();

    void Update()
    {

    }

    public void SetMoveData()
    {
        scrSpace = Camera.main.WorldToScreenPoint(transform.position);
        xf = Input.mousePosition.x - scrSpace.x;
        yf = Input.mousePosition.y - scrSpace.y;
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
            temp.radius = round / 2.6f;
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
        get { return onDrag; }
    }
    /*
    public MyParts parent
    {
        get { return parentComponent; }
        set
        {
            parentComponent = value;
            Debug.Log(parentComponent);
            if (value == null)
            {
                parentComponent = null;
                //transform.parent = null;
                if (LinkParts.Count == 0 && parentComponent == null)
                    tEnter = false;
            }
            else
            {
                parentComponent = value;
                //transform.parent = value.transform;
                transform.position = value.transform.position;
                transform.rotation = new Quaternion(value.transform.rotation.x, value.transform.rotation.y, value.transform.rotation.z, value.transform.rotation.w);
                tEnter = true;
                
            }
        }
    }
    */
    public List<Link> child
    {
        get { return LinkParts; }
    }
    
    public void Link(Link input)
    {
        LinkParts.Add(input);
        tEnter = true;
    }

    public void LinkExit(Link input)
    {
        LinkParts.Remove(input);
        if (LinkParts.Count == 0)
            tEnter = false;
    }
    
    public abstract void LinkRotation(MyParts parent, PowerData power);

    public void SetLinkMove(Link parent, GameObject head)
    {
        transform.parent = head.transform;

        foreach(Link link in LinkParts)
        {
            if (!link.Equals(parent))
                link.SetLinkMove(this, head);
        }
    }

    public void LinkMove(MyParts parent)
    {
        scrSpace = Camera.main.WorldToScreenPoint(parentComponent.transform.position);
        parentComponent.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x - xf, Input.mousePosition.y - yf, scrSpace.z));
    }
    
    public void QuitLinkMove(Link parent)
    {
        transform.parent = null;

        foreach(Link link in LinkParts)
        {
            if (!link.Equals(parent))
                link.QuitLinkMove(this);
        }
    }
    
    void OnMouseDown()
    {
        SetMoveData();

        if (Input.GetKey(KeyCode.A))
        {
            parentComponent = new GameObject();
            parentComponent.name = "parent";
            parentComponent.transform.position = transform.position;
            SetLinkMove(null, parentComponent);
        }
        else if (Input.GetKey(KeyCode.LeftControl))
            SetArcballData();
    }
    
    void OnMouseDrag()
    {
        onDrag = true;

        // GetKey Section
        {
            if (Input.GetKey(KeyCode.LeftControl))
                ArcballMove();
            else if (Input.GetKey(KeyCode.A) && parentComponent != null)
                LinkMove(this);
            else if (tEnter)
                VerticalMove();
            else
                MouseMove();
        }

        // GetKeyUp Section
        {
            if (Input.GetKeyUp(KeyCode.A) && parentComponent != null)
            {
                QuitLinkMove(null);
                Destroy(parentComponent);
            }
        }

    }

    void OnMouseUp()
    {
        onDrag = false;
        if (Input.GetKey(KeyCode.A) && parentComponent!= null)
        {
            QuitLinkMove(null);
            Destroy(parentComponent);
        }
        Destroy(sphere);
    }

    // if gear is entered... //
    
}

public class PowerData
{
    private float force;        // force for movement
    private float velocity;     // velocity for movement
    private bool RD;            // Rotation Direction of component
                                // if RD == true, then rotate Clock Wise
                                // if RD == false, then rotate Counter Clock Wise

    private Vector3 AngVel;     // Angular Velocity of Parent Component

    public PowerData()
    {
        force = 1;
        velocity = 1 / force;
        RD = true;
        AngVel = new Vector3(0, 0, 1);
    }
    public PowerData(float force, float velocity, bool RD)
    {
        this.force = force;
        this.velocity = velocity;
        this.RD = RD;
    }
    public PowerData(float force, float velocity, Vector3 AngVel, bool RD)
    {
        this.force = force;
        this.velocity = velocity;
        this.AngVel = AngVel;
        this.RD = RD;
    }

    public float Force
    {
        get { return force; }
        set { force = value; }
    }

    public float Velocity
    {
        get { return velocity; }
        set { velocity = value; }
    }

    public bool RotationDirection
    {
        get { return RD; }
        set { RD = value; }
    }

    public Vector3 AngularVelocity
    {
        get { return AngVel; }
        set { AngVel = value; }
    }
}
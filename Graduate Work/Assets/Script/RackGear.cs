using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RackGear : MonoBehaviour, IGear
{
    private bool onDrag;
    private bool tEnter;
    private float speed;
    private Vector3 scrSpace;
    private Vector3 befoMouse;
    private float xf;
    private float yf;
    private List<GameObject> LinkParts;
    private Vector3 dst;
    private Vector3 Vec;
    private Vector3 origin;
    private GameObject sphere;
    private Quaternion temp_rotate;
    public bool search; //탐색 확인 변수
    public GameObject emptyObject;//프리팹에서 empty오브젝트를 받아올 변수
    public GameObject Parent;//부모 개체
    public List<GameObject> AllList;//연결된 모든 파츠 리스트
    private MotorNode Node;
    public GearControl gearControl;
    public RotateMotor rotM;
    public Transform hole;
    public List<Transform> holeList;
    public List<Transform> otherList;
    public float dis;
    public float rad;
    private Vector3 point;
    private Vector3 axis;
    private float moveSpeed;
    private int moveType;
    private string kind;
    private bool loaded;
    public List<MoveCell> moveList;
    public Vector3 lastPos;
    public Vector3 moveDir;

    public void HoleInput(Transform hole, Transform other)
    {
        holeList.Add(hole);
        otherList.Add(other);
        //this.hole = hole;
    }
    public void HoleOut(Transform hole, Transform other)
    {
        if (holeList.Remove(hole))
        {

        }
        if (otherList.Remove(other))
        {

        }
    }
    public string Kind
    {
        get { return kind; }
        set { kind = value; }
    }
    public bool Loaded
    {
        set { loaded = value; }
        get { return loaded; }
    }

    void Start()
    {
        scrSpace = Camera.main.WorldToScreenPoint(transform.position);
        if (!loaded)
            transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.transform.position.x + Screen.width / 2, Camera.main.transform.position.y + Screen.height / 2, scrSpace.z));
        LinkParts = new List<GameObject>();
        holeList = new List<Transform>();
        otherList = new List<Transform>();
        moveList = new List<MoveCell>();
        onDrag = false;
        tEnter = false;
        emptyObject = Resources.Load("Models/Prefabs/Parent") as GameObject;
        search = false;
        Node = transform.gameObject.GetComponent<MotorNode>();
        Node.parts = this;
        gearControl = GameObject.Find("snapControl").GetComponent<GearControl>();
        gearControl.AddGearList(this);
        rotM = GameObject.Find("RotateControl").GetComponent<RotateMotor>();
        hole = null;
        dis = int.MaxValue;
        rad = transform.gameObject.GetComponent<Renderer>().bounds.size.x;
        lastPos = transform.position;
        moveDir = Vector3.zero;
        ResetValue();
    }

    public void Link(Transform hole, Transform otherTrans)
    {
        LinkParts.Add(otherTrans.gameObject);
        LinkParts = LinkParts.Distinct().ToList();
        tEnter = true;
    }

    public void LinkMove(Transform hole, Transform otherTrans)
    {
        //this.transform.rotation = otherTrans.rotation;
        befoMouse = Input.mousePosition;
        dst = otherTrans.position - hole.position;
        Vector3 zAxis = otherTrans.forward;
        zAxis = Vector3.Project(dst, zAxis);
        this.transform.position = this.transform.position - zAxis + dst;
        scrSpace = Camera.main.WorldToScreenPoint(transform.position);
        xf = Input.mousePosition.x - scrSpace.x;
        yf = Input.mousePosition.y - scrSpace.y;
    }

    public void LinkExit(Transform hole, Transform otherTrans)
    {
        LinkParts.Remove(otherTrans.gameObject);
        if (LinkParts.Count == 0)
        {
            tEnter = false;
        }
    }

    public void VerticalMove()
    {
        //scrSpace = Camera.main.WorldToScreenPoint(transform.position);
        //Vector3 vec = Input.mousePosition - befoMouse;
        //Vector3 forW = (Camera.main.WorldToScreenPoint(transform.position) - Camera.main.WorldToScreenPoint(transform.position + transform.forward)).normalized;
        //speed = Vector3.Dot(forW, vec);
        //speed /= 300f;
        //transform.position -= transform.forward * speed;
        befoMouse = Input.mousePosition;

        Vector3 camDis = Camera.main.transform.position - transform.position;
        float cm = Mathf.Sqrt(camDis.x * camDis.x + camDis.y * camDis.y + camDis.z * camDis.z);

        float x = Input.mousePosition.x - scrSpace.x;
        float y = Input.mousePosition.y - scrSpace.y;

        float r = Mathf.Abs(Mathf.Sqrt(xf * xf + yf * yf) - Mathf.Sqrt(x * x + y * y));
        if (r > 150 / cm)
        {
            transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x - xf, Input.mousePosition.y - yf, scrSpace.z));
            tEnter = false;
        }
    }

    public void MouseMove()
    {
        scrSpace = Camera.main.WorldToScreenPoint(transform.position);
        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x - xf, Input.mousePosition.y - yf, scrSpace.z));
    }

    public void ArcballMove()
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

    public void MotoringMove(Vector3 point, Vector3 axis, float speed, float rad, int moveType, Motor motor)
    {
        if (!search)
        {
            search = true;
            rotM.nodeList.Add(Node);
            foreach (MotorLink link in Node.lList)
            {
                IParts lparts;
                if (link.left.gameObj == this.gameObj)
                    lparts = link.right;
                else
                    lparts = link.left;

                if (link.type == MotorLink.LinkType.Tight)
                {
                    lparts.MotoringMove(point, axis, speed, 0, moveType, motor);
                }
                else if (link.type == MotorLink.LinkType.Loose)
                {
                    lparts.MotoringMove(point, axis, speed, 0, moveType, motor);
                }
                else if (link.type == MotorLink.LinkType.Gear)
                {
                    //float ratio = rad / this.rad;
                    //speed *= ratio;
                    //rad = this.rad;
                    lparts.MotoringMove(lparts.gameObj.transform.position, lparts.gameObj.transform.forward, -speed, rad, 0, motor);
                }
                else if (link.type == MotorLink.LinkType.Bevel)
                {
                    //float ratio = rad / this.rad;
                    //speed *= ratio;
                    //rad = this.rad;
                    lparts.MotoringMove(lparts.gameObj.transform.position, lparts.gameObj.transform.forward, -speed, rad, 0, motor);
                }
                else if (link.type == MotorLink.LinkType.Worm)
                {
                    lparts.MotoringMove(lparts.gameObj.transform.position, lparts.gameObj.transform.forward, -speed, rad, 0, motor);
                }
                else if (link.type == MotorLink.LinkType.Rack)
                {
                    lparts.MotoringMove(lparts.gameObj.transform.position, lparts.gameObj.transform.forward, -speed, rad, 0, motor);
                }
            }
           
            moveList.Add(new MoveCell(point, axis, speed, moveType, motor));
        }
    }

    public void MotorRotate()
    {
        foreach (MoveCell cell in moveList)
        {
            if (cell.MoveType == 0)
            {
                transform.RotateAround(cell.Point, cell.Axis, cell.MoveSpeed);

                int count = 0;
                GameObject obj = null;

                foreach (MotorLink link in Node.lList)
                {
                    if (link.type == MotorLink.LinkType.Tight)
                    {
                        return;
                    }
                    if (link.type == MotorLink.LinkType.Loose)
                    {
                        count++;
                        if (this.gameObject.Equals(link.right.gameObj))
                        {
                            obj = link.left.gameObj;
                        }
                        else
                        {
                            obj = link.right.gameObj;
                        }
                    }
                }
                if (count == 1)
                {
                    transform.RotateAround(obj.transform.position, obj.transform.forward, -cell.MoveSpeed);
                }
            }
            else
            {
                transform.Translate(cell.Axis, Space.World);
                moveDir = transform.position - lastPos;
                lastPos = transform.position;
            }
        }

        //if (this.moveType == 0)
        //{
        //    transform.RotateAround(point, axis, moveSpeed);
        //    int count = 0;
        //    GameObject obj = null;

        //    foreach (MotorLink link in Node.lList)
        //    {
        //        if (link.type == MotorLink.LinkType.Tight)
        //        {
        //            return;
        //        }
        //        if (link.type == MotorLink.LinkType.Loose)
        //        {
        //            count++;
        //            if (this.gameObject.Equals(link.right.gameObj))
        //            {
        //                obj = link.left.gameObj;
        //            }
        //            else
        //            {
        //                obj = link.right.gameObj;
        //            }
        //        }
        //    }
        //    if (count == 1)
        //    {
        //        transform.RotateAround(obj.transform.position, obj.transform.forward, -moveSpeed);
        //    }
        //}
        //else
        //{
        //    transform.Translate(axis, Space.World);
        //}
    }

    public void ResetValue()
    {
        moveList.Clear();
    }

    public bool OnDragCheck
    {
        get
        {
            return onDrag;
        }
    }

    public GameObject gameObj
    {
        get
        {
            return this.gameObject;
        }
    }

    void OnMouseDown()
    {
        scrSpace = Camera.main.WorldToScreenPoint(transform.position);
        xf = Input.mousePosition.x - scrSpace.x;
        yf = Input.mousePosition.y - scrSpace.y;
        onDrag = true;
        befoMouse = Input.mousePosition;
        loaded = false;
        //if (Input.GetKey(KeyCode.LeftControl))
        //{

        //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //    RaycastHit rayhit;

        //    if (Physics.Raycast(ray, out rayhit))
        //    {
        //        float round = 0;
        //        float InPro, nearOrtho;
        //        Vector3 far = new Vector3(rayhit.point.x, rayhit.point.y, rayhit.point.z);
        //        Vector3 asspnt;

        //        MeshCollider mesh = rayhit.collider as MeshCollider;

        //        foreach (Vector3 vertex in mesh.sharedMesh.vertices)
        //        {
        //            if (((transform.rotation * vertex + transform.position) - rayhit.point).magnitude > (far - rayhit.point).magnitude)
        //            {
        //                far = (transform.rotation * vertex + transform.position);
        //            }
        //        }
        //        nearOrtho = far.x * far.x + far.y * far.y + far.z * far.z;
        //        foreach (Vector3 vertex in mesh.sharedMesh.vertices)
        //        {
        //            asspnt = (transform.rotation * vertex + transform.position);
        //            InPro = far.x * asspnt.x + far.y * asspnt.y + far.z * asspnt.z;
        //            if (round < Mathf.Sqrt((far - rayhit.point).magnitude * (far - rayhit.point).magnitude + (asspnt - rayhit.point).magnitude * (asspnt - rayhit.point).magnitude) && Mathf.Abs(InPro) < Mathf.Abs(nearOrtho))
        //            {
        //                round = Mathf.Sqrt((far - rayhit.point).magnitude * (far - rayhit.point).magnitude + (asspnt - rayhit.point).magnitude * (asspnt - rayhit.point).magnitude);
        //                nearOrtho = InPro;
        //            }
        //        }

        //        sphere = new GameObject("Sphere Collider");
        //        sphere.transform.position = transform.position;
        //        sphere.transform.parent = transform;
        //        sphere.transform.rotation = transform.rotation;

        //        SphereCollider temp = sphere.AddComponent<SphereCollider>();
        //        temp.radius = round / 2;
        //        temp_rotate = transform.rotation;




        //        //MeshCollider mesh = rayhit.collider as MeshCollider;
        //        //origin = rayhit.point - transform.position;

        //        //sphere = new GameObject("Sphere Collider");
        //        //sphere.transform.position = transform.position;
        //        //sphere.transform.parent = transform;
        //        //sphere.transform.rotation = transform.rotation;

        //        //SphereCollider temp = sphere.AddComponent<SphereCollider>();
        //        //temp.radius = origin.magnitude;
        //        //temp_rotate = transform.rotation;

        //        ///*foreach (Vector3 vertex in mesh.sharedMesh.vertices)
        //        //    Debug.Log(vertex);*/
        //    }

        //    if (Physics.Raycast(ray, out rayhit) && rayhit.collider.gameObject.Equals(sphere))
        //    {
        //        origin = rayhit.point - transform.position;
        //    }
        //}

        if (Input.GetKey(KeyCode.A))//A키를 누른 상태에서 마우스 클릭
        {
            AllList = LinkSearch();
            Parent = MonoBehaviour.Instantiate(emptyObject, transform.position, Quaternion.identity) as GameObject;
            foreach (GameObject gobj in AllList)
            {
                gobj.transform.parent = Parent.transform;
            }
        }
    }

    public List<GameObject> LinkSearch()//연결된 파츠들 탐색
    {
        List<GameObject> list = new List<GameObject>();
        if (!search)//탐색이 안된 상태
        {
            list.Add(transform.gameObject);//자신을 리스트에 추가
            search = true;//탐색 완료
            foreach (GameObject gobj in LinkParts)//자신과 연결된 파츠들 탐색
            {
                IParts Ip = gobj.GetComponent<IParts>();//IParts 컴포넌트 불러오기
                list.AddRange(Ip.LinkSearch());//연결된 파츠와 연결된 파츠들을 LinkSearch 재귀로 불러오고 자신의 리스트와 합침
            }
        }
        return list;
    }

    public void LinkAllMove()
    {
        scrSpace = Camera.main.WorldToScreenPoint(Parent.transform.position);
        Parent.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x - xf, Input.mousePosition.y - yf, scrSpace.z));
    }

    public MotorNode node
    {
        get
        {
            return Node;
        }

        set
        {
            Node = value;
        }
    }

    void OnMouseUp()
    {
        if (otherList.Count != 0 && holeList.Count != 0)
        {
            Transform other = null;
            for (int i = 0; i < otherList.Count; i++)
            {
                Vector3 Dis;
                Vector3 zAxis;
                float tmpDis;

                Dis = otherList[i].position - holeList[i].position;
                zAxis = Vector3.Project(Dis, otherList[i].forward);
                Dis = Dis - zAxis;
                tmpDis = Mathf.Sqrt(Dis.x * Dis.x + Dis.y * Dis.y + Dis.z * Dis.z);
                if (tmpDis <= dis)
                {
                    dis = tmpDis;
                    hole = holeList[i];
                    other = otherList[i];
                }
            }

            Hole h = hole.gameObject.GetComponent<Hole>();

            h.HoleLink(h);
            holeList.Remove(hole);
            otherList.Remove(other);
            for (int i = 0; i < otherList.Count; i++)
            {
                Vector3 Dis;
                Vector3 zAxis;
                float tmpDis;

                Dis = otherList[i].position - holeList[i].position;
                zAxis = Vector3.Project(Dis, otherList[i].forward);
                Dis = Dis - zAxis;
                tmpDis = Mathf.Sqrt(Dis.x * Dis.x + Dis.y * Dis.y + Dis.z * Dis.z);

                if ((Mathf.Abs(dis - tmpDis)) < 0.05f)
                {
                    Hole newHo = holeList[i].gameObject.GetComponent<Hole>();
                    newHo.HoleLink(h);
                }
            }
            holeList.Clear();
            otherList.Clear();
            dis = int.MaxValue;
        }

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
        hole = null;
        onDrag = false;
    }

    void OnMouseDrag()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            //ArcballMove();
            float rotate_spd = 300.0f;
            float temp_x_axis = Input.GetAxis("Mouse X") * rotate_spd * Time.deltaTime;
            float temp_y_axis = Input.GetAxis("Mouse Y") * rotate_spd * Time.deltaTime;
            transform.Rotate(temp_y_axis, -temp_x_axis, 0, Space.World);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            LinkAllMove();
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

    public void GearMove()
    {
        //Vector3 vec = Input.mousePosition - befoMouse;
        //Quaternion qua = Quaternion.AngleAxis(90, linkGear.transform.TransformDirection(linkGear.transform.forward));
        //Vector3 Vev = qua * Vec;
        //speed = Vector3.Dot(Vev, vec);
        //if (speed > 10)
        //    speed = 10;
        //if (speed < -10)
        //    speed = -10;
        //transform.RotateAround(linkGear.transform.position, transform.TransformDirection(transform.forward), speed);
        //Vec = transform.position - linkGear.transform.position;
        //befoMouse = Input.mousePosition;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Gear" || other.tag == "BevelGear" || other.tag == "WormGear" || other.tag == "RackGear")
        {
            LinkParts.Add(other.gameObject);

            if (loaded)
            {
                foreach (MotorLink lk in Node.lList)
                {
                    GameObject g;

                    if (lk.left.gameObj.Equals(this.gameObject))
                    {
                        g = lk.right.gameObj;
                    }
                    else
                    {
                        g = lk.left.gameObj;
                    }

                    if (g.Equals(other.gameObject))
                    {
                        return;
                    }
                }
                IGear linkGear = other.transform.gameObject.GetComponent<IGear>();
                gearControl.linkGear(this, linkGear);
            }

            if (onDrag)
            {
                IGear linkGear = other.transform.gameObject.GetComponent<IGear>();
                gearControl.linkGear(this, linkGear);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Gear" || other.tag == "BevelGear" || other.tag == "WormGear" || other.tag == "RackGear")
        {
            LinkParts.Remove(other.gameObject);
            if (onDrag)
            {
                IGear linkGear = other.transform.gameObject.GetComponent<IGear>();
                gearControl.deLinkGear(this, linkGear);
            }
        }
    }

    public void SearchReset()
    {
        search = false;
    }

    void OnMouseEnter()
    {
        if (this.Loaded)
        {
            Loaded = false;
            AllList = LinkSearch();

            foreach (GameObject g in AllList)
            {
                IParts ip = g.GetComponent<IParts>();
                ip.Loaded = false;
                ip.SearchReset();
            }
            AllList.Clear();
        }
    }

    public void ObjectDestroy()
    {

    }
}

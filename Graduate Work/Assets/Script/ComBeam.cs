﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ComBeam : MonoBehaviour, IParts
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
    private Vector3 origin;
    private GameObject sphere;
    private Quaternion temp_rotate;
    public bool search; //탐색 확인 변수
    public GameObject emptyObject;//프리팹에서 empty오브젝트를 받아올 변수
    public GameObject Parent;//부모 개체
    public List<GameObject> AllList;//연결된 모든 파츠 리스트
    private MotorNode Node;
    public RotateMotor rotM;
    public Transform hole;
    public List<Transform> holeList;
    public List<Transform> otherList;
    public float dis;
    private Vector3 point;
    private Vector3 axis;
    private float moveSpeed;
    private int moveType;
    private string kind;
    private bool loaded;
    public MoveCell cell;

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
        onDrag = false;
        tEnter = false;
        emptyObject = Resources.Load("Models/Prefabs/Parent") as GameObject;
        search = false;
        Node = transform.gameObject.GetComponent<MotorNode>();
        Node.parts = this;
        rotM = GameObject.Find("RotateControl").GetComponent<RotateMotor>();
        hole = null;
        dis = int.MaxValue;
        cell = new MoveCell();
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
        //float xR = hole.eulerAngles.x - otherTrans.eulerAngles.x;
        //float yR = hole.eulerAngles.y - otherTrans.eulerAngles.y;
        //float zR = hole.eulerAngles.z - otherTrans.eulerAngles.z;

        //this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x + xR, this.transform.eulerAngles.y + yR, this.transform.eulerAngles.z);
        befoMouse = Input.mousePosition;
        //dst = otherTrans.position - hole.position;
        //Vector3 zAxis = otherTrans.forward;
        //zAxis = Vector3.Project(dst, zAxis);
        //this.transform.position = this.transform.position - zAxis + dst;
        if(otherTrans.parent != null && this.transform.gameObject.Equals(otherTrans.parent.gameObject))
        {
            Vector3 dis = hole.position - otherTrans.position;
            this.transform.position = this.transform.position + dis;
        }
        else
        {
            dst = otherTrans.position - hole.position;
            Vector3 zAxis = otherTrans.forward;
            zAxis = Vector3.Project(dst, zAxis);
            this.transform.position = this.transform.position - zAxis + dst;
        }
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
        scrSpace = Camera.main.WorldToScreenPoint(transform.position);
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
    
    void SetArcballData()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayhit;

            if (Physics.Raycast(ray, out rayhit))
            {
                float round = 0;
                Vector3 far = new Vector3(transform.position.x, transform.position.y, transform.position.z);

                MeshCollider mesh = rayhit.collider as MeshCollider;

                foreach (Vector3 vertex in mesh.sharedMesh.vertices)
                {
                    if (Mathf.Pow(transform.position.x - far.x, 2) + Mathf.Pow(transform.position.y - far.y, 2) + Mathf.Pow(transform.position.z - far.z, 2)
                        < Mathf.Pow(transform.position.x - vertex.x, 2) + Mathf.Pow(transform.position.y - vertex.y, 2) + Mathf.Pow(transform.position.z - vertex.z, 2))
                    {
                        far.x = vertex.x;
                        far.y = vertex.y;
                        far.z = vertex.z;
                    }
                }

                round = far.magnitude;

                sphere = new GameObject("Sphere Collider");
                sphere.transform.position = transform.position;
                sphere.transform.parent = transform;
                sphere.transform.rotation = transform.rotation;

                SphereCollider temp = sphere.AddComponent<SphereCollider>();
                temp.radius = round;
                temp_rotate = transform.rotation;
            }

            if (Physics.Raycast(ray, out rayhit) && rayhit.collider.gameObject.Equals(sphere))
            {
                origin = rayhit.point - transform.position;
            }
        }
    }


    public void ArcballMove()
    {
        Vector3 click, click_N, origin_N;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit rayhit;

        if (Physics.Raycast(ray, out rayhit) && rayhit.collider.gameObject.Equals(sphere))
        {
            click = rayhit.point - transform.position;

            // arcball rotation

            // Step_1. 
            click_N = Vector3.Normalize(click);
            origin_N = Vector3.Normalize(origin);

            // Step_2.
            //Inner Product : For getting an angle between two vectors
            float InPro = Vector3.Dot(origin_N, click_N);
            float angle = Mathf.Acos(InPro) / 2;

            // Step_3.
            // Cross Product : For getting rotation axis
            Vector3 CroPro = Vector3.Cross(origin_N, click_N);

            // Step_4. Now We can make conclusion Rotation by Quaternion
            Quaternion result = new Quaternion(Mathf.Sin(angle) * CroPro.x, Mathf.Sin(angle) * CroPro.y, Mathf.Sin(angle) * CroPro.z, Mathf.Cos(angle));

            sphere.transform.rotation = result * temp_rotate;
            transform.rotation = result * temp_rotate;
            //result.z = 0;
            //Debug.Log("x : " + Mathf.Abs(result.x));
            //Debug.Log("y : " + Mathf.Abs(result.y));
            //Debug.Log("=====");
            //if (Mathf.Abs(result.x) >= Mathf.Abs(result.y))
            //{
            //    // y-axis rotation
            //    result.y = 0;
            //    sphere.transform.rotation = result * temp_rotate;
            //    if (Mathf.Abs(sphere.transform.rotation.eulerAngles.x) % 90 < ArcballRange)
            //        transform.rotation = Quaternion.Euler(sphere.transform.rotation.eulerAngles.x, Mathf.Floor(sphere.transform.rotation.eulerAngles.y / 90) * 90, sphere.transform.rotation.eulerAngles.z);
            //    else if (90 - ArcballRange < Mathf.Abs(sphere.transform.rotation.eulerAngles.x) % 90)
            //        transform.rotation = Quaternion.Euler(sphere.transform.rotation.eulerAngles.x, (Mathf.Floor(sphere.transform.rotation.eulerAngles.y / 90) + 1) * 90, sphere.transform.rotation.eulerAngles.z);
            //    else
            //        transform.rotation = result * temp_rotate;
            //    //Debug.Log("HI");
            //}
            //else if (Mathf.Abs(result.x) < Mathf.Abs(result.y))
            //{
            //    // x-axis rotation
            //    result.x = 0;
            //    sphere.transform.rotation = result * temp_rotate;
            //    if (Mathf.Abs(sphere.transform.rotation.eulerAngles.y) % 90 < ArcballRange)
            //        transform.rotation = Quaternion.Euler(Mathf.Floor(sphere.transform.rotation.eulerAngles.x / 90) * 90, sphere.transform.rotation.eulerAngles.y, sphere.transform.rotation.eulerAngles.z);
            //    else if (90 - ArcballRange < Mathf.Abs(sphere.transform.rotation.eulerAngles.y) % 90)
            //        transform.rotation = Quaternion.Euler((Mathf.Floor(sphere.transform.rotation.eulerAngles.x / 90) + 1) * 90, sphere.transform.rotation.eulerAngles.y, sphere.transform.rotation.eulerAngles.z);
            //    else
            //        transform.rotation = result * temp_rotate;

            //    //Debug.Log("Chilly");
            //}

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

                if (lparts.Search)
                    continue;

                if (link.type == MotorLink.LinkType.Tight)
                {
                    lparts.MotoringMove(point, axis, speed, 0, moveType, motor);
                }
                else if (link.type == MotorLink.LinkType.Loose)
                {
                    lparts.MotoringMove(point, axis, speed, 0, moveType, motor);
                }
            }
            cell.Point = point;
            cell.Axis = axis;
            cell.MoveSpeed = speed;
            cell.MoveType = moveType;
            cell.Motor = motor;
            //moveList.Add(new MoveCell(point, axis, speed, moveType, motor));
        }
    }

    public void MotorRotate()
    {
        if (cell.MoveType == 0)
        {
            transform.RotateAround(cell.Point, cell.Axis, cell.MoveSpeed);

        }
        else
        {
            transform.Translate(cell.Axis, Space.World);
        }
        //foreach (MoveCell cell in moveList)
        //{
        //    if (cell.MoveType == 0)
        //    {
        //        transform.RotateAround(cell.Point, cell.Axis, cell.MoveSpeed);

        //    }
        //    else
        //    {
        //        transform.Translate(cell.Axis, Space.World);
        //    }
        //}

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

    void OnMouseDown()
    {
        if (Input.GetKey(KeyCode.LeftAlt))
            return;

        scrSpace = Camera.main.WorldToScreenPoint(transform.position);
        xf = Input.mousePosition.x - scrSpace.x;
        yf = Input.mousePosition.y - scrSpace.y;
        onDrag = true;
        befoMouse = Input.mousePosition;
        loaded = false;
        if (Input.GetKey(KeyCode.LeftControl))
            SetArcballData();
        if (Input.GetKey(KeyCode.A))
        {
            AllList = LinkSearch();
            Parent = MonoBehaviour.Instantiate(emptyObject, transform.position, Quaternion.identity) as GameObject;
            foreach (GameObject gobj in AllList)
            {
                gobj.transform.parent = Parent.transform;
            }
        }
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

    public bool Search
    {
        get
        {
            return this.search;
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

                if ((Mathf.Abs(dis - tmpDis)) < 0.02f)
                {
                    Hole newHo = holeList[i].gameObject.GetComponent<Hole>();
                    newHo.HoleLink(h);
                }
            }
            holeList.Clear();
            otherList.Clear();
            dis = int.MaxValue;
            Destroy(sphere);
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
        if (Input.GetKey(KeyCode.LeftAlt))
            return;

        if (Input.GetKey(KeyCode.LeftControl))
        {
            ArcballMove();
            //float rotate_spd = 300.0f;
            //float temp_x_axis = Input.GetAxis("Mouse X") * rotate_spd * Time.deltaTime;
            //float temp_y_axis = Input.GetAxis("Mouse Y") * rotate_spd * Time.deltaTime;
            //transform.Rotate(temp_y_axis, -temp_x_axis, 0, Space.World);
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
        //GameObject hole;

        //for (int i = Node.lList.Count - 1; i >= 0; i--)
        //{
        //    hole = Node.lList[i].linkObject;
        //    Hole h = hole.GetComponent<Hole>();

        //    MotorLink l = hole.GetComponent<MotorLink>();

        //    if (l.left.gameObj.Equals(this.gameObject))
        //    {
        //        h.LinkCancel(l.right.gameObj);
        //    }
        //    else
        //    {
        //        h.LinkCancel(l.left.gameObj);
        //    }
        //}

        if (Node.lList.Count == 0)
        {
            UIManager ui = GameObject.Find("UI Manager").GetComponent<UIManager>();
            if (ui.list.Remove(this.gameObject))
            {
                ui.data = null;
                Destroy(this.gameObject);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Motor : MonoBehaviour, IParts
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
    public Transform scroll;
    public GameObject motorText;
    public float RotateSpeed;
    public bool on;
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
        rotM.motorList.Add(this);
        hole = null;
        dis = int.MaxValue;
        scroll = GameObject.Find("Scroll View").transform.GetChild(0).GetChild(0);
        motorText = Resources.Load("Models/Prefabs/MotorList") as GameObject;
        motorText = MonoBehaviour.Instantiate(motorText, transform.position, Quaternion.identity) as GameObject;
        motorText.transform.parent = scroll;
        RotateSpeed = 5f;
        on = true;
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
        scrSpace = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 vec = Input.mousePosition - befoMouse;
        Vector3 forW = (Camera.main.WorldToScreenPoint(transform.position) - Camera.main.WorldToScreenPoint(transform.position + transform.forward)).normalized;
        speed = Vector3.Dot(forW, vec);
        speed /= 500f;
        transform.position -= transform.forward * speed;
        befoMouse = Input.mousePosition;

        Vector3 camDis = Camera.main.transform.position - transform.position;
        float cm = Mathf.Sqrt(camDis.x * camDis.x + camDis.y * camDis.y + camDis.z * camDis.z);

        float x = Input.mousePosition.x - scrSpace.x;
        float y = Input.mousePosition.y - scrSpace.y;

        float r = Mathf.Abs(Mathf.Sqrt(xf * xf + yf * yf) - Mathf.Sqrt(x * x + y * y));
        if (r > 230 / cm)
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
        //    //Debug.Log("Motor : " + point.ToString() + " " + axis.ToString() + " " + moveSpeed.ToString());
        //    transform.RotateAround(point, axis, moveSpeed);
        //}
        //else
        //{
        //    transform.Translate(axis);
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
        scrSpace = Camera.main.WorldToScreenPoint(transform.position);
        xf = Input.mousePosition.x - scrSpace.x;
        yf = Input.mousePosition.y - scrSpace.y;
        onDrag = true;
        befoMouse = Input.mousePosition;
        loaded = false;
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
            
            if (ui.list.Remove(this.gameObject) && rotM.motorList.Remove(this))
            {
                ui.data = null;
                Destroy(motorText);
                Destroy(this.gameObject);
            }
        }
    }
}
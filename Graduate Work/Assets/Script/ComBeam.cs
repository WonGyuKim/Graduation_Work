using System.Collections;
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
    private List<GameObject> LinkParts = new List<GameObject>();
    private Vector3 dst;

    public bool search; //탐색 확인 변수
    public GameObject emptyObject;//프리팹에서 empty오브젝트를 받아올 변수
    public GameObject Parent;//부모 개체
    public List<GameObject> AllList;//연결된 모든 파츠 리스트
    private MotorNode Node;
    public RotateMotor rotM;
    public Transform hole;
    public List<Transform> holeList = new List<Transform>();
    public List<Transform> otherList = new List<Transform>();
    public float dis;

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

    void Start()
    {
        scrSpace = Camera.main.WorldToScreenPoint(transform.position);
        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.transform.position.x + Screen.width / 2, Camera.main.transform.position.y + Screen.height / 2, scrSpace.z));
        onDrag = false;
        tEnter = false;
        emptyObject = Resources.Load("Models/Prefabs/Parent") as GameObject;
        search = false;
        Node = transform.gameObject.GetComponent<MotorNode>();
        Node.parts = this;
        rotM = GameObject.Find("RotateControl").GetComponent<RotateMotor>();
        hole = null;
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
        //befoMouse = Input.mousePosition;
        //dst = otherTrans.position - hole.position;
        //Vector3 zAxis = otherTrans.forward;
        //zAxis = Vector3.Project(dst, zAxis);
        //this.transform.position = this.transform.position - zAxis + dst;
        Vector3 dis = hole.position - otherTrans.position;
        this.transform.position = this.transform.position + dis;
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

        float x = Input.mousePosition.x - scrSpace.x;
        float y = Input.mousePosition.y - scrSpace.y;

        float r = Mathf.Abs(Mathf.Sqrt(xf * xf + yf * yf) - Mathf.Sqrt(x * x + y * y));
        if (r > 50)
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

    public void MotoringMove(Vector3 point, Vector3 axis, float speed)
    {
        if (!search)
        {
            transform.RotateAround(point, axis, speed);
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
                    lparts.MotoringMove(point, axis, speed);
                }
                else if (link.type == MotorLink.LinkType.Loose)
                {
                    lparts.MotoringMove(point, axis, speed);
                }
            }
        }
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

    void OnMouseUp()
    {
        if (otherList.Count != 0 && holeList.Count != 0)
        {
            for (int i = 0; i < otherList.Count; i++)
            {
                Vector3 Dis;
                Vector3 zAxis;
                float tmpDis;

                if (i == 0)
                {
                    Dis = otherList[0].position - holeList[0].position;
                    zAxis = Vector3.Project(Dis, otherList[0].forward);
                    Dis = Dis - zAxis;
                    tmpDis = Mathf.Sqrt(Dis.x * Dis.x + Dis.y * Dis.y + Dis.z * Dis.z);
                    dis = tmpDis;
                    hole = holeList[0];
                    continue;
                }
                else
                {
                    Dis = otherList[i].position - holeList[i].position;
                    zAxis = Vector3.Project(Dis, otherList[i].forward);
                    Dis = Dis - zAxis;
                    tmpDis = Mathf.Sqrt(Dis.x * Dis.x + Dis.y * Dis.y + Dis.z * Dis.z);
                    if (tmpDis <= dis)
                    {
                        dis = tmpDis;
                        hole = holeList[i];
                    }
                }

            }

            Hole h = hole.gameObject.GetComponent<Hole>();

            h.HoleLink();
            holeList.Clear();
            otherList.Clear();
            dis = 0;
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

    public void SearchReset()
    {
        search = false;
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Axle : MonoBehaviour, IParts
{
    private bool onDrag;
    private bool tEnter;
    private float speed;
    private Vector3 scrSpace;
    private Vector3 befoMouse;
    private float xf;
    private float yf;
    private List<GameObject> LinkParts = new List<GameObject>();
    public bool search; //탐색 확인 변수
    public GameObject emptyObject;//프리팹에서 empty오브젝트를 받아올 변수
    public GameObject Parent;//부모 개체
    public List<GameObject> AllList;//연결된 모든 파츠 리스트
    private MotorNode Node;
    public RotateMotor rotM;
    public Transform hole;
    public List<Transform> holeList = new List<Transform>();
    public float dis;
    private Vector3 point;
    private Vector3 axis;
    private float moveSpeed;
    private int moveType;
    private string kind;
    private bool loaded;

    public void HoleInput(Transform hole, Transform other)
    {
        holeList.Add(hole);
        //this.hole = hole;
    }
    public void HoleOut(Transform hole, Transform other)
    {
        if(holeList.Remove(hole))
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
        
        onDrag = false;
        tEnter = false;
        emptyObject = Resources.Load("Models/Prefabs/Parent") as GameObject;
        search = false;
        Node = transform.gameObject.GetComponent<MotorNode>();
        Node.parts = this;
        rotM = GameObject.Find("RotateControl").GetComponent<RotateMotor>();
        hole = null;
        dis = int.MaxValue;
        ResetValue();
    }

    //void Update()
    //{
    //    Debug.Log(holeList.Count);
    //}

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

    public void Link(Transform hole, Transform otherTrans)
    {
        tEnter = true;
        LinkParts.Add(otherTrans.gameObject);
        LinkParts = LinkParts.Distinct().ToList();
    }

    public void LinkMove(Transform hole, Transform otherTrans)
    {
        Vector3 zAxis = transform.forward;
        Vector3 dis = hole.position - transform.position;
        zAxis = Vector3.Project(dis, zAxis);
        this.transform.position = this.transform.position + (dis - zAxis);
        befoMouse = Input.mousePosition;
        scrSpace = Camera.main.WorldToScreenPoint(transform.position);
        xf = Input.mousePosition.x - scrSpace.x;
        yf = Input.mousePosition.y - scrSpace.y;
    }

    public void LinkExit(Transform hole, Transform otherTrans)
    {
        LinkParts.Remove(otherTrans.gameObject);
        scrSpace = Camera.main.WorldToScreenPoint(transform.position);
        xf = Input.mousePosition.x - scrSpace.x;
        yf = Input.mousePosition.y - scrSpace.y;
        if (LinkParts.Count == 0)
        {
            tEnter = false;
        }
    }

    public void VerticalMove()
    {
        Vector3 cDis = Camera.main.transform.position - transform.position;
        float dis = Mathf.Sqrt(cDis.x * cDis.x + cDis.y * cDis.y + cDis.z * cDis.z);
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

    public void MotoringMove(Vector3 point, Vector3 axis, float speed, float rad, int moveType)
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
                    if (moveType == 1)
                    {
                        lparts.MotoringMove(point, axis, speed, rad, 1);
                    }
                    else
                    {
                        lparts.MotoringMove(point, axis, speed, rad, moveType);
                    }
                }
                else if (link.type == MotorLink.LinkType.Loose)
                {
                    if (moveType == 1)
                    {
                        lparts.MotoringMove(point, axis, speed, rad, 1);
                    }
                    else
                    {
                        Vector3 tVector = (transform.position - point);
                        if (tVector == Vector3.zero)
                        {
                            tVector = transform.forward;
                        }
                        tVector = tVector.normalized;

                        if (Mathf.Round(Mathf.Abs(axis.x) * 1000f) != Mathf.Round(Mathf.Abs(tVector.x) * 1000f)
                             || Mathf.Round(Mathf.Abs(axis.y) * 1000f) != Mathf.Round(Mathf.Abs(tVector.y) * 1000f)
                             || Mathf.Round(Mathf.Abs(axis.z) * 1000f) != Mathf.Round(Mathf.Abs(tVector.z) * 1000f))
                        {
                            lparts.MotoringMove(point, axis, speed, rad, moveType);
                        }
                    }
                }
            }
            this.point = point;
            this.axis = axis;
            this.moveSpeed = speed;
            this.moveType = moveType;
        }
    }

    public void MotorRotate()
    {
        if (this.moveType == 0)
        {
            //Debug.Log("Axle : " + point.ToString() + " " + axis.ToString() + " " + moveSpeed.ToString());
            transform.RotateAround(point, axis, moveSpeed);
        }
        else
        {
            transform.Translate(axis, Space.World);
        }
    }

    public void ResetValue()
    {
        point = Vector3.zero;
        axis = Vector3.zero;
        moveSpeed = 0;
        moveType = 0;
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

    public void LinkAllMove()
    {
        scrSpace = Camera.main.WorldToScreenPoint(Parent.transform.position);
        Parent.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x - xf, Input.mousePosition.y - yf, scrSpace.z));
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

    void OnMouseUp()
    {
        if (holeList.Count != 0)
        {
            for(int i = 0; i < holeList.Count; i++)
            {
                Vector3 Dis;
                Vector3 zAxis;
                float tmpDis;

                Dis = holeList[i].position - transform.position;
                zAxis = Vector3.Project(Dis, transform.forward);
                Dis = Dis - zAxis;
                tmpDis = Mathf.Sqrt(Dis.x * Dis.x + Dis.y * Dis.y + Dis.z * Dis.z);
                if (tmpDis < dis)
                {
                    dis = tmpDis;
                    hole = holeList[i];
                }
            }

            Hole h = hole.gameObject.GetComponent<Hole>();

            h.HoleLink(h);
            holeList.Remove(hole);
            foreach(Transform ho in holeList)
            {
                Vector3 Dis;
                Vector3 zAxis;
                float tmpDis;

                Dis = ho.position - transform.position;
                zAxis = Vector3.Project(Dis, transform.forward);
                Dis = Dis - zAxis;
                tmpDis = Mathf.Sqrt(Dis.x * Dis.x + Dis.y * Dis.y + Dis.z * Dis.z);
                if((Mathf.Abs(dis - tmpDis)) < 0.05f)
                {
                    Hole newHo = ho.gameObject.GetComponent<Hole>();
                    newHo.HoleLink(h);
                }
            }
            holeList.Clear();
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
            {
                VerticalMove();
            }
            else
            {
                MouseMove();
            }
        }
    }

    public void SearchReset()
    {
        search = false;
    }
}

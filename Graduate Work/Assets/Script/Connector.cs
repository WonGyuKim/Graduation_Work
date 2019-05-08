using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connector : MonoBehaviour, IParts
{
    private bool onDrag;
    private bool tEnter;
    private float speed;
    private Vector3 scrSpace;
    private Vector3 befoMouse;
    private float xf;
    private float yf;
    private List<GameObject> LinkParts = new List<GameObject>();

    void Start()
    {
        scrSpace = Camera.main.WorldToScreenPoint(transform.position);
        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.transform.position.x + Screen.width / 2, Camera.main.transform.position.y + Screen.height / 2, scrSpace.z));
        onDrag = false;
        tEnter = false;
    }

    public void Link(Transform hole, Transform otherTrans)
    {
        LinkParts.Add(otherTrans.gameObject);
        tEnter = true;
    }

    public void LinkMove(Transform hole, Transform otherTrans)
    {
        this.transform.position = hole.position;
        this.transform.rotation = hole.rotation;
        befoMouse = Input.mousePosition;
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

    public void MouseMove()
    {
        scrSpace = Camera.main.WorldToScreenPoint(transform.position);
        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x - xf, Input.mousePosition.y - yf, scrSpace.z));
    }

    public void ArcballMove()
    {
        
    }

    public void MotoringMove()
    {

    }

    public bool OnDragCheck
    {
        get
        {
            return onDrag;
        }
    }

    void OnMouseDown()
    {
        scrSpace = Camera.main.WorldToScreenPoint(transform.position);
        xf = Input.mousePosition.x - scrSpace.x;
        yf = Input.mousePosition.y - scrSpace.y;
        onDrag = true;
    }

    void OnMouseUp()
    {
        onDrag = false;
    }

    void OnMouseDrag()
    {
        if (tEnter)
            VerticalMove();
        else
            MouseMove();
    }
}
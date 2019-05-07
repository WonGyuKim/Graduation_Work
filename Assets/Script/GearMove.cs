using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearMove : MonoBehaviour
{
    private Vector3 scrSpace;
    public Vector3 befoMouse;
    private float xf;
    private float yf;
    public bool OnDrag;
    public bool tEnter;
    private Vector3 Vec;
    private Collider col;
    private float speed;
    private Vector3 crossV;

    void Start()
    {
        scrSpace = Camera.main.WorldToScreenPoint(transform.position);
        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.transform.position.x + Screen.width / 2, Camera.main.transform.position.y + Screen.height / 2, scrSpace.z));

        OnDrag = false;
        tEnter = false;
    }

    void OnMouseDown()
    {
        scrSpace = Camera.main.WorldToScreenPoint(transform.position);
        xf = Input.mousePosition.x - scrSpace.x;
        yf = Input.mousePosition.y - scrSpace.y;
        OnDrag = true;
    }
    void OnMouseUp()
    {
        OnDrag = false;
    }

    void Update()
    {
        //if(tEnter)
        //{
        //    transform.RotateAround(col.transform.position, transform.TransformDirection(transform.forward), 0.3f);
        //}
    }

    void OnMouseDrag()
    {
        OnDrag = true;
        if (tEnter && OnDrag)
        {
            Vector3 vec = Input.mousePosition - befoMouse;
            Quaternion qua = Quaternion.AngleAxis(90, col.transform.TransformDirection(col.transform.forward));
            Vector3 Vev = qua * Vec;
            speed = Vector3.Dot(Vev, vec);
            if (speed > 10)
                speed = 10;
            if (speed < -10)
                speed = -10;
            transform.RotateAround(col.transform.position, transform.TransformDirection(transform.forward), speed);
            Vec = transform.position - col.transform.position;
            befoMouse = Input.mousePosition;
        }
        else
        {
            scrSpace = Camera.main.WorldToScreenPoint(transform.position);
            transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x - xf, Input.mousePosition.y - yf, scrSpace.z));
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Gear")
        {
            tEnter = true;
            Vec = transform.position - other.transform.position;
            col = other;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Gear")
        {
            tEnter = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other == col)
        {
            tEnter = false;
        }
    }
}

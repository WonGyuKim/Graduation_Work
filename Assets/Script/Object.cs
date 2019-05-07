using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Object : MonoBehaviour
{
    private Vector3 scrSpace;
    public Vector3 befoMouse;
    private float xf;
    private float yf;
    public bool OnDrag;
    public bool tEnter;
    private float speed;
    
    void Start()
    {
        //scrSpace 오브젝트의 스크린좌표를 계속 업데이트
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

    void OnMouseDrag()
    {
        ObjectMove();
    }

    void ObjectMove()
    {
        OnDrag = true;

        if (tEnter)
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
        else
        {
            scrSpace = Camera.main.WorldToScreenPoint(transform.position);
            transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x - xf, Input.mousePosition.y - yf, scrSpace.z));
        }
    }

    void OnMouseUp()
    {
        OnDrag = false;
    }

    //void OnTriggerEnter(Collider other)
    //{
    //    if (OnDrag)
    //    {
    //        Collide col = GameObject.Find(other.name).GetComponent<Collide>();
    //        if (!col.close)
    //        {
    //            if (this.tag == "Axle")
    //            {
    //                if (other.tag == "Conn_Hole" || other.tag == "Axle_Hole")
    //                {
    //                    transform.position = other.transform.position;
    //                    transform.rotation = other.transform.rotation;
    //                    tEnter = true;
    //                    befoMouse = Input.mousePosition;
    //                }
    //            }
    //            else if (this.tag == "Connector")
    //            {
    //                if (other.tag == "Conn_Hole")
    //                {
    //                    transform.position = other.transform.position;
    //                    transform.rotation = other.transform.rotation;
    //                    tEnter = true;
    //                    befoMouse = Input.mousePosition;
    //                }
    //            }
    //        }
    //    }
    //}

    //void OnTriggerExit(Collider other)
    //{
    //    tEnter = false;
    //}
}
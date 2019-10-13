using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class __2Dcollider_Axle : MonoBehaviour
{
    private GameObject target = null;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            if (true == (Physics.Raycast(_ray.origin, _ray.direction * 10, out hit)))
            {
                if (true == (Physics.Raycast(_ray.origin, _ray.direction * 10, out hit)))
                {
                    target = hit.collider.gameObject;
                }

                RayLook(target, _ray, hit);
            }
        }
    }

    public void RayLook(GameObject clicked_one, Ray ray, RaycastHit hit)
    {
        GameObject hitten_one = null;

        //Vector3 hittenPlace = null;

        if (true == (Physics.Raycast(target.transform.position, ray.direction * 10, out hit)))
        {
            //target.GetComponent<Renderer>().material.color = Color.blue;

            hitten_one = hit.collider.gameObject;

            if (hitten_one.transform.tag == "Conn_Hole" || hitten_one.transform.tag == "Axle_Hole")
            {
                //Debug.Log("NOPE");
                Debug.DrawRay(ray.origin, ray.direction * 10f, Color.green, 0.5f);

                //target.transform.position = hitten_one.transform.position;
                target.transform.position = hit.point;
            }
        }
    }
    /*
    void OnMouseUp()
    {
        target.GetComponent<Renderer>().material.color = Color.white;
    }*/
}
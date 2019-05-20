using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    public float speed = 6f;
    public float distance;
    public Transform target;
    private Vector2 input;
    private Vector3 befomouse;
    private bool altIn;
    private float zoom;

    void Start()
    {
        distance = (transform.position - target.position).magnitude;
        altIn = false;
        target.position = transform.position + transform.forward * distance;
        zoom = 2f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            altIn = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            altIn = false;
        }
        if (Input.GetMouseButton(0) && altIn)
        {
            input += new Vector2(Input.GetAxis("Mouse X") * speed, Input.GetAxis("Mouse Y") * -speed);

            transform.rotation = Quaternion.Euler(input.y, input.x, 0);
            transform.position = target.position - (transform.localRotation * Vector3.forward * distance);
        }
        else if (Input.GetMouseButton(2))
        {
            transform.position -= (Input.GetAxis("Mouse X") * transform.right + Input.GetAxis("Mouse Y") * transform.up) / 3f;
            target.position -= (Input.GetAxis("Mouse X") * transform.right + Input.GetAxis("Mouse Y") * transform.up) / 3f;
        }
    }

    void LateUpdate()
    {
        Vector3 TargetDist = transform.position - target.position;
        TargetDist = Vector3.Normalize(TargetDist);

        transform.position -= (TargetDist * Input.GetAxis("Mouse ScrollWheel") * zoom);
        distance = (transform.position - target.position).magnitude;
        if(distance <= 1 || distance >= 5)
        {
            target.position -= (TargetDist * Input.GetAxis("Mouse ScrollWheel") * zoom);
        }
    }
}
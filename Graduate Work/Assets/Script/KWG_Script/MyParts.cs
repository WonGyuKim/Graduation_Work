using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface PartsAdapter
{
    MyParts getParts();
}

public abstract class MyParts
{
    private Vector3 origin;
    private SphereCollider sphere;
    private bool onDrag;
    private List<MyParts> next;

    public MyParts()
    {
        onDrag = false;
    }

    public void VerticalMove()
    {

    }

    public void MouseMove()
    {

    }

    public void SetArcballData(Transform parent)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit rayhit;

        if (Physics.Raycast(ray, out rayhit))
        {
            float round = 0;
            float InPro, nearOrtho;
            Vector3 far = new Vector3(rayhit.point.x, rayhit.point.y, rayhit.point.z);
            Vector3 asspnt;

            MeshCollider mesh = rayhit.collider as MeshCollider;

            foreach (Vector3 vertex in mesh.sharedMesh.vertices)
            {
                if (((parent.rotation * vertex + parent.position) - rayhit.point).magnitude > (far - rayhit.point).magnitude)
                {
                    far = parent.rotation * vertex + parent.position;
                }
            }

            nearOrtho = far.x * far.x + far.y * far.y + far.z * far.z;

            foreach (Vector3 vertex in mesh.sharedMesh.vertices)
            {
                asspnt = parent.rotation * vertex + parent.position;
                InPro = far.x * asspnt.x + far.y * asspnt.y + far.z * asspnt.z;
                if (round < Mathf.Sqrt((far - rayhit.point).magnitude * (far - rayhit.point).magnitude + (asspnt - rayhit.point).magnitude * (asspnt - rayhit.point).magnitude) && Mathf.Abs(InPro) < Mathf.Abs(nearOrtho))
                {
                    round = Mathf.Sqrt((far - rayhit.point).magnitude * (far - rayhit.point).magnitude + (asspnt - rayhit.point).magnitude * (asspnt - rayhit.point).magnitude);
                    nearOrtho = InPro;
                }
            }

            sphere = new SphereCollider();

        }
    }
    
    public void ArcballMove(GameObject parent)
    {

    }

    public void Link(Transform hole, Transform otherTrans)
    {

    }

    public void LinkMove(Transform hole, Transform otherTrans)
    {

    }

    public void LinkExit(Transform hole, Transform otherTrans)
    {

    }

    public void LinkAllMove()
    {

    }
    public abstract void LinkRotation(double F, double V);

    public bool OnDragCheck
    {
        get { return onDrag; }
        set { onDrag = value; }
    }

}

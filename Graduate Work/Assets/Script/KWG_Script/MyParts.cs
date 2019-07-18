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

    private List<MyParts> next;

    public MyParts()
    {

    }

    public void VerticalMove()
    {

    }
    public void MouseMove()
    {

    }
    public void ArcballMove()
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
        get;
    }

}

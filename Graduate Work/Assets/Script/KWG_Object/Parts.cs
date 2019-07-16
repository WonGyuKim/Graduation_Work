using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Part : MyParts
{
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


    public void LinkAllMove() // 연결된 파츠들과 같이 움직임
    {
    }

    public List<GameObject> LinkSearch() // 연결된 파츠들을 탐색 
    {
        return null;
    }

    public void LinkRotation(double F, double V)
    {
    }

    public bool OnDragCheck
    {
        get;
    }
}
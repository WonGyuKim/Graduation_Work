﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interface : MonoBehaviour
{
    
}

public interface IParts
{
    void Link(Transform hole, Transform otherTrans);

    void LinkMove(Transform hole, Transform otherTrans);

    void LinkExit(Transform hole, Transform otherTrans);

    void VerticalMove();

    void MouseMove();

    void ArcballMove();

    void MotoringMove(Vector3 point, Vector3 axis, float speed);

    void LinkAllMove();//연결된 파츠들과 같이 움직임

    List<GameObject> LinkSearch();//연결된 파츠들을 탐색

    void SearchReset();//검색된 결과 리셋

    bool OnDragCheck
    {
        get;
    }

    GameObject gameObj
    {
        get;
    }

    MotorNode node
    {
        get;
        set;
    }
}

public interface IGear : IParts
{
    void GearMove();
}
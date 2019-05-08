using System.Collections;
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

    void MotoringMove();

    bool OnDragCheck
    {
        get;
    }
}
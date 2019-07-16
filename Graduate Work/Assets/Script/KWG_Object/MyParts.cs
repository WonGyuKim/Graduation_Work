using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface MyParts
{
    void VerticalMove();
    void MouseMove();
    void ArcballMove();

    void Link(Transform hole, Transform otherTrans);
    void LinkMove(Transform hole, Transform otherTrans);
    void LinkExit(Transform hole, Transform otherTrans);
    void LinkAllMove();
    void LinkRotation(double F, double V);

}

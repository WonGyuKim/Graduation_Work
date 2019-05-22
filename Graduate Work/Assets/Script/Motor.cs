using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Motor : MonoBehaviour, IParts
{
    private bool flagActivated;
    // Start is called before the first frame update
    void Start()
    {
        flagActivated = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (flagActivated)
        {
            Active();
        }
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
    public void VerticalMove()
    {

    }
    public void MouseMove()
    {

    }
    public void ArcballMove()
    {

    }
    public void MotoringMove()
    {

    }
    public bool OnDragCheck
    {
        get;
    }

    public bool Activated
    {
        set { flagActivated = value; }
        get { return flagActivated; }
    }

    private void Active()
    {
        transform.Rotate(0, 0, 1);
    }

    public void Message()
    {
        Debug.Log("ALRJLKDJOAIR");
    }
}

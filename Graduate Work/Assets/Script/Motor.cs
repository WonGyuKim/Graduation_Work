using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Motor : MonoBehaviour, IParts
{
    private bool Activated;
    // Start is called before the first frame update
    void Start()
    {
        Activated = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Activated)
            Rotating();
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
    public void LinkAllMove()//연결된 파츠들과 같이 움직임
    {
    }
    public List<GameObject> LinkSearch()//연결된 파츠들을 탐색
    {
        return null;
    }
    public void SearchReset()//검색된 결과 리셋
    {
    }
    public bool OnDragCheck
    {
        get;
    }

    public bool Active
    {
        set
        {
            Activated = value;
        }
        get
        {
            return Activated;
        }
    }
    private void Rotating()
    {
        transform.Rotate(0, 0, 1);
    }
}

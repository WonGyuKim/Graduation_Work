using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Link //: MonoBehaviour
{
    protected List<MyParts> linking;

    public Link()
    {
        linking = new List<MyParts>();
    }

    public void Connect(MyParts left, MyParts right)
    {
        Debug.Log(left);
        Debug.Log(right);

        linking.Add(left);
        linking.Add(right);
    }

    public void Disconnect()
    {

    }

    public virtual void LinkRotation(MyParts parent, PowerData power) { }

    public void SetLinkMove(MyParts parent, GameObject head)
    {
        foreach(MyParts parts in linking)
        {
            if (!parts.Equals(parent))
                parts.SetLinkMove(this, head);
        }

    }

    public void QuitLinkMove(MyParts parent)
    {
        foreach(MyParts parts in linking)
        {
            if (!parts.Equals(parent))
                parts.QuitLinkMove(this);
        }
    }

    /*
     * 동력이 왼쪽에서 오는지 어떻게 아는가?
     * Parent가 판단
     */
     /*
    public MyParts Left
    {
        get { return left; }
        set { left = value; }
    }
    public MyParts Right
    {
        get { return right; }
        set { right = value; }
    }
    */
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Link //: MonoBehaviour
{
    protected MyParts left;
    protected MyParts right;

    public Link()
    {
    }

    public void Connect(MyParts left, MyParts right)
    {
        left.Link(this);
        right.Link(this);

        Debug.Log(left);
        Debug.Log(right);
        
        this.left = left;
        this.right = right;
    }

    public void Disconnect()
    {
        left.LinkExit(this);
        right.LinkExit(this);

        left = null;
        right = null;
    }

    public virtual void LinkRotation(MyParts parent, PowerData power) { }

    public void SetLinkMove(MyParts parent, GameObject head)
    {
        if (!left.Equals(parent))
            left.SetLinkMove(this, head);
        else if (!right.Equals(parent))
            right.SetLinkMove(this, head);
    }

    public void QuitLinkMove(MyParts parent)
    {
        
        if (!left.Equals(parent))
            left.QuitLinkMove(this);
        else if (!right.Equals(parent))
            right.QuitLinkMove(this);
    }
    
}
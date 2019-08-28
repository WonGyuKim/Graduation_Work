using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Link : MonoBehaviour
{
    MyParts left;
    MyParts right;

    public void Connect()
    {

    }

    public void Disconnect()
    {

    }

    public virtual void LinkRotation() { }

    /*
     * 동력이 왼쪽에서 오는지 어떻게 아는가?
     * Parent가 판단
     * 
     */
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
}
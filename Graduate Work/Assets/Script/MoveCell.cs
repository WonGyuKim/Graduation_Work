using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCell
{
    public Vector3 Axis { get; set; }
    public Vector3 Point { get; set; }
    public int MoveType { get; set; }
    public float MoveSpeed { get; set; }
    public Motor Motor { get; set; }

    public MoveCell()
    {
        Axis = new Vector3(0, 0, 0);
        Point = new Vector3(0, 0, 0);
        MoveType = 0;
        MoveSpeed = 0;
    }

    public MoveCell(Vector3 point, Vector3 axis, float moveSpeed, int moveType, Motor motor)
    {
        Point = point;
        Axis = axis;
        MoveSpeed = moveSpeed;
        MoveType = moveType;
        Motor = motor;
    }

    //public void MoveChange(Vector3 point, Vector3 axis, float moveSpeed, int moveType, Motor motor)
    //{
    //    Point = point;
    //    Axis = axis;
    //    MoveSpeed = moveSpeed;
    //    MoveType = moveType;
    //    Motor = motor;
    //}
}

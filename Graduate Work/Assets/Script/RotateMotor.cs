using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateMotor : MonoBehaviour
{
    public List<Motor> motorList;
    public List<MotorNode> nodeList;

    public enum MoveType
    {
        Rotate = 0,
        Horizon
    }

    void Start()
    {
        motorList = new List<Motor>();
        nodeList = new List<MotorNode>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.M))
        {
            foreach (Motor motor in motorList)
            {
                motor.MotoringMove(motor.transform.position, motor.transform.forward, 5, 0, 0);
                foreach (MotorNode node in nodeList)
                {
                    node.parts.SearchReset();
                }
                nodeList.Clear();
            }
        }
        else if (Input.GetKey(KeyCode.N))
        {
            foreach (Motor motor in motorList)
            {
                motor.MotoringMove(motor.transform.position, motor.transform.forward, -5, 0, 0);
                foreach (MotorNode node in nodeList)
                {
                    node.parts.SearchReset();
                }
                nodeList.Clear();
            }
        }
    }
}

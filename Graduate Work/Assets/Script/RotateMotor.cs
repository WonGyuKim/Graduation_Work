using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RotateMotor : MonoBehaviour
{
    public List<Motor> motorList;
    public List<MotorNode> nodeList;
    public bool buttonClick;

    public enum MoveType
    {
        Rotate = 0,
        Horizon
    }

    void Start()
    {
        motorList = new List<Motor>();
        nodeList = new List<MotorNode>();
        buttonClick = false;
    }

    void ClickButton()
    {
        buttonClick = !buttonClick;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            //Debug.Log("Rotate Start");
            foreach (Motor motor in motorList)
            {
                motor.MotoringMove(motor.transform.position, motor.transform.forward, motor.RotateSpeed * Time.deltaTime * 100, 0, 0);
                nodeList = nodeList.Distinct().ToList();
                foreach (MotorNode node in nodeList)
                {
                    node.parts.SearchReset();
                }
            }
        }
        else if (Input.GetKey(KeyCode.M))
        {
            //Debug.Log("Motoring");
            foreach (MotorNode node in nodeList)
            {
                node.parts.MotorRotate();
            }
        }
        else if(Input.GetKeyUp(KeyCode.M))
        {
            //Debug.Log("Rotate Finish");
            foreach (MotorNode node in nodeList)
            {
                node.parts.ResetValue();
            }
            nodeList.Clear();
        }
        else if (Input.GetKeyDown(KeyCode.N))
        {
            foreach (Motor motor in motorList)
            {
                motor.MotoringMove(motor.transform.position, motor.transform.forward, -motor.RotateSpeed * Time.deltaTime * 100, 0, 0);
                foreach (MotorNode node in nodeList)
                {
                    node.parts.SearchReset();
                }
            }
        }
        else if (Input.GetKey(KeyCode.N))
        {
            foreach (MotorNode node in nodeList)
            {
                node.parts.MotorRotate();
            }
        }
        else if (Input.GetKeyUp(KeyCode.N))
        {
            foreach (MotorNode node in nodeList)
            {
                node.parts.ResetValue();
            }
            nodeList.Clear();
        }
    }
}
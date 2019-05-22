using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // UI Components
    public Button BtnCreate;
    public Button BtnPower;
    public Dropdown DDLCreate;

    // Variables
    private int target;
    public Motor[] motors;

    enum Object
    {
        gear15,
        gear30,
        axle1,
        axle2,
        axle4,
        beam3,
        beam6,
        beam12,
        connector1,
        connector2,
        motor,
        motorbody
    };

    public void Start()
    {
    }

    public void CreateObject()
    {
        GameObject obj = MakeObject(target);

        Instantiate(obj);
    }
    public void GetObjectValue()
    {
        target = DDLCreate.value;
    }

    /* 
     * Get input number and Return appropriate Object
     * You should change this function to adjust for making Objects
     */
    private GameObject MakeObject(int target)
    {
        string path = "Models/Prefabs/";
        string ObjectName = "";

        switch ((Object)target)
        {
            case Object.gear15:
                ObjectName = Object.gear15.ToString();
                break;
            case Object.gear30:
                ObjectName = Object.gear30.ToString();
                break;
            case Object.axle1:
                ObjectName = Object.axle1.ToString();
                break;
            case Object.axle2:
                ObjectName = Object.axle2.ToString();
                break;
            case Object.axle4:
                ObjectName = Object.axle4.ToString();
                break;
            case Object.beam3:
                ObjectName = Object.beam3.ToString();
                break;
            case Object.beam6:
                ObjectName = Object.beam6.ToString();
                break;
            case Object.beam12:
                ObjectName = Object.beam12.ToString();
                break;
            case Object.connector1:
                ObjectName = Object.connector1.ToString();
                break;
            case Object.connector2:
                ObjectName = Object.connector2.ToString();
                break;
            case Object.motor:
                ObjectName = Object.motor.ToString();
                break;
        }

        GameObject obj = Resources.Load(path + ObjectName) as GameObject;

        return obj;
    }

    /*
     * Search the whole motors and turn them on
     */
    public void EnableMotors()
    {
        motors = GetComponents(typeof(Motor)) as Motor[];

        Debug.Log(motors.Length);
        foreach (Motor motor in motors)
        {
            Debug.Log(motor.ToString());
            motor.Activated = !motor.Activated;
        }
    }
}

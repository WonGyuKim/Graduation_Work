using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // UI Components
    public Button BtnCreate;
    public Button BtnMotor;
    public Dropdown DDLCreate;

    // Variables
    private int target;
    public Motor[] motors;

    enum Object
    {
        axle2,
        axle3,
        axle4,
        axle5,
        axle6,
        axle7,
        axle8,
        axle9,
        axle10,
        axle11,
        axle12,
        beam3,
        beam7,
        beam11,
        bevelGear12,
        bevelGear20,
        connector2,
        connector3,
        connWithaxle,
        gear8,
        gear24,
        gear40,
        motor,
        rackGear,
        wormGear
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
            case Object.axle2:
                ObjectName = Object.axle2.ToString();
                break;
            case Object.axle3:
                ObjectName = Object.axle3.ToString();
                break;
            case Object.axle4:
                ObjectName = Object.axle4.ToString();
                break;
            case Object.axle5:
                ObjectName = Object.axle5.ToString();
                break;
            case Object.axle6:
                ObjectName = Object.axle6.ToString();
                break;
            case Object.axle7:
                ObjectName = Object.axle7.ToString();
                break;
            case Object.axle8:
                ObjectName = Object.axle8.ToString();
                break;
            case Object.axle9:
                ObjectName = Object.axle9.ToString();
                break;
            case Object.axle10:
                ObjectName = Object.axle10.ToString();
                break;
            case Object.axle11:
                ObjectName = Object.axle11.ToString();
                break;
            case Object.axle12:
                ObjectName = Object.axle12.ToString();
                break;
            case Object.beam3:
                ObjectName = Object.beam3.ToString();
                break;
            case Object.beam7:
                ObjectName = Object.beam7.ToString();
                break;
            case Object.beam11:
                ObjectName = Object.beam11.ToString();
                break;
            case Object.bevelGear12:
                ObjectName = Object.bevelGear12.ToString();
                break;
            case Object.bevelGear20:
                ObjectName = Object.bevelGear20.ToString();
                break;
            case Object.connector2:
                ObjectName = Object.connector2.ToString();
                break;
            case Object.connector3:
                ObjectName = Object.connector3.ToString();
                break;
            case Object.connWithaxle:
                ObjectName = Object.connWithaxle.ToString();
                break;
            case Object.gear8:
                ObjectName = Object.gear8.ToString();
                break;
            case Object.gear24:
                ObjectName = Object.gear24.ToString();
                break;
            case Object.gear40:
                ObjectName = Object.gear40.ToString();
                break;
            case Object.motor:
                ObjectName = Object.motor.ToString();
                break;
            case Object.rackGear:
                ObjectName = Object.rackGear.ToString();
                break;
            case Object.wormGear:
                ObjectName = Object.wormGear.ToString();
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MotorText : MonoBehaviour
{
    public Text motorText;
    public Motor motor;
    public InputField input;
    public float RotateSpeed;

    void Start()
    {
        motorText = GetComponent<Text>();
        RotateMotor rotM = GameObject.Find("RotateControl").GetComponent<RotateMotor>();
        motorText.text = "";
        motorText.text = "Motor" + rotM.motorList.Count.ToString() + " Velocity : ";
        motor = rotM.motorList[rotM.motorList.Count - 1];
        Transform childInput = transform.GetChild(0);

        input = childInput.gameObject.GetComponent<InputField>();
        input.text = 5.ToString();
        input.onEndEdit.AddListener(delegate
        {
            UpdateInput(input);
        });
    }
    void UpdateInput(InputField inF)
    {
        int chknum = 0;

        if(int.TryParse(inF.text, out chknum))
        {
            motor.RotateSpeed = chknum;
        }
        else
        {

        }
    }
}

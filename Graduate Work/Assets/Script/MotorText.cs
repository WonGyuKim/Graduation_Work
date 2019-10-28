using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MotorText : MonoBehaviour
{
    public Text motorText;
    public Motor motor;
    public InputField input;
    public Toggle tog;
    public float RotateSpeed;

    void OnEnable()
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

        Transform childTog = transform.GetChild(1);

        tog = childTog.gameObject.GetComponent<Toggle>();

        tog.onValueChanged.AddListener(delegate
        {
            UpdateTog(tog);
        });
    }

    void UpdateInput(InputField inF)
    {
        int chknum = 0;

        if(int.TryParse(inF.text, out chknum))
        {
            if(chknum > 70)
            {
                chknum = 70;
                inF.text = 70.ToString();
            }
            if(motor.RotateSpeed >= 0)
                motor.RotateSpeed = chknum;
            else
                motor.RotateSpeed = -chknum;
        }
    }

    void UpdateTog(Toggle togl)
    {
        if(togl.isOn)
        {
            motor.on = false;
        }
        else
        {
            motor.on = true;
        }
    }
}

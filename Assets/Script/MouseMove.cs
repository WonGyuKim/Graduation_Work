using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMove : MonoBehaviour
{
    private Vector2 startPos;
    private bool altIn;
    // 초기화 함수
    void Start()
    {
        altIn = false;
    }

    // 프레임마다 계속 호출
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftAlt))
        {
            altIn = true;
        }
        if(Input.GetKeyUp(KeyCode.LeftAlt))
        {
            altIn = false;
        }
        // 마우스 이동
        if (Input.GetMouseButtonDown(0) && altIn)
        {
            startPos = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0) && altIn)
        {
            float mouseMoveX = Input.mousePosition.x - startPos.x;
            float mouseMoveY = Input.mousePosition.y - startPos.y;
            startPos = Input.mousePosition;

            //float ex = transform.rotation.eulerAngles.x - mouseMoveY;
            //if(ex >= 50 || ex <= 0) mouseMoveY = 0;

            transform.Rotate(-mouseMoveY * 0.1f, 0, 0);
            transform.Rotate(0, mouseMoveX * 0.1f, 0, Space.World);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UImanager : MonoBehaviour {
    public Text text;
    public Dropdown dropdown;
    public Button button;
    private string target;
    private ObjectManager objectManager;
	// Use this for initialization
	void Start () {
        //보통 여기서 초기화 함
        target = "RED";
        objectManager = GameObject.Find("ObjectManager").GetComponent<ObjectManager>(); //public으로 해서 연결하는 방법도 있지만 Find로 오브젝트 찾는 방법도 있음. 단, 속도는 느리고 비동기 작업이므로 주의
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void OnValueChange(int val) {
        target = dropdown.options[val].text;   //옵션은 <OptionData, int> 기 때문에 string으로 받아주고 int 형으로 바꿈
    }
    public void OnClickAction() {
        if(objectManager != null) {
            //이런식으로 null체크는 해주는게 좋음
            objectManager.CreateObject(target);
        }
    }
}

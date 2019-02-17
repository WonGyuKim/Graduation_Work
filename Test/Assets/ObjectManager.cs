using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour {
    public GameObject[] gameObjects;
    public Transform pos;
    private GameObject mobj;
	// Use this for initialization
	void Start () {
        mobj = null;	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void CreateObject(string val) {
        if(mobj != null) {
            Destroy(mobj);
        }
        int num = 0;
        switch (val) {
            case "RED":
                num = 0;
                break;
            case "GREEN":
                num = 1;
                break;
            case "BLUE":
                num = 2;
                break;
            case "WHITE":
                num = 3;
                break;
            case "BLACK":
                num = 4;
                break;
            case "YELLO":
                num = 5;
                break;
        }
        mobj = Instantiate(gameObjects[num], pos);
    }
}

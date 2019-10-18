﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public class UIManager : MonoBehaviour
{
    // UI Components
    public Button BtnCreate;
    public Dropdown DDLCreate;
    public Button Save;
    public Button Load;
    public Text text;

    // Variables
    private int target;
    private List<GameObject> list;
    private IParts data;
    private bool selected, selected_all;
    string path;

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
        wormGear,
        angular_beam_2x4,
        angular_block_0,
        angular_block_90,
        angular_block_180,
        angular_connector_peg,
        cross_block,
        cross_block2,
        cross_block3,
        cross_block_fork,
        double_connector_peg,
        double_cross_block,
        triangle_beam_half
    };

    void Start()
    {
        list = new List<GameObject>();
        data = null;
        selected = false;
        selected_all = false;
        path = "Models/Prefabs/";
        text.text = "Selected : ";
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayhit;

            if (Physics.Raycast(ray, out rayhit))
            {
                GameObject search = rayhit.collider.gameObject;

                foreach (GameObject gameObject in list)
                {
                    if (gameObject.Equals(search))
                    {
                        if (data == null || !data.gameObj.Equals(search))
                        {
                            data = gameObject.GetComponent<IParts>();
                            selected = true;
                            selected_all = true;
                        }
                        else if (selected_all)
                        {
                            selected_all = false;
                        }
                        else
                        {
                            selected = false;
                            data = null;
                        }


                        break;
                    }

                }
                selected_all = true;
                text.text = "Selected : ";
                if (selected)
                {
                    text.text += data.ToString();
                }
                if (selected_all)
                {
                    text.text += "and connected with it";
                }

            }

        }
        
    }

    public void CreateObject()
    {
        string kind = WhatKind(target);
        GameObject obj = MakeObject(kind);
        obj.GetComponent<IParts>().Loaded = false;
    }

    public void GetObjectValue()
    {
        target = DDLCreate.value;
    }

    /* 
     * Get input number and Return appropriate Object
     * You should change this function to adjust for making Objects
     */

    private string WhatKind(int target)
    {
        string ObjectName = null;

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
            case Object.angular_beam_2x4:
                ObjectName = Object.angular_beam_2x4.ToString();
                break;
            case Object.angular_block_0:
                ObjectName = Object.angular_block_0.ToString();
                break;
            case Object.angular_block_90:
                ObjectName = Object.angular_block_90.ToString();
                break;
            case Object.angular_block_180:
                ObjectName = Object.angular_block_180.ToString();
                break;
            case Object.angular_connector_peg:
                ObjectName = Object.angular_connector_peg.ToString();
                break;
            case Object.cross_block:
                ObjectName = Object.cross_block.ToString();
                break;
            case Object.cross_block2:
                ObjectName = Object.cross_block2.ToString();
                break;
            case Object.cross_block3:
                ObjectName = Object.cross_block3.ToString();
                break;
            case Object.cross_block_fork:
                ObjectName = Object.cross_block_fork.ToString();
                break;
            case Object.double_connector_peg:
                ObjectName = Object.double_connector_peg.ToString();
                break;
            case Object.double_cross_block:
                ObjectName = Object.double_cross_block.ToString();
                break;
            case Object.triangle_beam_half:
                ObjectName = Object.triangle_beam_half.ToString();
                break;
        }

        return ObjectName;
    }

    private GameObject MakeObject(string ObjectName)
    {
        GameObject obj = Instantiate(Resources.Load(path + ObjectName)) as GameObject;
        obj.GetComponent<IParts>().Kind = ObjectName;
        list.Add(obj);
        return obj;
    }

    private GameObject MakeObject(SaveData save)
    {
        GameObject obj = Instantiate(Resources.Load(path + save.Kind), save.Vector3, save.Quaternion) as GameObject;
        obj.GetComponent<IParts>().Kind = save.Kind;
        list.Add(obj);

        return obj;
    }

    public void SaveObject()
    {
        string SavePath = EditorUtility.SaveFilePanel("Saving Data...", Application.dataPath, "", "bin");

        if (SavePath.Length != 0 && selected)
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream(SavePath, FileMode.Create);
            SaveData output;

            if (selected && selected_all) // save selected object and connected with it
            {
                List<GameObject> data_all = data.LinkSearch();
                output = new SaveData();

                foreach (GameObject gameobject in data_all)
                {
                    Debug.Log(gameobject);
                    
                    output.Vector3 = gameobject.transform.position;
                    output.Quaternion = gameobject.transform.rotation;
                    output.Kind = gameobject.GetComponent<IParts>().Kind;
                    bf.Serialize(fs, output);
                    gameobject.GetComponent<IParts>().SearchReset();
                }

            }
            else if (selected)// save only selected object
            {
                output = new SaveData(data.gameObj.transform.position, data.gameObj.transform.rotation, data.Kind);
                
                bf.Serialize(fs, output);
            }

            fs.Close();
        }
    }

    public void LoadObject()
    {
        string LoadPath = EditorUtility.OpenFilePanel("Opening Data...", Application.dataPath, "bin");

        if (LoadPath.Length != 0)
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream(LoadPath, FileMode.Open);
            SaveData input;
            GameObject load;
            
            while (fs.Position != fs.Length)
            {
                input = bf.Deserialize(fs) as SaveData;

                load = MakeObject(input);
                load.GetComponent<IParts>().Loaded = true;
                
            }
            


            fs.Close();

        }
    }
}

[Serializable]
class SaveData
{
    SVector3 sv3;
    SQuaternion sq;
    string kind;

    public SaveData()
    {
        sv3 = new SVector3();
        sq = new SQuaternion();
        kind = null;
    }

    public SaveData(Vector3 vector3, Quaternion quaternion, string Kind)
    {
        sv3 = new SVector3(vector3);
        sq = new SQuaternion(quaternion);
        kind = Kind;
    }

    public Vector3 Vector3
    {
        get { return sv3.Vector3; }
        set { sv3.Vector3 = value; }
    }

    public Quaternion Quaternion
    {
        get { return sq.Quaternion; }
        set { sq.Quaternion = value; }
    }

    public string Kind
    {
        get { return kind; }
        set { kind = value; }
    }

}

[Serializable]
class SVector3
{
    float x, y, z;

    public SVector3()
    {
    }

    public SVector3(Vector3 vector3)
    {
        this.Vector3 = vector3;
    }

    public Vector3 Vector3
    {
        get { return new Vector3(x, y, z); }
        set { x = value.x; y = value.y; z = value.z; }
    }
}

[Serializable]
class SQuaternion
{
    float x, y, z, w;

    public SQuaternion()
    {
    }

    public SQuaternion(Quaternion quaternion)
    {
        this.Quaternion = quaternion;
    }

    public Quaternion Quaternion
    {
        get { return new Quaternion(x, y, z, w); }
        set { x = value.x; y = value.y; z = value.z; w = value.w; }
    }
}
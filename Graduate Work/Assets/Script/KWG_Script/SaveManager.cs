using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

[Serializable]
public class SerializeData
{
    [Serializable]
    class SVector3
    {
        float x, y, z;

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

        public Quaternion Quaternion
        {
            get { return new Quaternion(x, y, z, w); }
            set { x = value.x; y = value.y; z = value.z; w = value.w; }
        }
    }
}
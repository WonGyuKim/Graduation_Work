using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (transform.parent != null)
        {
            Transform parent = transform;

            while (parent.transform.parent != null)
            {
                parent = parent.transform.parent;
            }
<<<<<<< HEAD
            parent.position = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.transform.position.x + Screen.width / 2, Camera.main.transform.position.y + Screen.height / 2, 2));
        }
        else        
            transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.transform.position.x + Screen.width / 2, Camera.main.transform.position.y + Screen.height / 2, 2));
=======
            parent.position = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.transform.position.x + Screen.width / 2, Camera.main.transform.position.y + Screen.height / 2, 10));
        }
        else        
            transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.transform.position.x + Screen.width / 2, Camera.main.transform.position.y + Screen.height / 2, 10));
>>>>>>> fc084b894c8c4f5c063d2b44f3c55f26483f076c
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDrag()
    {
        //transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
        if (transform.parent != null)
        {
            Transform parent = transform;
            while (parent.transform.parent != null)
            {
                parent = parent.transform.parent;
            }
<<<<<<< HEAD
            parent.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 2));
        }
        else
            transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 2));
=======
            parent.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
        }
        else
            transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
>>>>>>> fc084b894c8c4f5c063d2b44f3c55f26483f076c

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canvas_Behaviour : MonoBehaviour
{
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        gameObject.transform.LookAt(Camera.main.transform.position);
    }
}

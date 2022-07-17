using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Billboard : MonoBehaviour
{
    /// <summary>
    /// the camera to look at
    /// </summary>
    public Transform cam;

    private void Start()
    {
        if (cam == null)
        {
            cam = GameObject.FindGameObjectWithTag("MainCamera").transform;
        }
    }

    private void LateUpdate()
    {
        transform.LookAt(transform.position + cam.forward);
    }
}

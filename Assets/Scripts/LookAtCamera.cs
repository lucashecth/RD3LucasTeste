using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    // Start is called before the first frame update
    public Camera arCamera;

    void start()
    {/*
        arCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
       // arCamera = CameraController.Instance.CurrentCamera;
        Vector3 targetPosition = new Vector3(arCamera.transform.position.x, transform.position.y, arCamera.transform.position.z);
        transform.LookAt(targetPosition);*/
    }

}

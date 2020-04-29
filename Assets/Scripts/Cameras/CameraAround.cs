using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAround : MonoBehaviour
{
    private bool _isRun;
    public bool isRun
    {
        set
        {
            _isRun = value;
            if (value)
                SetCameraAround(value);
            else
                SetCameraAround(value);
        }
    }
    public float speed = 3;
    Camera mainCamera = null;


    int cameraX = 0;
    int cameraY = 30;
    int cameraZ;
    int RotationX = 60;

    private void Start()
    {
        cameraZ = -MapGenerator.MapSize / 2;
        mainCamera = GetComponentInChildren<Camera>();
        mainCamera.transform.position = new Vector3(cameraX,cameraY,cameraZ);
    }

    void SetCameraAround(bool value)
    {
        if (value)
        {
            mainCamera.transform.position = new Vector3(cameraX, cameraY, cameraZ);
            mainCamera.transform.rotation.Set(RotationX, 0, 0, 0);
        }
    }

    void Update()
    {
        if(_isRun)
            transform.Rotate(0, speed * Time.deltaTime, 0);
    }
}

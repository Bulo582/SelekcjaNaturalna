using System.Collections;
using System.Collections.Generic;
using UnityEngine;
enum CameraDir
{
    front = 1,
    left = 2,
    back = 3,
    right = 4
}
public class RTSCamera : MonoBehaviour
{
    private static bool _isRun;
    public bool isRun
    {
        set
        {
            _isRun = value;
            if (value)
                SetRTSCamera(value);
            else
                SetRTSCamera(value);
        }
    }

    Vector3 SavePos;
    Quaternion SaveRot;


    Vector2 panLimit;  // x,z limit
    float minY = -25f;  // y limit
    float maxY = 120f;
    float panBorderThickness = 10f;

    float rotY = 0;
    Quaternion rotQuar;
    CameraDir cameraDir = CameraDir.front;
    bool isRotating = false;

    float scrollSpeed = 20f;
    float speed = 3f;
    int boostSpeed => isShift ? 4 : 1;
    bool isShift = false;

    void Update()
    {
        if (_isRun)
        {
            CameraMove();
            Rotating();
        }
    }

    private void Start()
    {
        panLimit = new Vector2(MapGenerator.MapSize, MapGenerator.MapSize);
        rotQuar = this.transform.rotation;
        SavePos = this.transform.position;
        SaveRot = this.transform.rotation;
    }

    void SetRTSCamera(bool value)
    {
        if (!value)
        {
            SavePos = transform.position;
            SaveRot = transform.rotation;
            transform.position = new Vector3(0, 0, 0);
            transform.rotation = new Quaternion(0, 0, 0, 0);
        }
        else
        {
            transform.position = SavePos;
            transform.rotation = SaveRot;
        }
    }

    public void CameraMove()
    {
        Vector3 pos = transform.position;

        if (Input.GetKey("left shift"))
            isShift = true;

        if (cameraDir == CameraDir.front)
            FrontCamera(ref pos);
        else if (cameraDir == CameraDir.left)
            LeftCamera(ref pos);
        else if (cameraDir == CameraDir.back)
            DownCamera(ref pos);
        else if (cameraDir == CameraDir.right)
            RightCamera(ref pos);

        if (Input.GetKeyDown("q"))
        {
           rotY += 90;
           rotQuar = Quaternion.Euler(0, rotY, 0);

            if ((int)cameraDir == 1)
                cameraDir = CameraDir.left;
            else if ((int)cameraDir == 2)
                cameraDir = CameraDir.back;
            else if ((int)cameraDir == 3)
                cameraDir = CameraDir.right;
            else if ((int)cameraDir == 4)
                cameraDir = CameraDir.front;
        }

        if (Input.GetKeyDown("e"))
        {
            rotY -= 90;
            rotQuar = Quaternion.Euler(0, rotY, 0);

            if ((int)cameraDir == 1)
                cameraDir = CameraDir.right;
            else if ((int)cameraDir == 2)
                cameraDir = CameraDir.front;
            else if ((int)cameraDir == 3)
                cameraDir = CameraDir.left;
            else if ((int)cameraDir == 4)
                cameraDir = CameraDir.back;
        }

        float scrol = Input.GetAxis("Mouse ScrollWheel");
        pos.y -= scrol * scrollSpeed * 10f * Time.deltaTime * boostSpeed;

        pos.x = Mathf.Clamp(pos.x, -panLimit.x, panLimit.x);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        pos.z = Mathf.Clamp(pos.z, -panLimit.y, panLimit.y);

        transform.position = pos;
        isShift = false;
    }

    void Rotating()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, rotQuar, Time.deltaTime * 2);
    }

    void FrontCamera(ref Vector3 pos)
    {
        if (Input.GetKey("w") || Input.mousePosition.y >= Screen.height - panBorderThickness)
        {
            pos.z += speed * Time.deltaTime * boostSpeed; ;
        }
        if (Input.GetKey("s") || Input.mousePosition.y <= panBorderThickness)
        {
            pos.z -= speed * Time.deltaTime * boostSpeed; ;
        }
        if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - panBorderThickness)
        {
            pos.x += speed * Time.deltaTime * boostSpeed;
        }
        if (Input.GetKey("a") || Input.mousePosition.x <= panBorderThickness)
        {
            pos.x -= speed * Time.deltaTime * boostSpeed; ;
        }
    }

    void LeftCamera(ref Vector3 pos)
    {
        if (Input.GetKey("w") || Input.mousePosition.y >= Screen.height - panBorderThickness)
        {
            pos.x += speed * Time.deltaTime * boostSpeed; ;
        }
        if (Input.GetKey("s") || Input.mousePosition.y <= panBorderThickness)
        {
            pos.x -= speed * Time.deltaTime * boostSpeed; ;
        }
        if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - panBorderThickness)
        {
            pos.z -= speed * Time.deltaTime * boostSpeed;
        }
        if (Input.GetKey("a") || Input.mousePosition.x <= panBorderThickness)
        {
            pos.z += speed * Time.deltaTime * boostSpeed; ;
        }
    }

    void DownCamera(ref Vector3 pos)
    {
        if (Input.GetKey("w") || Input.mousePosition.y >= Screen.height - panBorderThickness)
        {
            pos.z -= speed * Time.deltaTime * boostSpeed; ;
        }
        if (Input.GetKey("s") || Input.mousePosition.y <= panBorderThickness)
        {
            pos.z += speed * Time.deltaTime * boostSpeed; ;
        }
        if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - panBorderThickness)
        {
            pos.x -= speed * Time.deltaTime * boostSpeed;
        }
        if (Input.GetKey("a") || Input.mousePosition.x <= panBorderThickness)
        {
            pos.x += speed * Time.deltaTime * boostSpeed; ;
        }
    }

    void RightCamera(ref Vector3 pos)
    {
        if (Input.GetKey("w") || Input.mousePosition.y >= Screen.height - panBorderThickness)
        {
            pos.x -= speed * Time.deltaTime * boostSpeed; ;
        }
        if (Input.GetKey("s") || Input.mousePosition.y <= panBorderThickness)
        {
            pos.x += speed * Time.deltaTime * boostSpeed; ;
        }
        if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - panBorderThickness)
        {
            pos.z += speed * Time.deltaTime * boostSpeed;
        }
        if (Input.GetKey("a") || Input.mousePosition.x <= panBorderThickness)
        {
            pos.z -= speed * Time.deltaTime * boostSpeed; ;
        }
    }
}



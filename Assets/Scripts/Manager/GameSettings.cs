using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    CameraAround ca;
    FreeLookCamera flc;
    RTSCamera rtsc;

    [Range(0, 10)]
    public float timeSpeed = 1;

    [Header("DebugModes")]
    public bool DoTXT = true;
    public bool debugMode = true;
    public bool loggerMode = false;

    [Header("Cameras")]
    public bool aroundCamera = true;
    public bool RTS_Camera = false;
    public bool freeLookCamera = false;
    public bool autoUpdate
    {
        get { return true; }
    }
    private void Start()
    {
        SetTime();
        SetDebugMode();
        ca = GameObject.Find("CameraManager").GetComponent<CameraAround>();
        rtsc = GameObject.Find("CameraManager").GetComponent<RTSCamera>();
        flc = GameObject.Find("CameraManager").GetComponent<FreeLookCamera>();
        SetAroundCam();
        SetRTSCam();
        SetFreeLookCam();
    }
    private void Awake()
    {
        SetLoggerMode();
        SetDoTXT();
        DeleteAllTestFile();
        DeleteAllTestFolders();
    }

    public void SetAroundCam()
    {
        ca.isRun = aroundCamera;
    }

    public void SetRTSCam()
    {
        rtsc.isRun = RTS_Camera;
    }

    public void SetFreeLookCam()
    {
        flc.isRun = freeLookCamera;
    }

    public void SetTime()
    {
        Time.timeScale = timeSpeed;
    }
    public void SetDoTXT()
    {
        MapToTxt.Do = DoTXT;
    }
    public void SetDebugMode()
    {
        GetComponent<KeepSimulation>().debugMode = debugMode;
        AlgoritmTime.Instance.canDo = debugMode;
    }
    public void SetLoggerMode()
    {
        GameManager.logger.canLog = loggerMode;
    }
    public void DeleteAllTestFile()
    {
        FileHelper.DeleteAllFiles(FileHelper.testFolder);
    }
    public void DeleteAllTestFolders()
    {
        FileHelper.DeleteAllFolders(FileHelper.testFolder);
    }

    private void OnValidate()
    {
       if(aroundCamera)
        {
            RTS_Camera = false;
            freeLookCamera = false;
        }
       if(RTS_Camera)
        {
            freeLookCamera = false;
            aroundCamera = false;
        }
       if(freeLookCamera)
        {
            RTS_Camera = false;
            aroundCamera = false;
        }
    }
}

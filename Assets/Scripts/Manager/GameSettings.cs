using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    [Range(0, 10)]
    public float timeSpeed = 1;
    public bool DoTXT = true;

    public bool debugMode = true;
    public bool loggerMode = false;

    public bool autoUpdate
    {
        get { return true; }
    }
    private void Start()
    {
        SetTime();
        SetDebugMode();
    }
    private void Awake()
    {
        SetLoggerMode();
        SetDoTXT();
        DeleteAllTestFile();
        DeleteAllTestFolders();
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
}

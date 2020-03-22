using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public bool autoUpdate
    {
        get { return true; }
    }

    [Range(0, 10)]
    public float timeSpeed = 1;
    public bool DoTXT = true;

    private void Start()
    {
        SetTime();
        SetDoTXT();
    }

    private void Awake()
    {
        DeleteAllTestFile();
        DeleteAllTestFolders();
    }

    public void SetTime()
    {
        Time.timeScale = timeSpeed;
    }
    public void SetDoTXT()
    {
        ArrayToTxt.Do = DoTXT;
    }

    public void DeleteAllTestFile()
    {
        FileHelper.DeleteAllFiles(ArrayToTxt.TestFolder);
    }

    public void DeleteAllTestFolders()
    {
        FileHelper.DeleteAllFolders(ArrayToTxt.TestFolder);
    }
}

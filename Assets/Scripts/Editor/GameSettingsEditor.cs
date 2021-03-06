﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameSettings))]
public class GameSettingsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        GameSettings gameSet = (GameSettings)target;
        if (DrawDefaultInspector())
        {
            if (gameSet.autoUpdate)
            {
                gameSet.SetTime();
                gameSet.SetDoTXT();
                gameSet.SetDebugMode();
                gameSet.SetLoggerMode();
                gameSet.SetRTSCam();
                gameSet.SetFreeLookCam();
                gameSet.SetAroundCam();
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileHelper 
{
    public static void DeleteAllFiles(string path)
    {
        DirectoryInfo di = new DirectoryInfo(path);
        foreach (FileInfo file in di.GetFiles())
        {
            file.Delete();
        }
    }

    public static void DeleteAllFolders(string path)
    {
        DirectoryInfo di = new DirectoryInfo(path);
        foreach (DirectoryInfo dict in di.GetDirectories())
        {
            dict.Delete(true);
        }
    }

    public static void CreateFolder(string path, string folderName)
    {
        Directory.CreateDirectory(Path.Combine(path, folderName));
    }

}

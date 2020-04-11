using System;
using System.IO;


public class FileHelper 
{
    public static string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
    public static readonly string testFolder = Path.Combine(desktop, "TestLand");

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

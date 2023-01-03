// Used to call functions when saving and loading to .DAT files
using UnityEngine;
using System;
using System.IO;


public class FileManager : MonoBehaviour
{
    public static bool WriteToFile(string a_FileName, string a_FileContents)
    {
        var fullPath = Path.Combine(Application.persistentDataPath, a_FileName);

        try
        {
            File.WriteAllText(fullPath, a_FileContents);
            Debug.LogFormat($"Successfully Saved to {fullPath}.");
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to write to {fullPath} with exception {e}");
        }

        return false;
    }


    public static bool LoadFromFile(string a_FileName, out string result)
    {
        var fullPath = Path.Combine(Application.persistentDataPath, a_FileName);

        try
        {
            result = File.ReadAllText(fullPath);
            Debug.LogFormat($"Successfully Loaded from {fullPath}.");
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to read from {fullPath} WaitWhile exception {e}");
            result = "";
            return false;
        }
    }
}

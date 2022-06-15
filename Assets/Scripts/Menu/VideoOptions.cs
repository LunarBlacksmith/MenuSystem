using UnityEngine;  // needed for Resources.Load()
using UnityEditor;  // to make our own tool, update our editor
using System;       // convert string to enum
using System.IO;    // have access to characters from a byte stream


/*
bit == is a singular 0 or 1 
byte = 8 bits = 01100001 = 1 char
so a byte is a group of bits
a stream is known as a sequence
sequence is a succession
a byte stream is a succession of a group of bits
 */
public class VideoOptions
{
    // at this file location
    static string path = Path.Combine(Application.streamingAssetsPath, "Save/VideoOptions.txt");
    //static string path = "Assets/Resources/Save/VideoOptions.txt";

    // Unity Editor allows me to create a tool in my Menus
#if UNITY_EDITOR
    [MenuItem("Tool/Save/Write VideoOptions File")]
#endif
    // This is a public static behaviour that we can call in our scripts
    public static void WriteSaveFile()
    {
        // true - add to file
        // false - overwrite file
        StreamWriter sWriter = new StreamWriter(path, false);

        sWriter.WriteLine("Fullscreen:" + Screen.fullScreen.ToString());

        // write our resolution to the file
        // the resolution will be written in with a : to separate the adjective "resolution" and the actual res
        sWriter.WriteLine("Resolution:" + Screen.currentResolution.ToString());

        sWriter.WriteLine("Quality:" + QualitySettings.GetQualityLevel().ToString());

        // writing is done
        sWriter.Close();

        // re-import the file to update the reference in the editor
//#if UNITY_EDITOR
//          AssetDatabase.ImportAsset(path);
//          TextAsset asset = Resources.Load("Save/VideoOptions.txt") as TextAsset;
//#endif
    }

#if UNITY_EDITOR
    [MenuItem("Tool/Save/Read VideoOptions File")]
#endif
    public static void ReadSaveFile()
    {
        // Read text from file
        StreamReader sReader = new StreamReader(path, true);

        // ref to the line we are reading
        string line;
        bool isFullscreen = false;
        Resolution res = new Resolution();
        int qualityIndex = 0;
        while ((line = sReader.ReadLine()) != null)
        {
            string[] parts = line.Split(':');
            switch (parts[0])
            {
                case "Fullscreen":
                    {
                        isFullscreen = (bool)Enum.Parse(typeof(bool), parts[1]);
                        break;
                    }
                case "Resolution":
                    {
                        res = (Resolution)Enum.Parse(typeof(Resolution), parts[1]);
                        break; 
                    }
                case "Quality":
                    {
                        qualityIndex = (int)Enum.Parse(typeof(int), parts[1]);
                        break; 
                    }
            }
        }
        if (res.width != 0 || res.height != 0)
        { Screen.SetResolution(res.width, res.height, isFullscreen); }
        else
        { Screen.SetResolution(Screen.resolutions[0].width, Screen.resolutions[0].height, isFullscreen); }
        QualitySettings.SetQualityLevel(qualityIndex);

        sReader.Close();
    }

}

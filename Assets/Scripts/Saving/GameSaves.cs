using UnityEngine;  // needed for Resources.Load()
using UnityEditor;  // to make our own tool, update our editor
using System;       // convert string to enum
using System.IO;    // have access to characters from a byte stream

public class GameSaves
{
    // at this file location
   static string path = Path.Combine(Application.streamingAssetsPath, "Save/GameSaves");
    //static string path = "Assets/Resources/Save/GameSaves";

    public static int lastGameSaveSlot;

    public static void ChooseSlotSave(int saveIndex_p)
    {
        if (!path.Contains("."))
        { path += saveIndex_p.ToString() + ".txt"; }
        else
        {
            string[] pathParts = path.Split('.');
            pathParts[0] = pathParts[0].Remove(pathParts[0].Length - 1);
            pathParts[0] += saveIndex_p.ToString() + ".txt";
            path = pathParts[0];
            lastGameSaveSlot = saveIndex_p;
        }
    }
    public static void ChooseSlotLoad(int saveIndex_p)
    {
        if (!path.Contains("."))
        { path += saveIndex_p.ToString() + ".txt"; }
        else
        {
            string[] pathParts = path.Split('.');
            pathParts[0] = pathParts[0].Remove(pathParts[0].Length - 1);
            pathParts[0] += saveIndex_p.ToString() + ".txt";
            path = pathParts[0];
        }
    }

    public static void ChooseLastSave()
    {
        if (!path.Contains("."))
        { path += lastGameSaveSlot.ToString() + ".txt"; }
        else
        {
            string[] pathParts = path.Split('.');
            pathParts[0] = pathParts[0].Remove(pathParts[0].Length - 1);
            pathParts[0] += lastGameSaveSlot.ToString() + ".txt";
            path = pathParts[0];
        }
    }

    public static void WriteSaveFile(Transform player)
    {
        if (!path.Contains("."))
        {
            Debug.LogWarning("No valid save path. Check the ChooseSlot or WriteSaveFile method. \nCreating new save slot in position 1.");
            path = Path.Combine(Application.streamingAssetsPath, "Save/GameSaves.txt");
            StreamWriter sWriter2 = new StreamWriter(path, false);
            sWriter2.WriteLine("Player Position:" + player.position.ToString());
            sWriter2.WriteLine("Player Rotation:" + player.rotation.ToString());

            // can write the lastGameSaveSlot into the file as well to keep track of the last save
            // when the player shuts the game down

            sWriter2.Close();
            return;
        }
        // true - add to file
        // false - overwrite file
        StreamWriter sWriter = new StreamWriter(path, false);
        sWriter.WriteLine("Player Position:" + player.position.ToString());
        sWriter.WriteLine("Player Rotation:" + player.rotation.ToString());

        // can write the lastGameSaveSlot into the file as well to keep track of the last save
        // when the player shuts the game down

        Debug.Log("Game saved successfully.");

        // writing is done
        sWriter.Close();
    }

    public static void ReadSaveFile()
    {
        // usually this will only happen on a new game or if the save files have been deleted, in which
        // case the player would have to start from scratch anyway
        if (!path.Contains("."))
        {
            Debug.LogWarning("No valid load path. If New Game, creating new save file with default information, otherwise check the ChooseSlot or ReadSaveFile method.");
            Transform player = GameObject.FindGameObjectWithTag("Player").transform;
            player.transform.position = Vector3.zero;
            player.transform.rotation = Quaternion.identity;
            WriteSaveFile(player);
            return;
        }

        // Read text from file
        StreamReader sReader = new StreamReader(path, true);

        // ref to the line we are reading
        string line;
        while ((line = sReader.ReadLine()) != null)
        {
            string[] parts = line.Split(':');

            switch (parts[0])
            {
                case "Player Position":
                    {
                        // position is a Vector3, convert our text file string to a Vec3
                        // assign the static player position to the converted Vec3
                        PlayerMovement.playerPos = StringToVector3(parts[1]);
                        break;
                    }
                case "Player Rotation":
                    {
                        // rotation is a Quaternion, convert our text file string to a Quaternion
                        // assign the static player rotation to the converted Quaternion
                        PlayerMovement.playerRot = StringToQuaternion(parts[1]);
                        break;
                    }
                default:
                    {
                        Debug.LogError("Something weird happened in ReadSaveFile method when comparing the string prior to the ':' with the switch statement cases.");
                        break;
                    }
            }
        }

        Debug.Log("Game loaded successfully.");
        sReader.Close();
    }


    /// <summary>
    /// Takes a string and parses it to a Vector3, then returns the result.
    /// </summary>
    /// <param name="sVector_p"></param>
    /// <returns></returns>
    public static Vector3 StringToVector3(string sVector_p)
    {
        // Remove the parentheses at the beginning and end of the string Vector
        if (sVector_p.StartsWith("(") && sVector_p.EndsWith(")"))
        { sVector_p = sVector_p.Substring(1, sVector_p.Length - 2); }

        // split the items by the commas
        string[] sArray = sVector_p.Split(',');

        try
        {
            // store as a Vector3
            // parsing the remaining string as a float (which will ignore the whitespace)
            Vector3 result = new Vector3(
                float.Parse(sArray[0]),
                float.Parse(sArray[1]),
                float.Parse(sArray[2]));
            Debug.Log(result);
            return result;
        }
        catch (Exception)
        { throw; }
    }

    public static Quaternion StringToQuaternion(string sQuat_p)
    {
        // Remove the parentheses at the beginning and end of the string Quaternion
        if (sQuat_p.StartsWith("(") && sQuat_p.EndsWith(")"))
        { sQuat_p = sQuat_p.Substring(1, sQuat_p.Length - 2); }

        // split the items by the commas
        string[] sArray = sQuat_p.Split(',');

        try
        {
            // store as a Quaternion (Vector4)
            // parsing the remaining string as a float (which will ignore the whitespace)
            Quaternion result = new Quaternion(
                float.Parse(sArray[0]),
                float.Parse(sArray[1]),
                float.Parse(sArray[2]),
                float.Parse(sArray[3]));
            Debug.Log(result);
            return result;
        }
        catch (Exception)
        { throw; }
    }

}

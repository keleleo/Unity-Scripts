using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
//By keleleo
public class Save
{
    //Save.SaveClass< Class >( object , path );
    public static void SaveClass<T>(T data, string path)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, data);
        stream.Close();
    }
    //Save.LoadClass< Class >( path );
    public static T LoadClass<T>(string path)
    {
        T data = Activator.CreateInstance<T>();

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            data = (T)formatter.Deserialize(stream);
            return data;
        }
        else
        {
            Debug.LogError("File Not Found in: " + path);
        }

        return data;
    }
}

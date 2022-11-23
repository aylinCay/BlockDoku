using System;
using System.Runtime.Serialization;
using UnityEngine.UI;

namespace BlockDoku
{
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using UnityEngine;

    public class BinaryDataStream : MonoBehaviour
    {
        public static void Save<T>(T serializedObject, string fileName)
        {
            string path = Application.persistentDataPath + "/saves";
            Directory.CreateDirectory(path);

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(path + fileName + ".dat",FileMode.Create);

            try
            {
              formatter.Serialize(fileStream,serializedObject);
            }
            catch (SerializationException e)
            {
                Debug.Log("Save filed.Error : " + e.Message);
            }
            finally
            {
                fileStream.Close();
            }
        }

        public static bool Exist(string fileName)
        {
            string path = Application.persistentDataPath + "/saves/";
            string fullFileName = fileName + ".dat";
            return File.Exists(path + fullFileName);
        }
    }

}
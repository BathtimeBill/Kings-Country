using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;

namespace BGG
{
    [Serializable]
    public class GameDataBase
    {
        public long dataObjectVersion = -1;
        public string dataObjectTimestamp = null;

        public DateTime GetTimestamp()
        {
            if (String.IsNullOrEmpty(dataObjectTimestamp))
                return new DateTime(0);
            else
                return DateTime.ParseExact(dataObjectTimestamp, GameData.dateFormat, null);
        }

        public virtual void CopyFrom(GameDataBase from)
        {
            dataObjectVersion = from.dataObjectVersion;
            dataObjectTimestamp = from.dataObjectTimestamp;
        }
    }

    public abstract class GameData : GameBehaviour
    {
        public bool dontDestroyOnLoad;
        // Change this to whatever you want
        protected string fileName = "data.sss";
        // The subdirectory for the save file
        string subDir = "Save";
        // Do we want to use Encryption
        public bool useEncryption = true;
        // The array of bytes we will use for our encryption key
        byte[] cryptoKey = { 0xF7, 0x24, 0x94, 0x08, 0x71, 0xE9, 0x64, 0x51, 0xC3, 0x5B, 0x84, 0x60, 0xCC, 0x55, 0x12, 0x76 };

        public static string dateFormat = "yyyy-MM-dd HH:mm:ss zzz";

        /// <summary>
        /// Gets the path of where the application is installed
        /// </summary>
        /// <returns>The path of the games instal location</returns>
        string GetPath()
        {
            return Application.dataPath.Substring(0,
                Application.dataPath.LastIndexOf('/')) + "/" + subDir + "/" + fileName;
        }

        protected string MakeTimestampNow()
        {
            return DateTime.Now.ToString(dateFormat); ;
        }

        // You must override this in the parent script
        public abstract void SaveData();
        // You must override this in the parent script
        public abstract void DeleteData();

        void OnApplicationQuit()
        {
            SaveData();
        }

        void OnApplicationFocus(bool appInFocus)
        {
            //if (!appInFocus)
            //    SaveData();
        }

        /// <summary>
        /// Loads our data as a GameDataObject type
        /// </summary>
        /// <typeparam name="T">The type of data to return</typeparam>
        /// <returns></returns>
        protected T LoadDataObject<T>() where T : SaveDataObject
        {
            // Ensure that the file exists
            if (File.Exists(GetPath()))
            {
                //Creates the File Stream for opening files
                FileStream stream = new FileStream(GetPath(), FileMode.Open);

                //Creates a stream reader and reads the stream
                StreamReader reader = new StreamReader(stream);

                //If we use encryption
                if (useEncryption)
                {
                    // Create a new AES instance
                    Aes aes = Aes.Create();

                    // Set our encryption mode to Cipher Block Chain
                    aes.Mode = CipherMode.CBC;

                    //Create an array of correct size based on ASE IV
                    byte[] outputIV = new byte[aes.IV.Length];

                    // Read the IV from the file
                    stream.Read(outputIV, 0, outputIV.Length);

                    // Create cryptostream around the filestream
                    CryptoStream cStream = new CryptoStream(stream, aes.CreateDecryptor(cryptoKey, outputIV), CryptoStreamMode.Read);

                    // Update the reader with our cryptostream
                    reader = new StreamReader(cStream);
                }

                //Read the entire file into a string value
                string jSave = reader.ReadToEnd();

                //Close the stream
                stream.Close();

                //Returns the string converted to json then to our GameData type
                return JsonUtility.FromJson<T>(jSave);
            }
            else
            {
                Debug.Log("Save file not found in " + GetPath());
                return null;
            }
        }

        /// <summary>
        /// Saves our data object to disk
        /// </summary>
        /// <typeparam name="T">The data type</typeparam>
        /// <param name="data">The data object to save</param>
        protected void SaveDataObject<T>(T data) where T : SaveDataObject
        {
            //Creates the save directory if it doesn't exist
            Directory.CreateDirectory(Path.GetDirectoryName(GetPath()));

            //Convert the This Game Data object into json then put into a string
            string jSave = JsonUtility.ToJson(data);

            //Create a filestream to create files
            FileStream stream = new FileStream(GetPath(), FileMode.Create);

            //Create our stream writer to write the data 
            StreamWriter writer = new StreamWriter(stream);

            // If we are using encryption
            if (useEncryption)
            {
                // Create a new AES instance
                Aes aes = Aes.Create();

                // Set our encryption mode to Cipher Block Chain
                aes.Mode = CipherMode.CBC;

                // Save newly generated IV
                byte[] inputIV = aes.IV;

                // Write the IV to the Filestream unencrypted
                stream.Write(inputIV, 0, inputIV.Length);

                // Create cryptostream wrapping filestream
                CryptoStream cStream = new CryptoStream(stream, aes.CreateEncryptor(cryptoKey, aes.IV), CryptoStreamMode.Write);

                // Create Streamwriter
                writer = new StreamWriter(cStream);

                // Write the innermost stream which we will encrypt
                writer.Write(jSave);

                //Close Streamwriter
                writer.Close();

                // Close Cryptostream
                cStream.Close();
            }
            else
            {
                //Write the data from the jSave string
                writer.Write(jSave);

                //Close the stream writer
                writer.Close();
            }

            //Close Filestream
            stream.Close();
        }

        /// <summary>
        /// Deletes our Game Data Object
        /// </summary>
        protected void DeleteDataObject()
        {
            if (File.Exists(GetPath()))
            {
                Debug.Log("Deleting file " + fileName);
                File.Delete(GetPath());
            }
            else
            {
                Debug.Log("No file found in " + GetPath());
            }
        }
    }
}
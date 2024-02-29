using Google.Protobuf;
using SurvivorDTO;
using System.Collections.Generic;
using System.IO;

namespace Protobuf.Utils
{
    public static class ProtobufUtils
    {
        public static void SaveObjectBinary(Dictionary<string, string> saveData, string filePath)
        {
            byte[] bytes = GetSaveableObject(saveData).ToByteArray();
            
            File.WriteAllBytes(filePath, bytes);
        }

        public static SaveableObject GetSaveableObject(Dictionary<string, string> saveData)
        {
            SaveableObject reflectedObject = new SaveableObject();

            foreach (string key in saveData.Keys)
            {
                reflectedObject.SaveData.Add(key, saveData[key]);
            }

            return reflectedObject;
        }

        public static SaveableObject LoadObjectBinary(string filePath)
        {
            SaveableObject reflectedSave = new SaveableObject();
            byte[] bytes = File.ReadAllBytes(filePath);

            reflectedSave.MergeFrom(bytes);
            return reflectedSave;
        }

        public static void SaveObjectList(ListContainer list, string filePath)
        {
            byte[] bytes = list.ToByteArray();

            File.WriteAllBytes(filePath, bytes);
        }

        public static ListContainer LoadObjectList(string filePath)
        {
            ListContainer list = new ListContainer();

            byte[] bytes = File.ReadAllBytes(filePath);
            list.MergeFrom(bytes);

            return list;
        }

        public static void SaveGameFileBinary(GameSaveFile gsf, string filePath)
        {
            byte[] bytes = gsf.ToByteArray();

            File.WriteAllBytes(filePath, bytes);
        }

        public static GameSaveFile LoadGameFileBinary(string filePath)
        {
            GameSaveFile gsf = new GameSaveFile();

            gsf.MergeFrom(File.ReadAllBytes(filePath));
            return gsf;
        }
    }
}

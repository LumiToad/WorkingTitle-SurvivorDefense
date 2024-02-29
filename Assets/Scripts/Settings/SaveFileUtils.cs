using Protobuf.Utils;
using Sirenix.Utilities;
using SurvivorDTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;

public static class SaveFileUtils
{
    private static string publicOSFolder { get => Application.persistentDataPath + Path.DirectorySeparatorChar; }
    private static string gameSaveFile = "save";
    private static string jsonFileExtension = ".json";
    private static string binaryFileExtension = ".sav";

    public static bool SaveClassToJson<T>(T classToFile, string fileName)
    {
        string pathToFile = GenerateFilePathByName(fileName, jsonFileExtension);
        string classAsJson = JsonUtility.ToJson(classToFile, true);
        File.WriteAllText(pathToFile, classAsJson);
        return File.Exists(pathToFile);
    }

    public static bool TryLoadClassFromJsonFile<T>(ref T classToOverride, string fileName)
    {
        string filePath = GenerateFilePathByName(fileName, jsonFileExtension);
        if (!JsonSaveFileExists(fileName)) return false;
        classToOverride = LoadClassFromJsonFileInternal<T>(filePath);

        return true;
    }

    private static T LoadClassFromJsonFileInternal<T>(string pathToFile)
    {
        string classAsJson = File.ReadAllText(pathToFile);
        return JsonUtility.FromJson<T>(classAsJson);
    }

    public static void DeleteSaveFileJSON(string fileName)
    {
        string pathToFile = GenerateFilePathByName(fileName, jsonFileExtension);
        if (JsonSaveFileExists(fileName))
        {
            File.Delete(pathToFile);
        }
    }

    public static void DeleteSaveFileBinary(string fileName)
    {
        string pathToFile = GenerateFilePathByName(fileName, binaryFileExtension);

        if (BinarySaveFileExists(fileName))
        {
            File.Delete(pathToFile);
        }
    }

    public static bool JsonSaveFileExists(string fileName)
    {
        string pathToFile = GenerateFilePathByName(fileName, jsonFileExtension);
        return File.Exists(pathToFile);
    }

    public static bool BinarySaveFileExists(string fileName)
    {
        string pathToFile = GenerateFilePathByName(fileName, binaryFileExtension);
        return File.Exists(pathToFile);
    }

    private static string GenerateFilePathByName(string fileName, string fileExtension)
    {
        return publicOSFolder + fileName + fileExtension;
    }

    public static bool SaveObjectBinary<T>(T data, string name, string fileName)
    {
        string pathToFile = GenerateFilePathByName(fileName, binaryFileExtension);

        var saveDict = GetSaveDict<T>(data, name);

        ProtobufUtils.SaveObjectBinary(saveDict, pathToFile);

        return BinarySaveFileExists(pathToFile);
    }

    private static Dictionary<string, string>GetSaveDict<T>(T data, string name)
    {
        Dictionary<string, string> saveDict = new();
        Type type = data.GetType();
        saveDict.Add(type.Name, name);
        FieldInfo[] dataFields = type.GetFields();

        foreach (FieldInfo field in dataFields)
        {
            if (field.GetValue(data) == null) continue;

            string nameAsString = field.Name;
            object value = field.GetValue(data);
            string valueAsString = field.GetValue(data).ToString();
            if (field.FieldType.IsEnum)
            {
                valueAsString = Convert.ToInt32(value).ToString();
            }

            valueAsString = valueAsString.IsNullOrWhitespace() ? string.Empty : valueAsString;
            if (valueAsString == string.Empty) continue;
            saveDict.Add(nameAsString, valueAsString);
        }

        return saveDict;
    }

    public static bool LoadObjectBinary<T>(ref T data, string fileName)
    {
        if (!BinarySaveFileExists(fileName)) return false;

        string pathToFile = GenerateFilePathByName(fileName, binaryFileExtension);

        SaveableObject loaded = ProtobufUtils.LoadObjectBinary(pathToFile);
        return GetObjectBinary(ref data, loaded);
    }

    private static bool GetObjectBinary<T>(ref T data, SaveableObject loaded)
    {
        Type type = typeof(T);

        if (!loaded.SaveData.ContainsKey(type.Name)) return false;
        loaded.SaveData.Remove(type.Name);

        FieldInfo[] dataFields = type.GetFields();

        foreach (string key in loaded.SaveData.Keys)
        {
            foreach (FieldInfo field in dataFields)
            {
                if (field.Name != key) continue;

                string? valueAsString = loaded.SaveData[key];
                valueAsString = valueAsString.IsNullOrWhitespace() ? null : valueAsString;

                if (valueAsString == null) continue;
                Type fieldType = (field.FieldType.IsEnum) ? typeof(int) : field.FieldType;
                field.SetValue(data, Convert.ChangeType(valueAsString, fieldType));
            }
        }

        return true;
    }

    public static bool SaveObjectListBinary<T>(List<T> list, string name, string fileName)
    {
        string pathToFile = GenerateFilePathByName(fileName, binaryFileExtension);
        ListContainer objectList = new ListContainer();

        foreach (T item in list)
        {
            var saveDict = GetSaveDict(item, name);
            objectList.Container.Add(ProtobufUtils.GetSaveableObject(saveDict));
        }

        ProtobufUtils.SaveObjectList(objectList, pathToFile);

        return BinarySaveFileExists(pathToFile);
    }

    public static bool LoadObjectListBinary<T>(T sample, ref List<T> list, string fileName) where T : new()
    {
        string pathToFile = GenerateFilePathByName(fileName, binaryFileExtension);

        ListContainer listContainer = ProtobufUtils.LoadObjectList(pathToFile);
        int size = listContainer.Container.Count;

        foreach (SaveableObject loaded in listContainer.Container)
        {
            list.Add((T)sample);
        }

        int iterator = 0;
        foreach (SaveableObject loaded in listContainer.Container) 
        {
            T data = new T();
            if (!GetObjectBinary(ref data, loaded)) continue;
            list[iterator] = (T)data;
            iterator++;
        }

        return BinarySaveFileExists(pathToFile);
    }

    public static bool SaveGameFileBinary(List<SaveableAchievement> achievements)
    {
        GameSaveFile gsf = new GameSaveFile();

        foreach (var ach in achievements) 
        {
            gsf.Achievements.Add(ach);
        }

        string pathToFile = GenerateFilePathByName(gameSaveFile, binaryFileExtension);

        ProtobufUtils.SaveGameFileBinary(gsf, pathToFile);

        return BinarySaveFileExists(gameSaveFile);
    }

    public static bool LoadGameFileBinary(ref GameSaveFile gsf)
    {
        if (!BinarySaveFileExists(gameSaveFile)) return false;

        string pathToFile = GenerateFilePathByName(gameSaveFile, binaryFileExtension);
        gsf.MergeFrom(ProtobufUtils.LoadGameFileBinary(pathToFile));

        return true;
    }
}
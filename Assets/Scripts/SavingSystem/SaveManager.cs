using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public static class SaveManager
{
    private static Dictionary<Type, object> models = new Dictionary<Type, object>();
    private static JsonConverter[] customConverters = new JsonConverter[]
    {
        new RectConverter(),
        new JsonInt32Converter(),
        new Vector2Converter(),
        new Vector3Converter(),
        new QuaternionConverter(),
    };

    private static string bankFolder = string.Empty;

    private const string defaultApplicationFolderName = ".timlab";

    public static void SetBankFolder(string dictionary)
    {
        bankFolder = dictionary;
    }

    public static void ClearBankFolder(string dictionary)
    {
        var applicationDirectory = GetSaveDirectoryPath();

        if (Directory.Exists(applicationDirectory))
        {
            System.IO.DirectoryInfo di = new DirectoryInfo(applicationDirectory);

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }
        }
    }

    public static T GetSaveModel<T>(string prefix)
    {
        var type = typeof(T);

        if (!models.ContainsKey(type))
        {
            models.Add(type, LoadModel<T>(prefix));
        }

        return (T)models[type];
    }

    public static T LoadModel<T>(string prefix)
    {
        var model = LoadModel(typeof(T), prefix);

        return (T)model;
    }

    public static object LoadModel(Type type, string prefix)
    {
        prefix = prefix.Replace('/', '\\');

        try
        {
            var typeName = type.Name;
            var applicationDirectory = Path.Combine(GetSaveDirectoryPath(), prefix);
            var filePath = Path.Combine(applicationDirectory, typeName);

            if (File.Exists(filePath))
            {
                var data = File.ReadAllText(filePath);

                return JsonConvert.DeserializeObject(data, type, customConverters);
            }
            else
            {
                return default;
            }
        }
        catch(Exception e)
        {
            Debug.LogError(e.Message);
            return default;
        }
    }

    public static void SaveModel(object model, string prefix)
    {
        try
        {
            if (model == null)
            {
                return;
            }

            var data = JsonConvert.SerializeObject(model, customConverters);
            var type = model.GetType();
            var typeName = type.Name;

            if (models.ContainsKey(type))
            {
                models[type] = model;
            }
            else
            {
                models.Add(type, model);
            }

            var applicationDirectory = Path.Combine(GetSaveDirectoryPath(), prefix);
            var filePath = Path.Combine(applicationDirectory, typeName);

            if (!Directory.Exists(applicationDirectory))
            {
                Directory.CreateDirectory(applicationDirectory);
            }

            File.WriteAllText(filePath, data);
        }
        catch(Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    public static string GetSaveDirectoryPath()
    {
        var rootDirectory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var saveDirectory = Path.Combine(rootDirectory, defaultApplicationFolderName);

        if (!string.IsNullOrEmpty(bankFolder))
        {
            saveDirectory = Path.Combine(saveDirectory, bankFolder);
        }

        return saveDirectory;
    }
}

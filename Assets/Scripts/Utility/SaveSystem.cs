using System.IO;
using UnityEngine;

public static class SaveSystem
{
    public const string MINIONS_SAVE_NAME = "minionsSavedData.txt";
    public const string LEVEL_PROGRESS_SAVE_NAME = "LevelProgressData.txt";
    public const string SQUAD_ORDER_SAVE_NAME = "squadOrderSavedData.txt";
    public const string CURRENCY_SAVE_NAME = "currencySavedData.txt";
    public const string TUTORIAL_SAVE_NAME = "tutorialSavedData.txt";


    public static bool canSave = true;

    public static void Save<T>(T data, string fileName)
    {
        if (!canSave) return;

        var path = Path.Combine(Application.persistentDataPath, fileName);

        string jsonString = JsonUtility.ToJson(data);

        using (StreamWriter streamWriter = File.CreateText(path))
        {
            streamWriter.Write(jsonString);
        }
    }

    public static T Load<T>(string fileName)
    {
        var path = Path.Combine(Application.persistentDataPath, fileName);

        if (File.Exists(path))
        {
            using (StreamReader streamReader = File.OpenText(path))
            {
                string jsonString = streamReader.ReadToEnd();
                return JsonUtility.FromJson<T>(jsonString);
            }
        }

        return default(T);
    }

    public static void DeleteFile(string fileName)
    {
        var path = Path.Combine(Application.persistentDataPath, fileName);

        if (File.Exists(path))
        {
            try
            {
                File.Delete(path);
            }
            catch (System.Exception er)
            {
                Debug.Log("Message "+ er.Message);
                Debug.Log("Inner Exception " + er.InnerException);
                Debug.Log("StackTrace " + er.StackTrace);
                throw;
            }
        }
    }
}

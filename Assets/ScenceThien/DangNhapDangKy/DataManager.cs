using UnityEngine;
using System.IO;

public static class UserDataManager
{
    private static string filePath => Application.persistentDataPath + "/user_data.json";

    public static UserDatabase Load()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            return JsonUtility.FromJson<UserDatabase>(json);
        }

        return new UserDatabase();
    }

    public static void Save(UserDatabase db)
    {
        string json = JsonUtility.ToJson(db, true);
        File.WriteAllText(filePath, json);
    }
}

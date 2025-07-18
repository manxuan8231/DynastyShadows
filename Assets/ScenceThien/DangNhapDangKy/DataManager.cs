using UnityEngine;
using System.IO;

public static class UserDataManager
{
    private static string filePath = Application.persistentDataPath + "/users.json";

    public static UserDatabase Load()
    {
        if (!File.Exists(filePath))
        {
            return new UserDatabase(); 
        }

        string json = File.ReadAllText(filePath);
        return JsonUtility.FromJson<UserDatabase>(json);
    }

    public static void Save(UserDatabase db)
    {
        string json = JsonUtility.ToJson(db, true);
        File.WriteAllText(filePath, json);
    }
}

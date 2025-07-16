using System.IO;
using UnityEngine;

public class SaveManagerMan : MonoBehaviour
{
    private static string fileName = "saveData.json";
   
    public static void SaveGame(GameSaveData data)
    {
        string json = JsonUtility.ToJson(data, true); // true để dễ đọc
        string path = GetSavePath();

        File.WriteAllText(path, json);
        Debug.Log($"Game saved to: {path}");
    }

    public static GameSaveData LoadGame()
    {
        string path = GetSavePath();

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            GameSaveData data = JsonUtility.FromJson<GameSaveData>(json);
            Debug.Log($"Game loaded from: {path}");
            return data;
        }
        else
        {
            Debug.Log("No save file found, returning new save data.");
            return new GameSaveData();
        }
    }

    public static void DeleteSave()
    {
        string path = GetSavePath();
        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("Save file deleted.");
        }
    }

    private static string GetSavePath()
    {
        return Path.Combine(Application.persistentDataPath, fileName);
    }

    
   


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            string path = GetSavePath();
            if (File.Exists(path))
            {
                File.Delete(path);
                Debug.Log("Save file deleted.");
            }
        }
    }
}

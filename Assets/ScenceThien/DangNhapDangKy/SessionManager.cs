using System.IO;
using UnityEngine;

public static class SessionManager
{
    private static string sessionFilePath = Application.persistentDataPath + "/session.json";

    public static void SaveSession(string username)
    {
        SessionData session = new SessionData { loggedInUser = username };
        string json = JsonUtility.ToJson(session, true);
        File.WriteAllText(sessionFilePath, json);
    }

    public static string LoadSession()
    {
        if (!File.Exists(sessionFilePath))
            return null;

        string json = File.ReadAllText(sessionFilePath);
        SessionData session = JsonUtility.FromJson<SessionData>(json);
        return session.loggedInUser;
    }

    public static void ClearSession()
    {
        if (File.Exists(sessionFilePath))
            File.Delete(sessionFilePath);
    }
}

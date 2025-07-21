using System.IO;
using UnityEngine;

public static class SessionManager
{
    private static string sessionFilePath = Application.persistentDataPath + "/session.json";

    public static void SaveSession(string username)
    {
        SessionData session = new SessionData { loggedInUser = username };
        string sessionJson = JsonUtility.ToJson(session, true);
        File.WriteAllText(Application.persistentDataPath + "/session.json", sessionJson);

    }

    public static string LoadSession()
    {
        if (!File.Exists(sessionFilePath))
            return null;
        string sessionJson = File.ReadAllText(Application.persistentDataPath + "/session.json");
        SessionData session = JsonUtility.FromJson<SessionData>(sessionJson);
        string currentUser = session.loggedInUser;
        return session.loggedInUser;
    }

    public static void ClearSession()
    {
        if (File.Exists(sessionFilePath))
            File.Delete(sessionFilePath);
    }
}

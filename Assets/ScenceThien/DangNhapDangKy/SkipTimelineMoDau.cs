using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class TimelineDone : MonoBehaviour
{
    public string mainScene = "MainScene";

    public void OnTimelineFinished()
    {
        string userFile = Application.persistentDataPath + "/users.json";
        string currentUserPath = Application.persistentDataPath + "/current_user.txt";

        if (!File.Exists(userFile) || !File.Exists(currentUserPath)) return;

        string currentUser = File.ReadAllText(currentUserPath);
        string json = File.ReadAllText(userFile);
        UserDatabase db = JsonUtility.FromJson<UserDatabase>(json);

        foreach (var user in db.users)
        {
            if (user.username == currentUser)
            {
                user.hasSeenTimeline = true;
                break;
            }
        }


        string updatedJson = JsonUtility.ToJson(db, true);
        File.WriteAllText(userFile, updatedJson);

        // Chuyển cảnh
        SceneManager.LoadScene(mainScene);
    }
}

using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.IO;

public class LoginManager : MonoBehaviour
{
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;
    public TMP_Text messageText;
    public string nextScene = "MainScene";
    private string filePath;

    void Awake()
    {
        filePath = Application.persistentDataPath + "/users.json";
    }

    public void TryLogin()
    {
        string username = usernameInput.text;
        string password = passwordInput.text;

        string path = Application.persistentDataPath + "/users.json";

        if (!File.Exists(path))
        {
            messageText.text = "Không tìm thấy file dữ liệu!";
            return;
        }

        string json = File.ReadAllText(path);
        UserDatabase db = JsonUtility.FromJson<UserDatabase>(json);

        foreach (var user in db.users)
        {
            if (user.username == username && user.password == password)
            {
                // Ghi tên user đang đăng nhập
                File.WriteAllText(Application.persistentDataPath + "/current_user.txt", username);

                // Nếu chưa xem Timeline → vào Timeline trước
                if (!user.hasSeenTimeline)
                {
                    SceneManager.LoadScene("TimelineMoDau");
                }
                else
                {
                    // Nếu đã xem timeline → load scene đã lưu
                    string savePath = Application.persistentDataPath + "/saveData.json";

                    if (File.Exists(savePath))
                    {
                        string savedJson = File.ReadAllText(savePath);
                        GameSaveData saveData = JsonUtility.FromJson<GameSaveData>(savedJson);

                        if (!string.IsNullOrEmpty(saveData.savedSceneName))
                        {
                           
                            SceneManager.LoadScene(saveData.savedSceneName);
                        }
                        else
                        {
                            SceneManager.LoadScene("Map1"); // fallback nếu chưa có tên scene
                        }
                    }
                    else
                    {
                        SceneManager.LoadScene("Map1"); // fallback nếu chưa từng lưu
                    }
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                }
                return;
            }
        }

        messageText.text = "Sai tài khoản hoặc mật khẩu!";
    }

}

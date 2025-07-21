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
                // Ghi lại tên người dùng đang đăng nhập
                File.WriteAllText(Application.persistentDataPath + "/current_user.txt", username);

                // Kiểm tra trạng thái đã xem timeline chưa
                if (user.hasSeenTimeline)
                    SceneManager.LoadScene("Map1");
                else
                    SceneManager.LoadScene("TimelineMoDau");
                return;
            }
        }

        messageText.text = "Sai tài khoản hoặc mật khẩu!";
    }

}

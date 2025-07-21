using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.IO;

public class LoginManager : MonoBehaviour
{
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;
    public TMP_Text messageText;
    public string nextScene = "MainScene"; // Tên scene sẽ chuyển sau khi đăng nhập

    private string filePath;

    [System.Serializable]
    public class User
    {
        public string username;
        public string password;
    }

    [System.Serializable]
    public class UserDatabase
    {
        public User[] users;
    }

    void Awake()
    {
        filePath = Application.persistentDataPath + "/users.json";
    }

    public void TryLogin()
    {
        string username = usernameInput.text;
        string password = passwordInput.text;

        if (!File.Exists(filePath))
        {
            messageText.text = "Không tìm thấy dữ liệu người dùng!";
            return;
        }

        string json = File.ReadAllText(filePath);
        UserDatabase db = JsonUtility.FromJson<UserDatabase>(json);

        foreach (var user in db.users)
        {
            if (user.username == username && user.password == password)
            {
                messageText.text = "Đăng nhập thành công!";
                SceneManager.LoadScene(nextScene);
                return;
            }
        }

        messageText.text = "Sai tài khoản hoặc mật khẩu!";
    }
}

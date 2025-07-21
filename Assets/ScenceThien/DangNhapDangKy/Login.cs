using System.IO;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
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

    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;
    public Button loginButton;
    public TMP_Text messageText;
    public string nextScene = "MainGameScene"; // Đổi tên scene nếu cần

    private string filePath;

    void Awake()
    {
        filePath = Application.persistentDataPath + "/users.json";
        loginButton.interactable = false;

        usernameInput.onValueChanged.AddListener(delegate { ValidateInputs(); });
        passwordInput.onValueChanged.AddListener(delegate { ValidateInputs(); });


    }

    void ValidateInputs()
    {
        loginButton.interactable =
            !string.IsNullOrWhiteSpace(usernameInput.text) &&
            !string.IsNullOrWhiteSpace(passwordInput.text);
    }

    public void TryLogin()
    {
        string username = usernameInput.text;
        string password = passwordInput.text;

        if (!File.Exists(filePath))
        {
            messageText.text = "Chưa có dữ liệu người dùng!";
            return;
        }

        string json = File.ReadAllText(filePath);
        UserDatabase db = JsonUtility.FromJson<UserDatabase>(json);

        foreach (var user in db.users)
        {
            if (user.username == username && user.password == password)
            {
                PlayerPrefs.SetString("LoggedInUser", username);
                PlayerPrefs.Save();

                messageText.text = "Đăng nhập thành công!";
                SceneManager.LoadScene(nextScene);
                return;
            }
        }

        messageText.text = "Sai tên đăng nhập hoặc mật khẩu!";
    }

    public void Logout()
    {
        PlayerPrefs.DeleteKey("LoggedInUser");
        PlayerPrefs.Save();
    }
}

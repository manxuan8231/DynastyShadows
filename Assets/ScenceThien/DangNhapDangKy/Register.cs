using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class RegisterManager : MonoBehaviour
{
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;
    public TMP_Text messageText;

    public string loginScene = "Login";

    public void Register()
    {
        string username = usernameInput.text.Trim();
        string password = passwordInput.text.Trim();

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            messageText.text = "❌ Tên đăng nhập và mật khẩu không được để trống!";
            return;
        }

        UserDatabase db = UserDataManager.Load();

        foreach (var user in db.users)
        {
            if (user.username == username)
            {
                messageText.text = "❌ Tài khoản đã tồn tại!";
                return;
            }
        }

        UserData newUser = new UserData
        {
            username = username,
            password = password
        };

        db.users.Add(newUser);
        UserDataManager.Save(db);

        messageText.text = "✅ Đăng ký thành công!";
        Invoke(nameof(GoToLoginScene), 1.5f); // Delay nhẹ để người dùng thấy thông báo
    }

    public void GoToLoginScene()
    {
        SceneManager.LoadScene(loginScene);
    }
}

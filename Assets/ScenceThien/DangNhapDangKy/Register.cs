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
        string username = usernameInput.text;
        string password = passwordInput.text;

        UserDatabase db = UserDataManager.Load();

        foreach (var user in db.users)
        {
            if (user.username == username)
            {
                messageText.text = "Tài khoản đã tồn tại!";
                return;
            }
        }

        UserData newUser = new UserData { username = username, password = password, hasSeenTimeline = false };
        db.users.Add(newUser);
        UserDataManager.Save(db);

        messageText.text = "Đăng ký thành công!";
        GoToLoginScene();
    }

    public void GoToLoginScene()
    {
        SceneManager.LoadScene(loginScene);
    }
}

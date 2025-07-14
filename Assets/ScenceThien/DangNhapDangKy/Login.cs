using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;
    public TMP_Text messageText;
    public string nextScene = "TimelineMoDau";

    public void Login()
    {
        string username = usernameInput.text;
        string password = passwordInput.text;

        UserDatabase db = UserDataManager.Load();

        foreach (var user in db.users)
        {
            if (user.username == username)
            {
                if (user.password == password)
                {
                    messageText.text = "? dang nhap thanh cong";
                    SceneManager.LoadScene(nextScene);
                    return;
                }
                else
                {
                    messageText.text = "? Sai mat khau!";
                    return;
                }
            }
        }

        messageText.text = "? Tài khoan không ton tai!";
    }

    public void GoToRegisterScene(string registerScene)
    {
        SceneManager.LoadScene(registerScene);
    }
}

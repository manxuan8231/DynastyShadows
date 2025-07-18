using UnityEngine;
using UnityEngine.SceneManagement;

public class AutoLogin : MonoBehaviour
{
    public string SceneGame = "MainScene";
    void Start()
    {
        string loggedUser = PlayerPrefs.GetString("LoggedInUser", "");

        if (!string.IsNullOrEmpty(loggedUser))
        {
            Debug.Log("Auto login as: " + loggedUser);
            SceneManager.LoadScene(SceneGame); 
        }
    }
}

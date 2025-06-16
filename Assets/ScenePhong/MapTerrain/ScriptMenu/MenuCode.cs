using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuCode : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Regis(string Registant)
    {
        SceneManager.LoadScene(Registant);
    }

    public void Logi(string Login)
    {
        SceneManager.LoadScene(Login);
    }

    public void Backtomainmenu(string MainMenu)
    {
        SceneManager.LoadScene(MainMenu);
    }

    public void ResAndLog(string ReAndLog)
    {
        SceneManager.LoadScene(ReAndLog);
    }
    public void PlayGame(string timeLime)
    {
        SceneManager.LoadScene(timeLime);
    }
    public void Quit()
    {
        Application.Quit();
        Debug.Log("Player has quit the game");
    }
}

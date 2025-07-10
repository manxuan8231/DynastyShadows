using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistentMusic : MonoBehaviour
{
    public string[] allowedScenes; // Danh sách các scene cho phép giữ nhạc

    private static PersistentMusic instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            // Lắng nghe sự kiện scene thay đổi
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject); // Tránh tạo nhiều nhạc trùng
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        bool isAllowed = false;
        foreach (string sceneName in allowedScenes)
        {
            if (scene.name == sceneName)
            {
                isAllowed = true;
                break;
            }
        }

        if (!isAllowed)
        {
            Destroy(gameObject); // Nếu scene không thuộc danh sách -> hủy object nhạc
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroyPrefabs : MonoBehaviour
{
    public GameObject playerPre;
    public Vector3 posNextScence = new Vector3(0, 0, 0);

    


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
          DontDestroyOnLoad(playerPre);
            SceneManager.sceneLoaded += OnScenceLoad;
          SceneManager.LoadScene("Map2");
        }
    }
    public void OnScenceLoad( Scene scence, LoadSceneMode mod)
    {
        playerPre.transform.position = posNextScence;
        SceneManager.sceneLoaded -= OnScenceLoad;
    }
}
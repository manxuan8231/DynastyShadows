using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroyPrefabs : MonoBehaviour
{
    public GameObject playerPre;
  

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
          DontDestroyOnLoad(playerPre);
            SceneManager.sceneLoaded += OnScenceLoad;
          SceneManager.LoadScene("Map2");
        }
    }
    public void OnScenceLoad(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnScenceLoad;
       
    }

   
}
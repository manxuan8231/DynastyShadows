using System.Collections;
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
    public void OnScenceLoad(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnScenceLoad;
        StartCoroutine(WaitAndSetPosition());
    }

    private IEnumerator WaitAndSetPosition()
    {
        yield return null; // chờ 1 frame
        CharacterController cc = playerPre.GetComponent<CharacterController>();
        cc.enabled = false; // tắt CharacterController để tránh lỗi khi đặt vị trí
        playerPre.transform.position = posNextScence;
        cc.enabled = true; // bật lại CharacterController
    }
}
using System.Collections;
using UnityEngine;

public class ChuyenScence : MonoBehaviour
{
    public string scenceName;
    public float thoiGianChuyen = 65f; // Thời gian chuyển cảnh
    void Start()
    {
        StartCoroutine(ChuyenScenceGame());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator ChuyenScenceGame()
    {
        yield return new WaitForSeconds(thoiGianChuyen);
        UnityEngine.SceneManagement.SceneManager.LoadScene(scenceName);
    }
}

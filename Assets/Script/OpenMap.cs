using UnityEngine;

public class OpenMap : MonoBehaviour
{
    public GameObject mapUI;
    public AudioSource mapAudio;
    public AudioClip mapClip;

    void Start()
    {
        mapUI.SetActive(false);
        mapAudio = GetComponent<AudioSource>();

        // Ẩn chuột lúc bắt đầu nếu cần
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (mapUI.activeSelf)
            {
                mapAudio.PlayOneShot(mapClip);
                mapUI.SetActive(false);
                Time.timeScale = 1f;

                // Ẩn chuột khi tắt map
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                mapAudio.PlayOneShot(mapClip);
                mapUI.SetActive(true);
                Time.timeScale = 0f;

                // Hiện chuột khi mở map
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }
}

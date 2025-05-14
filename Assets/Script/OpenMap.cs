using UnityEngine;

public class OpenMap : MonoBehaviour
{
    public GameObject mapUIConten;
    public GameObject mapUIBG;
    public AudioSource mapAudio;
    public AudioClip mapClip;

    void Start()
    {
        mapUIBG.SetActive(false);
        mapUIConten.SetActive(false);
        mapAudio = GetComponent<AudioSource>();

        // Ẩn chuột lúc bắt đầu nếu cần
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (mapUIConten.activeSelf)
            {
                mapAudio.PlayOneShot(mapClip);
                mapUIConten.SetActive(false);
                mapUIBG.SetActive(false);
                Time.timeScale = 1f;

                // Ẩn chuột khi tắt map
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                mapAudio.PlayOneShot(mapClip);
                mapUIConten.SetActive(true);
                mapUIBG.SetActive(true);
                Time.timeScale = 0f;

                // Hiện chuột khi mở map
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }
}

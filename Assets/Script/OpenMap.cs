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
    }

   
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            if (mapUI.activeSelf)
            {
                mapAudio.PlayOneShot(mapClip);
                mapUI.SetActive(false);
                Time.timeScale = 1f;
            }
            else
            {
                mapAudio.PlayOneShot(mapClip);
                mapUI.SetActive(true);
                Time.timeScale = 0f; 
            }
        }
    }
}

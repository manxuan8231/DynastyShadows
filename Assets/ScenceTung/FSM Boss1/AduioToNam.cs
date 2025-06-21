using UnityEngine;

public class AduioToNam : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip roarSound;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void RoarPlay()
    {
        audioSource.PlayOneShot(roarSound);    }
}

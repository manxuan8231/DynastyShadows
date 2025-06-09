using UnityEngine;

public class AudioBoss1 : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] attackAudioClips;
    public AudioClip[] skillAudioClips;
    public AudioClip[] deathAudioClips;
    public AudioClip runSound;
    public AudioClip roarSound;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        
    }

  
}

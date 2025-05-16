using UnityEngine;

public class DrakonitAudioManager : MonoBehaviour
{
    public AudioSource audioSource; // Âm thanh của enemy
    //Am thanh enemy
    public AudioClip attackSound; // Âm thanh tấn công
    public AudioClip roar;// Âm thanh gầm  
    public AudioClip SoundHit; // Âm thanh Hit
    public AudioClip SoundDie; // Âm thanh chết
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        
    }
    public void SoundAttack()
    {
        audioSource.PlayOneShot(attackSound); // Phát âm thanh tấn công
    }
}

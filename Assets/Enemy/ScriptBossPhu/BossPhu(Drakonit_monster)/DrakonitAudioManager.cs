using UnityEngine;

public class DrakonitAudioManager : MonoBehaviour
{
    public AudioSource audioSource; // Âm thanh của enemy
    public AudioClip backGroundSound; // Âm thanh nen
    //Am thanh enemy
    public AudioClip attackSound; // Âm thanh tấn công
    public AudioClip roar;// Âm thanh gầm  
    public AudioClip SoundHit; // Âm thanh Hit
    public AudioClip SoundDie; // Âm thanh chết
    public AudioClip SoundRuning; // Âm thanh skill
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
    public void playRunning()
    {
        audioSource.PlayOneShot(SoundRuning); // Phát âm thanh chạy
    }
}

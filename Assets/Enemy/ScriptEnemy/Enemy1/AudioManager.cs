using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource AudioSource;
    public AudioClip deathSound;
    public AudioClip attackSound;
    public AudioClip punchSound; 
    public AudioClip hitSound;
 
    public AudioClip runSound;
    void Start()
    {
        AudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlayRunSound()
    {
        AudioSource.PlayOneShot(runSound);
    }
 
    public void PlayAttackSound()
    {
        AudioSource.PlayOneShot(attackSound);
    }
    public void PlayPunchSound()
    {
        AudioSource.PlayOneShot(punchSound);
    }
    public void PlayHitSound()
    {
        AudioSource.PlayOneShot(hitSound);
    }
    public void PlayDeathSound()
    {
        AudioSource.PlayOneShot(deathSound);
    }
}


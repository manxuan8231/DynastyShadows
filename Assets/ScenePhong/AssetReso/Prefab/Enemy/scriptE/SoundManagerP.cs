using UnityEngine;

public class AudioManagerP : MonoBehaviour
{
    public AudioSource AudioSource;
    public AudioClip deathSound;
    public AudioClip attackSound;
    public AudioClip punchSound;
    public AudioClip hitSound;
    public AudioClip parrySound;

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

    public void PlayParrySound()
    {
        AudioSource.PlayOneShot(parrySound);
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


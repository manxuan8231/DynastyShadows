using UnityEngine;

public class NecAudioManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip audopBackgroud;
    public AudioClip attack1Sound;
    public AudioClip attack2Sound;
    public AudioClip walkSound;
    public AudioClip skill1Sound;
    public AudioClip skill2Sound;
    public AudioClip skill3Sound;
    public AudioClip deathClip;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

    }


    public void PlaySoundAttack()
    {
        audioSource.PlayOneShot(attack1Sound);
    }
    public void PlaySoundAttack2()
    {
        audioSource.PlayOneShot(attack2Sound);
    }  
    public void WalkSound()
    {
        audioSource.PlayOneShot(walkSound);

    }
}

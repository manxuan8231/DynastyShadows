using UnityEngine;

public class AudioBoss1 : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip attackAudioClips1;
    public AudioClip attackAudioClips2;
   public AudioClip attackAudioClips3; 

    public AudioClip skillAudioClips1;
    public AudioClip skillAudioClips2;
     public AudioClip skillAudioClips3; 
    public AudioClip skillAudioClips4;
    public AudioClip deathAudioClips;
    public AudioClip runSound;
    

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        
    }
    //dừng toàn bộ âm thanh
    public void StopAllAudio()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }
    public void PlayAttackSound1()
    {
        audioSource.PlayOneShot(attackAudioClips1);

    }

    public void PlayAttackSound2()
    {
        audioSource.PlayOneShot(attackAudioClips2);


    }
    public void PlayAttackSound3()
    {
       
            audioSource.PlayOneShot(attackAudioClips3);
        
    }
    public void PlaySkillSound1()
    {
        
            audioSource.PlayOneShot(skillAudioClips1);
      
    }
    public void PlaySkillSound2()
    {
        
            audioSource.PlayOneShot(skillAudioClips2);
        
    }
    public void PlaySkillSound3()
    {
      
            audioSource.PlayOneShot(skillAudioClips3);
        
    }
    public void PlaySkillSound4()
    {
       
            audioSource.PlayOneShot(skillAudioClips4);
        
    }
    public void PlayDeathSound()
    {
        
           audioSource.PlayOneShot(deathAudioClips);
        
    }
    public void PlayRunSound()
    {
        
            audioSource.PlayOneShot(runSound);
        
    }



}

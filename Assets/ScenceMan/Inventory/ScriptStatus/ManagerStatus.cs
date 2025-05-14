using TMPro;
using UnityEngine;
using UnityEngine.Audio;

public class ManagerStatus : MonoBehaviour
{
    //ke thua
    LevelAvatar levelAvatar;
    PlayerStatus playerStatus;

    public AudioSource audioSource;
    public AudioClip clipClick;
  
   
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        playerStatus = FindAnyObjectByType<PlayerStatus>();
        levelAvatar = FindAnyObjectByType<LevelAvatar>();
       
    }

   
    void Update()
    {
        
    }
    public void ButtonUpHealth()
    {
        if (levelAvatar.score > 0)
        {
            levelAvatar.TakeScore(1);
            audioSource.PlayOneShot(clipClick);
            playerStatus.UpMaxHealth(200);
        }
    }
    public void ButtonUpManaSkill()
    {
        if (levelAvatar.score > 0)
        {
            levelAvatar.TakeScore(1);
            audioSource.PlayOneShot(clipClick);
            playerStatus.UpMaxManaSkill(200);
        }
    }
    public void ButtonUpMana()
    {
        if (levelAvatar.score > 0)
        {
            levelAvatar.TakeScore(1);
            audioSource.PlayOneShot(clipClick);
            playerStatus.UpMaxMana(200);
        }
    }
}

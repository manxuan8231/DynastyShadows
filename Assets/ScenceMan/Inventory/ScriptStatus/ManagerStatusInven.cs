using TMPro;
using UnityEngine;
using UnityEngine.Audio;

public class ManagerStatusInven : MonoBehaviour
{


    public AudioSource audioSource;
    public AudioClip clipClick;

    //ke thua
    LevelAvatar levelAvatar;//quan ly avatarlv
    PlayerStatus playerStatus;//quan ly mau mana



    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        playerStatus = FindAnyObjectByType<PlayerStatus>();
        levelAvatar = FindAnyObjectByType<LevelAvatar>();

    }


    void Update()
    {

    }
    public void ButtonUpHealth()//up mau
    {
        if (levelAvatar.score > 0)
        {
            levelAvatar.TakeScore(1);
            audioSource.PlayOneShot(clipClick);
            playerStatus.UpMaxHealth(200);

        }
    }
    public void ButtonUpManaSkill()//upmana
    {
        if (levelAvatar.score > 0)
        {
            levelAvatar.TakeScore(1);
            audioSource.PlayOneShot(clipClick);
            playerStatus.UpMaxManaSkill(200);
        }
    }
    public void ButtonUpMana()//up stamina
    {
        if (levelAvatar.score > 0)
        {
            levelAvatar.TakeScore(1);
            audioSource.PlayOneShot(clipClick);
            playerStatus.UpMaxMana(200);
        }
    }

   
    public void UpCriticalHitDamage()//up max dame
    {
        if (levelAvatar.score > 0)
        {
            levelAvatar.TakeScore(1);//tru 1 score
            audioSource.PlayOneShot(clipClick);
            playerStatus.UpCriticalHitDamage(100);


        }
    }
    public void UpCriticalHitChance()//up ty le 
    {
        if (levelAvatar.score > 0)
        {
            levelAvatar.TakeScore(1);//tru 1 score
            audioSource.PlayOneShot(clipClick);
            playerStatus.UpCriticalHitChance(10);
        }
    }

    public void UpBaseDamage()//up dame chinh
    {
        if (levelAvatar.score > 0)
        {
            levelAvatar.TakeScore(1);//tru 1 score
            audioSource.PlayOneShot(clipClick);
            playerStatus.UpBaseDamage(20);
        }
    }

}

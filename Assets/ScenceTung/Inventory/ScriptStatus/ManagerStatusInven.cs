using TMPro;
using UnityEngine;
using UnityEngine.Audio;

public class ManagerStatusInven : MonoBehaviour
{
    public GameObject stats;
    public GameObject inven;
    public GameObject evipment;

    public AudioSource audioSource;
    public AudioClip clipClick;

    //ke thua
   
    PlayerStatus playerStatus;//quan ly mau mana



    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        playerStatus = FindAnyObjectByType<PlayerStatus>();
       
        stats.SetActive(false);
    }


    void Update()
    {

    }
    public void Stats()
    {
        audioSource.PlayOneShot(clipClick);
        stats.SetActive(true);
        inven.SetActive(false);
        evipment.SetActive(false);
    }
    public void ButtonInven()
    {
        audioSource.PlayOneShot(clipClick);
        stats.SetActive(false);
        inven.SetActive(true);
        evipment.SetActive(false);
    }
    public void ButtonEvipment()
    {
        audioSource.PlayOneShot(clipClick);
        stats.SetActive(false);
        inven.SetActive(false);
        evipment.SetActive(true);
    }
    public void ButtonUpHealth()//up mau
    {
        if (playerStatus.score > 0)
        {

            playerStatus.TakeScore(1);
            audioSource.PlayOneShot(clipClick);
            playerStatus.UpMaxHealth(200);

        }
    }
   
    public void ButtonUpMana()//up stamina
    {
        if (playerStatus.score > 0)
        {
            playerStatus.TakeScore(1);
            audioSource.PlayOneShot(clipClick);
            playerStatus.UpMaxMana(200);
        }
    }

   
    public void UpCriticalHitDamage()//up max dame
    {
        if (playerStatus.score > 0)
        {
            playerStatus.TakeScore(1);//tru 1 score
            audioSource.PlayOneShot(clipClick);
            playerStatus.UpCriticalHitDamage(100);


        }
    }
    public void UpCriticalHitChance()//up ty le 
    {
        if (playerStatus.score > 0)
        {
            playerStatus.TakeScore(1);//tru 1 score
            audioSource.PlayOneShot(clipClick);
            playerStatus.UpCriticalHitChance(10);
        }
    }

    public void UpBaseDamage()//up dame chinh
    {
        if (playerStatus.score > 0)
        {
            playerStatus.TakeScore(1);//tru 1 score
            audioSource.PlayOneShot(clipClick);
            playerStatus.UpBaseDamage(20);
        }
    }

}

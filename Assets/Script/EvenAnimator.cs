using UnityEngine;

public class EvenAnimator : MonoBehaviour
{
    //even attackCombo default
    //effect slash
    [SerializeField] private GameObject effectAttack1;
    //[SerializeField] private GameObject effectAttack2;
    [SerializeField] private GameObject effectAttack3;
    [SerializeField] private GameObject effectAttackFly3; 
    private AudioSource audioSource; //audio
    public AudioClip slashSound1;
    public AudioClip slashSound2;
    public AudioClip slashSound3;
    public AudioClip slashSoundFly;


    //Goi ham
    DameZone dameZone;
    ComboAttack comboAttack;
    void Start()
    {

        effectAttack1.SetActive(false);
        effectAttack3.SetActive(false);
        effectAttackFly3.SetActive(false);
        audioSource = GetComponent<AudioSource>();
        dameZone = FindAnyObjectByType<DameZone>();
        comboAttack = FindAnyObjectByType<ComboAttack>();

    }
    //effect even
    public void StartEffectAttack1()
    {
        effectAttack1.SetActive(true);
    }
    public void EndEffectAttack1()
    {
        effectAttack1.SetActive(false);
    }

    /* public void StartEffectAttack2()
     {
         effectAttack2.SetActive(true);
     }
     public void EndEffectAttack2()
     {
         effectAttack2.SetActive(false);
     }*/

    public void StartEffectAttack3()
    {
        effectAttack3.SetActive(true);
    }
    public void EndEffectAttack3()
    {
        effectAttack3.SetActive(false);
    }

    public void StartEffectAttackFly3()
    {
        effectAttackFly3.SetActive(true);
    }
    public void EndEffectAttackFly3()
    {
        effectAttackFly3.SetActive(false);
    }

    //sounds even
    public void PlaySlashSound1()
    {
        audioSource.PlayOneShot(slashSound1);
    }
    public void PlaySlashSound2()
    {
        audioSource.PlayOneShot(slashSound2);
    }
    public void PlaySlashSound3()
    {
        audioSource.PlayOneShot(slashSound3);
    }
    public void PlaySlashSoundFly()
    {
        audioSource.PlayOneShot(slashSoundFly);
    }

    //damezone box
    public void StartDameZone()
    {
        dameZone.beginDame();
    }
    public void EndDameZone()
    {
        dameZone.endDame();
    }
    public void StartDameZoneHit()
    {

    }
    public void EndDameZoneHit()
    {

    }

   
}

using UnityEngine;

public class EvenAnimator : MonoBehaviour
{
    //even attackCombo default
    //effect slash
    [Header("EvenAttack--------------------------------")]
    [SerializeField] private GameObject effectAttack1;
    //[SerializeField] private GameObject effectAttack2;
    [SerializeField] private GameObject effectAttack3;
    [SerializeField] private GameObject effectAttackFly3; 
    private AudioSource audioSource; //audio
    public AudioClip slashSound1;
    public AudioClip slashSound2;
    public AudioClip slashSound3;
    public AudioClip slashSoundFly;

    [Header("EvenSkill4--------------------------------")]
    [SerializeField] private GameObject effectChangeSkill4;
    [SerializeField] private GameObject effectSkill4Attack1;
    [SerializeField] private GameObject effectSkill4Attack2;
    [SerializeField] private GameObject effectSkill4Attack3;
    [SerializeField] private GameObject effectSkill4Fly;
    [SerializeField] private Transform positionSKill4;
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

    //skill4--------------------------------------------------------
    public void StartEffectSkill4Level1(float force = 10f, float duration = 2f)//effect skill4 level 1
    {
        GameObject effect = Instantiate(effectSkill4Attack1, positionSKill4.position, transform.rotation);
        Rigidbody rb = effect.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * force, ForceMode.Impulse);
        Destroy(effect, duration);
    }

    public void StartEffectSkill4Level2(float force = 15f, float duration = 2.5f)//effect skill4 level 2
    {
        GameObject effect = Instantiate(effectSkill4Attack2, positionSKill4.position, transform.rotation);
        Rigidbody rb = effect.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * force, ForceMode.Impulse);
        Destroy(effect, duration);
    }

    public void StartEffectSkill4Level3(float force = 20f, float duration = 3f)//effect skill4 level 3
    {
        GameObject effect = Instantiate(effectSkill4Attack3, positionSKill4.position, transform.rotation);
        Rigidbody rb = effect.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * force, ForceMode.Impulse);
        Destroy(effect, duration);
    }

    public void StartEffectSkill4Ultimate(float force = 25f, float duration = 3.5f)//effect skill4 ultimate
    {
        GameObject effect = Instantiate(effectSkill4Fly, positionSKill4.position, transform.rotation);
        Rigidbody rb = effect.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * force, ForceMode.Impulse);
        Destroy(effect, duration);
    }

    public void StartEffectSkill4Transform(float duration = 2f)//effect skill4 transform
    {
        GameObject effect = Instantiate(effectChangeSkill4, positionSKill4.position, Quaternion.identity);
        Destroy(effect, duration);
    }
}
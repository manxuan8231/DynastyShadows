using UnityEngine;

public class EvenAnimator : MonoBehaviour
{
    //even attackCombo default
    //effect slash
    [Header("EvenAttack--------------------------------")]
    [SerializeField] private GameObject effectAttack1;
    public AudioClip slashSound1;
    //[SerializeField] private GameObject effectAttack2;
    public AudioClip slashSound2;
    [SerializeField] private GameObject effectAttack3;
    public AudioClip slashSound3;
    [SerializeField] private GameObject effectAttackFly3;
    public AudioClip slashSoundFly;
 
    [Header("EvenSkill4Core--------------------------------")]
    [SerializeField] private GameObject effectChangeSkill4;
    [SerializeField] private Transform positionSKill4;

    [Header("even audio move---------------------------- ")]
    public AudioClip audioJump;
    public AudioClip audioRoll;
    public AudioClip audioMovemen;

    [Header("EvenSkill1EF----------------------------------")]
    // Cầu lửa
    public GameObject fireBall;
    public Transform firePoint;
    //mua lua
    public GameObject rainFireBall;
    public Transform rainFireBallPs;

    //tham chieu 
    DameZone dameZone;
    ComboAttack comboAttack;
    private AudioSource audioSource; //audio
    void Start()
    {

        effectAttack1.SetActive(false);
        effectAttack3.SetActive(false);
        effectAttackFly3.SetActive(false);
        audioSource = GetComponent<AudioSource>();
        dameZone = FindAnyObjectByType<DameZone>();
        comboAttack = FindAnyObjectByType<ComboAttack>();

    }
    //movemen
    //sound even 
    public void SoundMovemen()
    {
        audioSource.PlayOneShot(audioMovemen);
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

    //skill4Core--------------------------------------------------------
    public void StartEffectChangeSkill4()
    {
        GameObject effect = Instantiate(effectChangeSkill4, positionSKill4.position, transform.rotation);
        Destroy(effect, 1.3f);
    }

    //skill1EF---------------------------------------------------------
    public void StartFireBall()
    {
       
        Debug.Log("Dùng kỹ năng cầu lửa!");
       
        Vector3 custom = new Vector3(90, 0, 0); 
        Quaternion fireballRotation = Quaternion.LookRotation(custom);
        GameObject instan = Instantiate(fireBall, firePoint.position, fireballRotation);

        Rigidbody rb = instan.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Debug.Log("Bắn thành công");
            rb.AddForce(firePoint.forward * 100, ForceMode.Impulse);
        }
        Destroy(instan, 5f); // Hủy sau 5 giây
    }
    public void StartRainFireBall()
    {
        Debug.Log("Dùng kỹ năng mưa lửa!");
        GameObject instan = Instantiate(rainFireBall, rainFireBallPs.position, rainFireBallPs.rotation);
       
        Destroy(instan, 5f); // Hủy sau 5 giây
    }
}
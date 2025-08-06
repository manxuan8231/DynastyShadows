using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class EvenAlien : MonoBehaviour
{

    //shoot ban dan
    public GameObject bulletPrefab;
    public Transform spawnBulletPosi;
    public float speed = 100f;
    public GameObject effectShort;
    //telepathic la skill hut
    public GameObject telePathic;
    //nem bong
    public GameObject ballPrefab;
    

    //sound clip
    public AudioClip attackLClip;
    public AudioClip attackRClip;
    public AudioClip teleClip;
    public AudioClip dieClip;
    public AudioClip runClip;
    //tham chieu
    private CameraShake cameraShake;
    private DemonAlien demonAlien;
    private DameZoneAttackAlien dameZoneAttackAlien;
    private DemonAlienHp demonAlienHp;
    private DameZoneBall dameZoneBall;
    public AudioSource audioSource;
    void Start()
    {
        cameraShake = FindAnyObjectByType<CameraShake>();
        demonAlien = FindAnyObjectByType<DemonAlien >();
        dameZoneAttackAlien = FindAnyObjectByType   <DameZoneAttackAlien >();
        demonAlienHp = FindAnyObjectByType<DemonAlienHp>();
        dameZoneBall = FindAnyObjectByType<DameZoneBall >();
        audioSource = GetComponent<AudioSource>();
        effectShort.SetActive(false);
        telePathic.SetActive(false);
        
    }

    
    void Update()
    {
        
    }
    //dameattak left
    public void BeginDameLeft()
    {
        audioSource.PlayOneShot(attackLClip);
        dameZoneAttackAlien.BeginDame();


    }
    public void EndDameLeft()
    {
        dameZoneAttackAlien.EndDame();
    }

    //dameattak right ball
    public void BeginDameRight()
    {
        audioSource.PlayOneShot(attackRClip);
        dameZoneBall.BeginDame();

    }
    public void EndDameRight()
    {
        dameZoneBall.EndDame();
    }

    //rung camera
    public void ShakeCamera()
    {
        cameraShake.Shake(0.3f);
    }
    //ban dan
    public void ShootBullet()
    {
        if (demonAlienHp.currentMana < 30) return;//mana be hon 30 thi ko cho
        Vector3 targetPos = demonAlien.player.position;
        targetPos.y = demonAlien.transform.position.y;
        demonAlien.transform.LookAt(targetPos);

        Vector3 spawn = spawnBulletPosi.position;
        // Lấy viên đạn từ pool
        GameObject bullet = Instantiate(bulletPrefab, spawn, Quaternion.identity);

        Vector3 rota = transform.forward;
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.linearVelocity = rota * speed;
        Destroy(bullet, 10f);
        effectShort.SetActive(true);
        demonAlienHp.currentMana -= 30;
    }
    public void EndEffectShort()
    {
        effectShort.SetActive(false);
        
    }

    //hut toi
    public void StartTelePathic()
    {
        demonAlien.transform.LookAt(demonAlien.player.position);
        telePathic.SetActive(true );

    }
    public void EndTelePathic()
    {
        telePathic.SetActive(false);

    }

    //nem bong effect
    public void StartEffectBall()
    {
        Vector3 targetPos = demonAlien.player.position;
        targetPos.y = demonAlien.transform.position.y;
        demonAlien.transform.LookAt(targetPos);
        GameObject ins = Instantiate(ballPrefab,transform.position, transform.rotation);
        Destroy(ins,4f);

    }

    //die
    public void PlaySoundDie()
    {
        audioSource.PlayOneShot(dieClip);
    }
   
    //run
    public void PlaySoundRun()
    {
        audioSource.PlayOneShot(runClip);
    }
}

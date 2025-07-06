using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class EvenAlien : MonoBehaviour
{

    //shoot
    public string tagBullet;
    public Transform spawnBulletPosi;
    public float speed = 100f;
    public GameObject effectShort;
    //telepathic
    public GameObject telePathic;
    //sound
    public AudioClip attackLClip;
    public AudioClip attackRClip;
    public AudioClip teleClip;
    //tham chieu
    private CameraShake cameraShake;
    private DemonAlien demonAlien;
    private DameZoneAttackAlien dameZoneAttackAlien;
    private DameZoneBall dameZoneBall;
    public AudioSource audioSource;
    void Start()
    {
        cameraShake = FindAnyObjectByType<CameraShake>();
        demonAlien = FindAnyObjectByType<DemonAlien >();
        dameZoneAttackAlien = FindAnyObjectByType   <DameZoneAttackAlien >();
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

    //rung cam
    public void ShakeCamera()
    {
        cameraShake.Shake();
    }
    //ban dan
    public void ShootBullet()
    {
        
        demonAlien.transform.LookAt(demonAlien.player.position);
        Vector3 spawn = spawnBulletPosi.position;

        // Lấy viên đạn từ pool
        GameObject bullet = ObjPoolingManager.Instance.GetEnemyFromPool(tagBullet, spawn);
        
        Vector3 rota = transform.forward;
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.linearVelocity = rota * speed;
        effectShort.SetActive(true);
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

    
}

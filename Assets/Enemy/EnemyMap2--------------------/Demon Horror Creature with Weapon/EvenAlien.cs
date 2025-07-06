using UnityEngine;

public class EvenAlien : MonoBehaviour
{

    //shoot
    public string tagBullet;
    public Transform spawnBulletPosi;
    public float speed = 100f;
    public GameObject effectShort;
    //telepathic
    public GameObject telePathic;

    //tham chieu
    public CameraShake cameraShake;
    public DemonAlien demonAlien;
    void Start()
    {
        cameraShake = FindAnyObjectByType<CameraShake>();
        demonAlien = FindAnyObjectByType<DemonAlien >();
        effectShort.SetActive(false);
        telePathic.SetActive(false);
    }

    
    void Update()
    {
        
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
        telePathic.SetActive(true );

    }
    public void EndTelePathic()
    {
        telePathic.SetActive(false);

    }
}

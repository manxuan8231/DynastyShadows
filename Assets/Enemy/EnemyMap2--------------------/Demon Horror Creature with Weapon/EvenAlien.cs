using UnityEngine;

public class EvenAlien : MonoBehaviour
{

    //shoot
    public string tagBullet;
    public Transform spawnBulletPosi;
    public float speed = 100f;
    //tham chieu
    public CameraShake cameraShake;
    public DemonAlien demonAlien;
    void Start()
    {
        cameraShake = FindAnyObjectByType<CameraShake>();
        demonAlien = FindAnyObjectByType<DemonAlien >();
    }

    
    void Update()
    {
        
    }
    public void ShakeCamera()
    {
        cameraShake.Shake();
    }
    public void ShootBullet()
    {
        demonAlien.transform.LookAt(demonAlien.player.position);
        Vector3 spawn = spawnBulletPosi.position;

        // Lấy viên đạn từ pool
        GameObject bullet = ObjPoolingManager.Instance.GetEnemyFromPool(tagBullet, spawn);
        
        Vector3 rota = transform.forward;
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.linearVelocity = rota * speed;
    }



}


using UnityEngine;

public class EvenAnimatorAssa : MonoBehaviour
{
    //ao anh
    public GameObject shadowPrefab;

    //ao anh sau khi dash
    [Header("Tao anh khi dash")]
    public GameObject dashAfterImg;//prefab hình ảnh sau khi dash
    public float afterImgDuration = 0.5f; // Thời gian tồn tại 
   
    //phong dao
    public GameObject knifePrefab;
    public Transform knifeSpawnPoint;
    [Header("Effect")]
    public GameObject lastExplosionPrefab; // Prefab hiệu ứng nổ 

    [Header("AudioClip")]
    public AudioClip knifeThrowSound; // Âm thanh khi ném dao
    public AudioClip dashSound; // Âm thanh khi luot qua
    public AudioClip attackSound; // Âm thanh taans cong binh thuong
    public AudioClip slashSound;// âm thanh khi chém
    [Header("Tham Chieu")]
    public AudioSource audioSource; // Thêm biến AudioSource để phát âm thanh
    public DameZoneLeftAssa dameZoneLeftAssa;
    public DameZoneRightAssa dameZoneRightAssa;
    public ControllerStateAssa controllerStateAssa;
    void Start()
    {
        dameZoneLeftAssa = FindAnyObjectByType<DameZoneLeftAssa>();
        dameZoneRightAssa = FindAnyObjectByType <DameZoneRightAssa>();
        controllerStateAssa = FindAnyObjectByType<ControllerStateAssa>();
        audioSource = FindAnyObjectByType<AudioSource>();

    }

    
  
    //tay trai
    public void BeginDameLeft()
    {
        PlayAttackSound();
        dameZoneLeftAssa.BeginDame();
    }
    public void EndDameLeft() 
    {
        dameZoneLeftAssa.EndDame();
    }
    //tay phai
    public void BeginDameRight()
    {
        PlayAttackSound();
        dameZoneRightAssa.BeginDame();
    }
    public void EndDameRight()
    {
        dameZoneRightAssa.EndDame();
    }
    public void BeginDameBack()//hit back
    {
        PlayAttackSound();
        dameZoneRightAssa.BeginDameBack();
    }
    public void EndDameBack()
    {
        dameZoneRightAssa.EndDameBack();
    }

    //tao bong
    public void StartShadow()
    {
        GameObject instan = Instantiate(shadowPrefab,transform.position,transform.rotation);
        
    }
    //phong dao 
    public void KnifeThrower()
    {
        // Tạo dao tại vị trí spawn
        GameObject instan = Instantiate(knifePrefab, knifeSpawnPoint.position, Quaternion.identity);

        Rigidbody rb = instan.GetComponent<Rigidbody>();
        if (rb != null && controllerStateAssa.player != null)
        {
            // Hướng từ enemy tới player
            Vector3 direction = (controllerStateAssa.player.transform.position - knifeSpawnPoint.position).normalized;
            
            Quaternion lookRot = Quaternion.LookRotation(direction); // hướng về player
            lookRot *= Quaternion.Euler(-90f, 0f, 0f); 
            instan.transform.rotation = lookRot;
            // Bắn dao theo hướng đó
            rb.AddForce(direction * 50f, ForceMode.Impulse);

           Destroy(instan, 4f); // Hủy dao sau 2 giây
        }
    }

    //tao hinh anh khi dash
    public void CreateAsterImg()
    {
    
            GameObject afterImg = Instantiate(dashAfterImg, transform.position, transform.rotation);
            Destroy(afterImg, afterImgDuration);
          
        
    }

    //am thah----------------
    public void PlayKnifeThrowSound()//am thanh khi phong dao
    {
        if (audioSource != null && knifeThrowSound != null)
        {
            audioSource.PlayOneShot(knifeThrowSound);
        }
    }
    public void PlayDashSound()//am thanh khi dash
    {
        if (audioSource != null && dashSound != null)
        {
            audioSource.PlayOneShot(dashSound);
        }
    }
    public void PlaySlashSound()
    {
        if (audioSource != null && slashSound != null)
        {
            audioSource.PlayOneShot(slashSound);
        }
    }
    public void PlayAttackSound() //audio slash
    {
        if (audioSource != null && attackSound != null)
        {
            audioSource.PlayOneShot(attackSound);
        }
    }

    //effect
    public void PlayLastAttackExplosion()
    {
        // Tạo hiệu ứng nổ tại vị trí của enemy
        GameObject explosionEffect = Instantiate(lastExplosionPrefab, transform.position, transform.rotation);
        Destroy(explosionEffect, 1.5f); // Hủy hiệu ứng sau 2 giây
    }
}

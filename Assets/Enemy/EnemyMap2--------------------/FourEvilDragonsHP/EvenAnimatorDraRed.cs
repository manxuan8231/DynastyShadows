using UnityEngine;

public class EvenAnimatorDraRed : MonoBehaviour
{
    // Animator cho các hiệu ứng tấn công của rồng đỏ
    public GameObject effectAttack1;
    public GameObject effectFlame;
    public GameObject effectAttack3;
    //attack fly
    public GameObject fireBallPrefabShoter;
    public Transform posiFireShooterLeft;
    public Transform posiFireShooterRight;
    //box dame
    public GameObject boxDame3;
    //sound
    public AudioClip stunSound; // Âm thanh choáng
    public AudioClip flameSound;
    public AudioClip flySound;
    public AudioClip attack3Sound; // Âm thanh tấn công 3
    //tham chieu
    DrakonitDameZone drakonitDameZone;
    DragonRed dragonRed;
    DragonRedHP dragonRedHp;
    void Start()
    {
        drakonitDameZone = FindAnyObjectByType<DrakonitDameZone>();
        dragonRed = FindAnyObjectByType<DragonRed>();
        dragonRedHp = FindAnyObjectByType<DragonRedHP>();

        effectFlame.SetActive(false);
        boxDame3.SetActive(false);
    }

    
    void Update()
    {
       if(dragonRedHp.currentArmor <= 0)//nếu dg choáng thì tắt lửa
        {
            EndEffectFlame();
        }
    }
    // Hiệu ứng lua
    public void PlayEffectFlame()
    {
        dragonRed.audioSource.PlayOneShot(flameSound); // Phát âm thanh lửa
        effectFlame.SetActive(true);
    }
    public void EndEffectFlame()
    {
        dragonRed.audioSource.clip = flameSound;
        dragonRed.audioSource.Stop(); // Dừng âm thanh lửa
        effectFlame.SetActive(false);
    }
    //attack
    public void BeginDame()
    {
        drakonitDameZone.beginDame();
    }
    public void EndDame()
    {
        drakonitDameZone.endDame();
    }

    public void BeginDame3()
    {
        boxDame3.SetActive(true);
       
    }
    public void EndDame3()
    {
        
        boxDame3.SetActive(false);
    }
    //rainFireBall
    public void FireBallRain()
    {
        GameObject fireBallL = Instantiate(fireBallPrefabShoter, posiFireShooterLeft.position, transform.rotation);
        GameObject fireBallR = Instantiate(fireBallPrefabShoter, posiFireShooterRight.position, transform.rotation);
        Destroy(fireBallL, 5f);
        Destroy(fireBallR, 5f);
    }

    //am thanh
    public void PlayAttack3()
    {
        dragonRed.audioSource.PlayOneShot(attack3Sound); // Phát âm thanh cắn
    }
    public void PlayStunSound()
    {
        dragonRed.audioSource.PlayOneShot(stunSound); // Phát âm thanh choáng
    }
    public void PlaySoundFly()
    {
        dragonRed.audioSource.PlayOneShot(flySound);
    }
}

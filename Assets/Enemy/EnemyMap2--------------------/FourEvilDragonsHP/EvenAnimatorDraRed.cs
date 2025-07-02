using UnityEngine;

public class EvenAnimatorDraRed : MonoBehaviour
{
    // Animator cho các hiệu ứng tấn công của rồng đỏ
    public GameObject effectAttack1;
    public GameObject effectFlame;
    public GameObject effectAttack3;
    //attack fly
    public GameObject fireBallPrefab;
    public Transform effectPosition;
    //box dame
    public GameObject boxDame3;
   
   

    //tham chieu
    DrakonitDameZone drakonitDameZone;
    
    void Start()
    {
        drakonitDameZone = FindAnyObjectByType<DrakonitDameZone>();
       
        effectFlame.SetActive(false);
        boxDame3.SetActive(false);
    }

    
    void Update()
    {
       
    }
    // Hiệu ứng lua
    public void PlayEffectFlame()
    {
        effectFlame.SetActive(true);
    }
    public void EndEffectFlame()
    {
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
        GameObject fireBall = Instantiate(fireBallPrefab, effectPosition.position, Quaternion.identity);
        Destroy(fireBall, 5f); // Hủy sau 5 giây
    }
}

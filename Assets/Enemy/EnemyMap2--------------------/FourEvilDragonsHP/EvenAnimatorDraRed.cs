using UnityEngine;

public class EvenAnimatorDraRed : MonoBehaviour
{
    // Animator cho các hiệu ứng tấn công của rồng đỏ
    public GameObject effectAttack1;
    public GameObject effectFlame;
    public GameObject effectAttack3;
    //box dame
    public GameObject boxDame3;
    //vi tri
    public Transform effectPosition;

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
    public void PlayEffectFlame()
    {
        effectFlame.SetActive(true);
    }
    public void EndEffectFlame()
    {
        effectFlame.SetActive(false);
    }

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
}

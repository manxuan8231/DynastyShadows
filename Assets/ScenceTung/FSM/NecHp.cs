using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NecHp : MonoBehaviour
{
    public int curhp;
    public int maxhp;
    public Slider sliderHp;
    public TextMeshProUGUI textHp;
    public GameObject sliderHpBoss2;
    private NecController controller;
    private NecAudioManager audioManager;
    public BoxCollider triggerBox;
    public bool isAudioBgr;
    void Start()
    {
        audioManager = FindAnyObjectByType<NecAudioManager>();
        sliderHpBoss2.SetActive(false);

       UpdateUI();
        controller = FindAnyObjectByType<NecController>();
        isAudioBgr = false;
    }
   
    public void UpdateUI()
    {
        curhp = maxhp;
        sliderHp.maxValue = curhp; // Đặt giá trị tối đa cho thanh máu
        sliderHp.value = curhp; // Đặt giá trị hiện tại cho thanh máu
        textHp.text = $"{curhp}/{maxhp}"; // Cập nhật text hiển thị máu
    }
   
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player"  )
        {
            if (!isAudioBgr) audioManager.audioSource.PlayOneShot(audioManager.audopBackgroud); isAudioBgr = true;

            sliderHpBoss2.SetActive(true);
            triggerBox.enabled = false;
            
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            triggerBox.enabled = true;
        }
    }
    public void TakeDamage()
    {
       
        if (curhp <= 0)
        {
           
                controller.anmt.enabled = true; // Bật animator để có thể chơi animation chết
                controller.enabled = true; // Bật lại Enemy2 để có thể chơi animation chết
                controller.agent.enabled = true; // Bật lại NavMeshAgent để có thể chơi animation chết
            
        }
       
    }
}

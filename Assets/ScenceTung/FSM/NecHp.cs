using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NecHp : MonoBehaviour
{
    public float curhp;
    public float maxhp;
    public Slider sliderHp;
    public TextMeshProUGUI textHp;
    public GameObject sliderHpBoss2;
    private NecController controller;
    private NecAudioManager audioManager;

    void Start()
    {
        audioManager = FindAnyObjectByType<NecAudioManager>();
        sliderHpBoss2.SetActive(false);

        curhp = maxhp;
        sliderHp.maxValue = curhp; // Đặt giá trị tối đa cho thanh máu
        sliderHp.value = curhp; // Đặt giá trị hiện tại cho thanh máu
        textHp.text = $"{curhp}/{maxhp}"; // Cập nhật text hiển thị máu
        controller = FindAnyObjectByType<NecController>();
    }


   
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            audioManager.audioSource.PlayOneShot(audioManager.audopBackgroud);
            sliderHpBoss2.SetActive(true);
            
        }
    }
    
}

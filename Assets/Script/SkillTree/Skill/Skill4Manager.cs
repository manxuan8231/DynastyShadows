using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Skill4Manager : MonoBehaviour
{
    public SkinnedMeshRenderer[] skill4MeshRenderers; // Mảng chứa các Skin của skill 4
  
    public bool isInputSkill4 = false;//kiem tra de chuyen trang thai skill4
    public bool isChangeStateSkill4 = false;
    public float coolDownTime = 50f; // Thời gian hồi chiêu của skill 4
    public float lastCoolDown = -50f; // Biến để theo dõi thời gian hồi chiêu hiện tại
    public float timeSkill4 = 10f;//thời gian skill 4 ton tai
    public Slider sliderCoolDown;
    public TextMeshProUGUI textCoolDownSkill;
    //unlock skill trong UI thi moi cho dung skill
    public bool isUnlockSkill4 = false;//lv1
    public bool isReflectDamage = false;//unlock để mở trạng thái phản dame lv4
    public bool isUpSpeed = false;//tang tốc khi mở khóa level 5
    public bool isStun = false; // trạng thái stun khi mở khóa level 6
    public bool isImmotal; // trạng thái bất tử khi mở khóa level 7

    public GameObject iconSkill4;
   
    //tham chieu
    public PlayerStatus playerStatus;
   
    void Start()
    {
        playerStatus = FindAnyObjectByType<PlayerStatus>();
     
        textCoolDownSkill.enabled = false;
        isInputSkill4 = true; // Khởi tạo trạng thái skill 4 là không được kích hoạt
        isChangeStateSkill4 = false;
        iconSkill4.SetActive(false);
        sliderCoolDown.maxValue = coolDownTime; // Đặt giá trị tối đa của slider là thời gian hồi chiêu
        sliderCoolDown.value = coolDownTime; // Đặt giá trị ban đầu của slider là thời gian hồi chiêu
     
        sliderCoolDown.enabled = false; // Tắt slider ban đầu
        ToggleSkill4(false); // Tắt skill 4 ban đầu
    }

   
    void Update()
    {

        //slider giảm dần
        float time = Time.time - lastCoolDown;
        float remainingCooldown = Mathf.Clamp(0, coolDownTime - time, coolDownTime);
        sliderCoolDown.value = remainingCooldown;
        textCoolDownSkill.text = Mathf.FloorToInt(remainingCooldown).ToString();
        if (remainingCooldown <= 0) { 
        textCoolDownSkill.enabled = false;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4) && Time.time >= lastCoolDown + coolDownTime && isUnlockSkill4 == true && isInputSkill4 == true) 
        {
            sliderCoolDown.enabled = true; // Bật slider khi nhấn phím 4
            textCoolDownSkill.enabled = true;
            isChangeStateSkill4 = true;
            StartCoroutine(WaitChangeSkin()); // Bắt đầu coroutine để thay đổi skin
            lastCoolDown = Time.time; // Cập nhật thời gian hồi chiêu
        }
    }
    public void ToggleSkill4(bool isActive)
    {
        foreach (var renderer in skill4MeshRenderers)
        {
            renderer.enabled = isActive; // Bật hoặc tắt tất cả các SkinnedMeshRenderer
        }
    }

    public IEnumerator WaitChangeSkin()
    {
        yield return new WaitForSeconds(1.5f); // Chờ 10 giây trước khi tắt skill 4
        ToggleSkill4(true); // Bật skin khi nhấn phím 4   
                            //hoi day hp
        playerStatus.currentHp = playerStatus.maxHp;
        playerStatus.sliderHp.value = playerStatus.maxHp;
        //hoi day mana
        playerStatus.currentMana = playerStatus.maxMana;
        playerStatus.sliderMana.value = playerStatus.maxMana;
    }
}

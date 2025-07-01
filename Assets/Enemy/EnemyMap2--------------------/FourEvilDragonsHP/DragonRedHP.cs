using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class DragonRedHP : MonoBehaviour
{
    //hp
    public Slider sliderHp;//máu
    public TextMeshProUGUI textHp;
    public float currentHp;
    public float maxHp = 10000f;
    //giáp ảo
    public Slider sliderArmor; //giáp
    public float currentArmor;
    public float maxArmor = 5000;
    private bool isArmorRegenerating = false; // Biến để kiểm tra xem giáp ảo 
    public bool isStunned = false;//chỉ cho chạy animator 1 lần

    //tich diem de phan khang
    public int strugglePoint = 0;
    //tham chieu
    private DragonRed dragonRed; // Tham chiếu đến script DragonRed
    void Start()
    {
        dragonRed = FindAnyObjectByType<DragonRed>(); // Tìm đối tượng DragonRed trong cảnh
        // Cập nhật thanh máu
        currentHp = maxHp;
        sliderHp.maxValue = maxHp;
        sliderHp.value = currentHp;
        textHp.text = $"{currentHp}/{maxHp}";
        // Cập nhật thanh giáp
        currentArmor = maxArmor;
        sliderArmor.maxValue = maxArmor;
        sliderArmor.value = currentArmor;
        isStunned = false;
    }

    
    void Update()
    {
        Stun(); //stun sau khi bi  het giap
    }
    //ham lay hp----------------------------
    public void TakeDame(float amount)
    {
        if (currentArmor > 0)
        {
           
            currentArmor -= amount; // Giảm giáp ảo
            UpdateUI();
            strugglePoint++;
        }
        if (currentArmor <= 0)
        {
            currentHp -= amount; // Nếu giáp ảo hết, giảm máu
            UpdateUI();
            if (currentHp <= 0)
            {
               Destroy(gameObject); 

            }
        }
    }
    //cap nhap UI slider 
    public void UpdateUI()
    {
        // Cập nhật thanh máu      
        currentHp = Mathf.Clamp(currentHp, 0, maxHp); // Đảm bảo máu không vượt quá giới hạn
        sliderHp.maxValue = maxHp;
        sliderHp.value = currentHp;
        textHp.text = $"{currentHp}/{maxHp}";
        // Cập nhật thanh giáp       
        currentArmor = Mathf.Clamp(currentArmor, 0, maxArmor); // Đảm bảo giáp không vượt quá giới hạn
        sliderArmor.maxValue = maxArmor;
        sliderArmor.value = currentArmor;

    }
    //stun sau khi bi  het giap
    public void Stun()
    {
        if (currentArmor <= 0)//neu giáp ảo <= 0 thì choáng
        {
            if (!isStunned) // Chỉ cho phép chạy hành động này một lần
            {
                isStunned = true; // Đánh dấu là đã bị choáng
                dragonRed. animator.SetTrigger("Stun");
                dragonRed. animator.SetBool("IsWalking", false); // Tắt hành động di chuyển trong Animator
                dragonRed. navMeshAgent.isStopped = true;

            }
        }
        if (currentArmor > 0)//nếu giáp ảo > 0 thì không choáng nữa
        {
            if (isStunned) // Chỉ cho phép chạy hành động này một lần
            {
                Debug.Log("het stun");
                isStunned = false; // Nếu giáp còn, không choáng nữa
                dragonRed.animator.SetTrigger("Idle");
                dragonRed.navMeshAgent.isStopped = false;
            }

        }
        // Kiểm tra nếu giáp ảo đã hết và chưa bắt đầu hồi phục
        if (currentArmor <= 0 && !isArmorRegenerating)
        {
            isArmorRegenerating = true;
            StartCoroutine(WaitRegenerateArmor()); // Bắt đầu Coroutine để hồi phục giáp ảo
        }
    }
    //khi het giap thi bat dau doi 10f r hoi phuc
    public IEnumerator WaitRegenerateArmor()
    {
        yield return new WaitForSeconds(10f); // Thời gian chờ 5 giây
        currentArmor = maxArmor; // Đặt lại giáp ảo về giá trị tối đa
        isArmorRegenerating = false; // Cho phép lần hồi tiếp theo
        UpdateUI();
    }
   
}

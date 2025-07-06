using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DragonRedHP : MonoBehaviour,IDamageable
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
    //mana fly
    public Slider sliderMana; //mana
    public float currentMana;
    public float maxMana = 1000;

    //tich diem de phan khang
    public int strugglePoint = 0;
    //tham chieu
    private DragonRed dragonRed; // Tham chiếu đến script DragonRed
    private DragonRedFly dragonRedFly; // Tham chiếu đến script DragonRedFly
    
    public TimeLineBossDragonDead timeLine;
    void Start()
    {
        dragonRed = FindAnyObjectByType<DragonRed>(); // Tìm đối tượng DragonRed trong cảnh
        dragonRedFly=FindAnyObjectByType<DragonRedFly>(); // Tìm đối tượng DragonRedFly trong cảnh
      
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
        //mana
       
        sliderMana.maxValue = maxMana;
        sliderMana.value = currentMana;

        timeLine = FindAnyObjectByType<TimeLineBossDragonDead>(); // Tìm đối tượng TimeLineBossDragonDead trong cảnh
    }


    void Update()
    {
        Stun(); //stun sau khi bi  het giap
        ManaFly();
        

    }
    //ham lay hp----------------------------
    public void TakeDamage(float amount)
    {
        if (currentArmor > 0)
        {
            currentArmor -= amount;
            currentArmor = Mathf.Clamp(currentArmor, 0, maxArmor); // Đảm bảo giáp không vượt quá giới hạn

        }
        else
        {
            currentHp -= amount;
            currentHp = Mathf.Clamp(currentHp, 0, maxHp); // Đảm bảo máu không vượt quá giới hạn
        }

        UpdateUI();

        if (currentHp <= 0)
        {
            timeLine.Run();
        }
    }


    //stun sau khi bi  het giap
    public void Stun()
    {
        if (currentArmor <= 0)
        {
            if (!isStunned)
            {
                isStunned = true;

                // Reset rồi mới Set để đảm bảo trigger hoạt động lại
                dragonRed.animator.ResetTrigger("Stun");
                dragonRed.animator.SetTrigger("Stun");

                dragonRed.animator.SetBool("IsWalking", false);
                dragonRed.aiPath.isStopped = true;
            }
        }
        else
        {
            if (isStunned)
            {
                isStunned = false;

                Debug.Log("het stun");

                dragonRed.animator.ResetTrigger("Idle");
                dragonRed.animator.SetTrigger("Idle");

                dragonRed.aiPath.isStopped = false;
            }
        }

        if (currentArmor <= 0 && !isArmorRegenerating)
        {
            isArmorRegenerating = true;
            StartCoroutine(WaitRegenerateArmor());
        }
    }

    //khi het giap thi bat dau doi 10f r hoi phuc
    public void ManaFly()
    {
        if(isStunned) return; // Nếu đang choáng thì không thực hiện hồi phục mana
        if (dragonRedFly.isFly)//dg bay thi tru mana
        {
            currentMana -= Time.deltaTime * 10f;
            UpdateUI();
        }
        else
        {
            currentMana += Time.deltaTime * 50f;
           
            UpdateUI();
        }
    }
    //cap nhap UI slider 
    public void UpdateUI()
    {
        

        // Cập nhật thanh máu      
       
        sliderHp.maxValue = maxHp;
        sliderHp.value = currentHp;
        textHp.text = $"{currentHp}/{maxHp}";
        
        // Cập nhật thanh giáp       
       
        sliderArmor.maxValue = maxArmor;
        sliderArmor.value = currentArmor;
        //mana
       
        sliderMana.value = currentMana; // thanh mana
    }
    public IEnumerator WaitRegenerateArmor()
    {
        yield return new WaitForSeconds(10f); // Thời gian chờ 5 giây
        currentArmor = maxArmor; // Đặt lại giáp ảo về giá trị tối đa
        isArmorRegenerating = false; // Cho phép lần hồi tiếp theo
        UpdateUI();
    }
   
}

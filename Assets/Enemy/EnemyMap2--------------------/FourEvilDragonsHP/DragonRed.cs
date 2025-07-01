using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class DragonRed : MonoBehaviour
{
    public GameObject player;// người chơi
   
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
    private bool isStunned = false;//chỉ cho chạy animator 1 lần

    //tham chieu
    public NavMeshAgent navMeshAgent;
    public Animator animator;

    //cây hành vi
    private Node _rootNode;// Cây hành vi gốc
  

    void Start()
    {
        SetupBehaviorTree();// thiết lập cây hành vi
        navMeshAgent = GetComponent<NavMeshAgent>(); // Lấy NavMeshAgent từ đối tượng này
        animator = GetComponent<Animator>(); // Lấy Animator từ đối tượng này
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
        // Kiểm tra nếu giáp ảo đã hết và chưa bắt đầu hồi phục
        if (currentArmor <= 0 && !isArmorRegenerating)
        {
            isArmorRegenerating = true;
            StartCoroutine(WaitRegenerateArmor()); // Bắt đầu Coroutine để hồi phục giáp ảo
        }
        _rootNode?.Evaluate();
        
    }

    private void SetupBehaviorTree()
    {
        var isNoArmor = new ConditionNode(()=> currentArmor <= 0);//kiểm tra giáp ảo hết chưa mới cho phép giảm máu
        var actionStun = new ActionNode(() =>
        {
            if (!isStunned)
            {
                animator.SetBool("Stun", true);
                isStunned = true;
            }
            return NodeState.SUCCESS;
        }); // chạy animator stun
        var isArmorRestored = new ConditionNode(() => currentArmor > 0); // kiểm tra giáp ảo hoi chua
        var actionUnsStun = new ActionNode(() =>
        {
            if (isStunned)
            {
                animator.SetBool("Stun", false); // Dừng hành động stun trong Animator
                isStunned = false; // Đặt lại trạng thái stun
            }
           
            return NodeState.SUCCESS; // Trả về thành công khi dừng hành động stun
        });

        // Cây hành vi gốc
        _rootNode = new SelectorNode(new List<Node>
        {
            new SequenceNode(new List<Node> { isNoArmor, actionStun }),
            new SequenceNode(new List<Node> { isArmorRestored, actionUnsStun }),
        });

    }  






    public void TakeDame(float amount)
    {
        if(currentArmor > 0)
        {
            currentArmor -= amount; // Giảm giáp ảo
            UpdateUI();
        }      
        if(currentArmor <= 0)
        {
            currentHp -= amount; // Nếu giáp ảo hết, giảm máu
            UpdateUI();
            if (currentHp <= 0)
            {             
                animator.SetTrigger("Die"); // Kích hoạt hành động chết trong Animator
                
            }
        }
    }
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

    public IEnumerator WaitRegenerateArmor()//khi het giap thi bat dau doi 10f r hoi phuc
    {
        yield return new WaitForSeconds(10f); // Thời gian chờ 5 giây
        currentArmor = maxArmor; // Đặt lại giáp ảo về giá trị tối đa
        isArmorRegenerating = false; // Cho phép lần hồi tiếp theo
        UpdateUI();
    }
}

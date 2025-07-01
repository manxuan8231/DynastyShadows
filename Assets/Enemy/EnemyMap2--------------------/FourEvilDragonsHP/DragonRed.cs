using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class DragonRed : MonoBehaviour
{
    public GameObject player;// người chơi
    public float detectionRangePL = 100f;//phat hien nguoi choi
    public float stopDistance = 14f; // Khoảng cách dừng di chuyển khi đến gần người chơi
    public float moveSpeed = 3.5f; // Tốc độ di chuyển của rồng đỏ
    //attackcooldown
    public float attackCooldown = 2f; // Thời gian chờ giữa các đợt tấn công
    public float lastAttackTime = -2f; // Thời gian của lần tấn công cuối cùng
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

    
    void Start()
    {
       
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

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player"); // Tìm người chơi trong cảnh        
        }
        
    }

    void Update()
    {
        Stun();//stun sau khi bi  het giap
        MoveToPlayer();
        Attack();

    }


    //stun sau khi bi  het giap
    public void Stun()
    {
        if(currentArmor <= 0)//neu giáp ảo <= 0 thì choáng
        {
            if (!isStunned) // Chỉ cho phép chạy hành động này một lần
            {
                isStunned = true; // Đánh dấu là đã bị choáng
                animator.SetTrigger("Stun"); 
                animator.SetBool("IsWalking", false); // Tắt hành động di chuyển trong Animator
                navMeshAgent.isStopped = true; 
              
            }
        }
        if(currentArmor > 0)//nếu giáp ảo > 0 thì không choáng nữa
        {
            if (isStunned) // Chỉ cho phép chạy hành động này một lần
            {
                Debug.Log("het stun");  
                isStunned = false; // Nếu giáp còn, không choáng nữa
                animator.SetTrigger("Idle");

                navMeshAgent.isStopped = false;
            }
            
        }
        // Kiểm tra nếu giáp ảo đã hết và chưa bắt đầu hồi phục
        if (currentArmor <= 0 && !isArmorRegenerating)
        {
            isArmorRegenerating = true;
            StartCoroutine(WaitRegenerateArmor()); // Bắt đầu Coroutine để hồi phục giáp ảo
        }
    }
    // Di chuyển rồng đỏ về phía người chơi nếu trong phạm vi phát hiện
    public void MoveToPlayer()
    {
        if (isStunned) return; //Không di chuyển khi đang stun
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position); // Tính khoảng cách đến người chơi
        if (distanceToPlayer <= detectionRangePL && distanceToPlayer > stopDistance)
        {
            navMeshAgent.SetDestination(player.transform.position); // Di chuyển đến vị trí của người chơi
            animator.SetBool("IsWalking", true); // Kích hoạt hành động di chuyển trong Animator
        }
        else
        {
            navMeshAgent.ResetPath(); // Dừng di chuyển nếu không còn trong phạm vi phát hiện
            animator.SetBool("IsWalking", false); // Tắt hành động di chuyển trong Animator
        }
    }

    public void Attack()
    {
        if (isStunned) return; //Không di chuyển khi đang stun
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if(distanceToPlayer <= stopDistance && Time.time >= lastAttackTime + attackCooldown)
        {
            animator.SetTrigger("AttackHand"); // Kích hoạt hành động tấn công trong Animator         
            lastAttackTime = Time.time; // Cập nhật thời gian của lần tấn công cuối cùng
        }
    }

    

    //ham lay hp----------------------------
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

    //khi het giap thi bat dau doi 10f r hoi phuc
    public IEnumerator WaitRegenerateArmor()
    {
        yield return new WaitForSeconds(10f); // Thời gian chờ 5 giây
        currentArmor = maxArmor; // Đặt lại giáp ảo về giá trị tối đa
        isArmorRegenerating = false; // Cho phép lần hồi tiếp theo
        UpdateUI();
    }
}

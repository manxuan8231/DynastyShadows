using Pathfinding;
using UnityEngine;

public class ControllerStateAssa : MonoBehaviour
{
    
    //tim pl va thong so 
    public GameObject player;
    public float playerRange = 100f;
    public float stopRange = 4f;

    //cooldown attack current   
    public float coolDownAttack = 4f;
    public float lastAttackTime = -4f;
    public float stepAttack = 0f;

    //skill knife kich hoat khi thay player 
    public GameObject autoSkillKnife;
    public float autoSkillCooldown = 120f; // 2 phút
    public float lastAutoSkillTime = -120f;
    public float autoSkillDuration = 10f;

    //skill dash va tao bong
    public float cooldownSkillDash = 4f;
    public float lastTimeSkillDash = -4f;
    public float randomMoveSkillDash;
    //tham chieu
    public AIPath aiPath;
    public Animator animator;
    public Collider boxTakeDame;
    // Trạng thái hiện tại
    private AssasinState currentState;
    private void Start()
    {
        player = GameObject.FindWithTag("Player");     
        aiPath = GetComponent<AIPath>();
        animator = GetComponent<Animator>();
      
        autoSkillKnife.SetActive(false);
        ChangeState(new CurrentStateAssa(this));
    }
    private void Update()
    {
       
        currentState?.Update();
    }
    public void ChangeState(AssasinState newState)
    {
        currentState?.Exit();     // Thoát trạng thái cũ 
        currentState = newState;  // Gán trạng thái mới
        currentState.Enter();     // Kích hoạt trạng thái mới
    }
    

}

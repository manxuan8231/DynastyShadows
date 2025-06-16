// Enemy AI: Đứng yên → Phát hiện Player → Di chuyển đến gần khoảng cách tấn công → Đánh → Lùi lại → Chờ cooldown → Tấn công tiếp

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class MinotaurEnemy : MonoBehaviour
{
    public Transform targetPlayer;//người chơi
    public float detectionRange = 10f;//phát hiện người chơi
    public float attackRange = 2f;// khoảng cách tấn công người chơi
    public Slider sliderHp;
    public float currentHealth = 100f;// máu hiện tại 
    public float maxHealth = 100f;// máu tối đa
    public bool isDead = false;// trạng thái chết của kẻ thù
    //cooldown tấn công
    public float attackCooldown = 4f;// thời gian chờ giữa các đợt tấn công
    public float _lastAttackTime = -4f;// thời gian tấn công cuối cùng


    private Node _rootNode;// Cây hành vi gốc
    private Animator _animator;
    private NavMeshAgent _agent;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        targetPlayer = FindClosestPlayer(); // Tìm người chơi gần nhất
        currentHealth = maxHealth; // Khởi tạo máu hiện tại bằng máu tối đa
        sliderHp.maxValue = currentHealth; // Thiết lập giá trị tối đa của thanh máu
        sliderHp.value = currentHealth; // Thiết lập giá trị hiện tại của thanh máu
        SetupBehaviorTree();// thiết lập cây hành vi
    }

    void Update()
    {
        if (isDead) return; // Nếu đã chết, không thực hiện hành động nào khác    
        targetPlayer = FindClosestPlayer(); // Cập nhật người chơi gần nhất mỗi khung hình
        _rootNode?.Evaluate();
    }
   
    private void SetupBehaviorTree()
    {
        //xu ly die-------------------
        var isDead = new ConditionNode(() => currentHealth <= 0);// kiểm tra xem  đã chết hay chưa
        var deathAction = new ActionNode(PlayDeathAnimation);// hành động khi  chết

        // xu ly di chuyển đến người chơi -----------------
        var isMove = new ConditionNode(ISPlayerMove);  
        var moveToPlayerAction = new ActionNode(MoveToPlayer);

        //xu ly tấn công người chơi---------------------
        var isPlayerInAttackRange = new ConditionNode(IsPlayerInAttackRange);// kiểm tra xem người chơi có trong khoảng tấn công hay không
        var isCooldownAttack = new ConditionNode(() => Time.time >= _lastAttackTime + attackCooldown);// kiểm tra cooldown tấn công                                                                               
        var attackAction = new ActionNode(AttackPlayer);// hành động tấn công người chơi    
        var waitAction = new ActionNode(WaitForCooldown); // Viết hàm đứng yên chờ cooldown

        // hành vi idle------------------
        var idleAction = new ActionNode(Idle); // hành vi idle

        // Cây hành vi gốc
        _rootNode = new SelectorNode(new List<Node>
        {
             //die
             new SequenceNode(new List<Node> { isDead, deathAction }),
             //move
             new SequenceNode(new List<Node> {isMove, moveToPlayerAction}),
             //Attack
             new SequenceNode(new List<Node> { isPlayerInAttackRange, isCooldownAttack, attackAction, waitAction, }),                                         
              //idle
             idleAction

        });
    }
    //battle-----------------
    private bool ISPlayerMove()// phát hiện người chơi
    {
        return targetPlayer != null && Vector3.Distance(transform.position, targetPlayer.position) <= detectionRange &&
                                       Vector3.Distance(transform.position, targetPlayer.position) >=  attackRange;
    }

    private NodeState MoveToPlayer()
    {
        if (targetPlayer == null) return NodeState.FAILURE;
        targetPlayer = FindClosestPlayer(); // Tìm người chơi gần nhất
        _agent.isStopped = false;
        _agent.SetDestination(targetPlayer.position);
        _animator.SetBool("IsRunning", true);
        return NodeState.RUNNING;
    }


    //attack----------
    private bool IsPlayerInAttackRange()// khoảng cách tấn công người chơi
    {
        return targetPlayer != null && Vector3.Distance(transform.position,
                                                         targetPlayer.position) <= attackRange;
    }
   
    private NodeState AttackPlayer()
    {
        if (targetPlayer == null) return NodeState.FAILURE;
        _animator.SetBool("IsRunning", false); // tắt animation chạy
        _agent.isStopped = true;
        transform.LookAt(targetPlayer);
        if (currentHealth > 50) {
            _animator.SetTrigger("Attack");
        }
        else if(currentHealth <= 50 && currentHealth > 0) {
            _animator.SetTrigger("Attack2");
        }
            Debug.Log(" tấn công");
        _lastAttackTime = Time.time;// cập nhật thời gian tấn công cuối cùng
        return NodeState.SUCCESS;// tấn công thành công
    }


    private NodeState WaitForCooldown()
    {
        _agent.isStopped = true;
        _animator.SetBool("IsRunning", false);
        return NodeState.RUNNING;
    }
    // chết----------------  
    private NodeState PlayDeathAnimation()
    {
        if (isDead == false)
        {
            isDead = true; // đánh dấu là đã chết
            _animator.SetTrigger("Die");
            _agent.isStopped = true;
            Debug.Log("Hilichurl đã chết");
           
        }
        return NodeState.SUCCESS;// đã chết thành công
    }
    private NodeState Idle()
    {
        _agent.isStopped = true;
        _animator.SetBool("IsRunning", false); // tắt animation chạy
                                                // Không set trigger attack hay die, vì idle là trạng thái mặc định
        return NodeState.SUCCESS;
    }

    Transform FindClosestPlayer()// tìm người chơi gần nhất
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        Transform closestPlayer = null;
        float closestDistance = Mathf.Infinity;
        foreach (GameObject player in players)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPlayer = player.transform;
            }
        }
        return closestPlayer;
    }
    public void TakeDamage(float damage)
    {
        Debug.Log($"Minotaur nhận {damage} sát thương");
        currentHealth -= damage;
        sliderHp.value = currentHealth; // Cập nhật thanh máu
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Đảm bảo máu không âm
        if (currentHealth <= 0)
        {
           StartCoroutine(WaitAnimatorDie()); // Bắt đầu coroutine để xử lý animation chết
        }
    }

    public IEnumerator WaitAnimatorDie()
    {
        yield return new WaitForSeconds(2f); // Thời gian chờ để animation chết hoàn thành
        ObjPoolingManager.Instance.ReturnToPool("Minotaur1", gameObject); // Trả về pool sau khi animation chết hoàn thành
    }
}

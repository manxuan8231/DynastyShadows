using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyEvil : MonoBehaviour
{
    public Transform targetPlayer;//người chơi
    public float detectionRange = 10f;//phát hiện người chơi
    public float attackRange = 2f;// khoảng cách tấn công người chơi
    public float currentHealth = 100f;// máu hiện tại của Hilichurl
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

        SetupBehaviorTree();// thiết lập cây hành vi
    }

    void Update()
    {
        _rootNode?.Evaluate();
    }

    private void SetupBehaviorTree()
    {
        var isDead = new ConditionNode(() => currentHealth <= 0);// kiểm tra xem Hilichurl đã chết hay chưa
        var deathAction = new ActionNode(PlayDeathAnimation);// hành động khi Hilichurl chết

        var isPlayerInAttackRange = new ConditionNode(IsPlayerInAttackRange);// kiểm tra xem người chơi có trong khoảng tấn công hay không
        var attackAction = new ActionNode(AttackPlayer);// hành động tấn công người chơi

        var isPlayerInDetectionRange = new ConditionNode(IsPlayerInDetectionRange);// kiểm tra xem người chơi có trong khoảng phát hiện hay không
        var moveToPlayer = new ActionNode(MoveToPlayer);// hành động di chuyển đến người chơi

        var idleAction = new ActionNode(Idle); // hành vi idle
        // Cây hành vi gốc
        _rootNode = new SelectorNode(new List<Node>
        {
            new SequenceNode(new List<Node> { isDead, deathAction }),
            new SequenceNode(new List<Node> { isPlayerInAttackRange, attackAction }),
            new SequenceNode(new List<Node> { isPlayerInDetectionRange, moveToPlayer }),
             idleAction//hành động idle cuối cùng
        });
    }

    // Hành vi  nút 

    private bool IsPlayerInDetectionRange()//khoảng cách phát hiện người chơi
    {
        float distance = Vector3.Distance(transform.position, targetPlayer.position);
        return distance <= detectionRange && distance >= attackRange && targetPlayer != null; // kiểm tra khoảng cách và người chơi có tồn tại không
    }

    private bool IsPlayerInAttackRange()// khoảng cách tấn công người chơi
    {
        return targetPlayer != null && Vector3.Distance(transform.position,
                                                         targetPlayer.position) <= attackRange;
    } 

    private NodeState MoveToPlayer()
    {
        if (targetPlayer == null) return NodeState.FAILURE;//

        _agent.isStopped = false;
        _agent.SetDestination(targetPlayer.position);

        _animator?.SetBool("IsRunning", true);

        return NodeState.RUNNING;// đang di chuyển đến người chơi
    }

    private NodeState AttackPlayer()
    {
        if (targetPlayer == null) return NodeState.FAILURE;
        if (Time.time >= _lastAttackTime + attackCooldown) // kiểm tra cooldown tấn công
        {
            _agent.isStopped = true;
            transform.LookAt(targetPlayer);
            _animator?.SetBool("IsRunning", false); // tắt animation chạy
            _animator?.SetTrigger("Attack");

            Debug.Log("Hilichurl tấn công!");
            _lastAttackTime = Time.time;// cập nhật thời gian tấn công cuối cùng
          
        }
       
       
        return NodeState.SUCCESS;// tấn công thành công
    }

    private NodeState PlayDeathAnimation()
    {
        _animator?.SetTrigger("Die");
        _agent.isStopped = true;
        Debug.Log("Hilichurl đã chết");
        return NodeState.SUCCESS;// đã chết thành công
    }
    private NodeState Idle()
    {
        _agent.isStopped = true;
        _animator?.SetBool("IsRunning", false); // tắt animation chạy
                                                // Không set trigger attack hay die, vì idle là trạng thái mặc định
        return NodeState.SUCCESS;
    }

}

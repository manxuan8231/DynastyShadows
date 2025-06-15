// Enemy AI: Đứng yên → Phát hiện Player → Di chuyển đến gần khoảng cách tấn công → Đánh → Lùi lại → Chờ cooldown → Tấn công tiếp

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MinotaurEnemy : MonoBehaviour
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
        var backOffAction = new ActionNode(BackOff);// hành động lùi lại sau khi tấn công
        var waitAction = new ActionNode(WaitForCooldown); // Viết hàm đứng yên chờ cooldown

        // hành vi idle------------------
        var idleAction = new ActionNode(Idle); // hành vi idle

        // Cây hành vi gốc
        _rootNode = new SelectorNode(new List<Node>
        {
             //die
             new SequenceNode(new List<Node> { isDead, deathAction }),
             //move
             new SequenceNode(new List<Node> { isCooldownAttack, isMove, moveToPlayerAction}),
             //Attack
             new SequenceNode(new List<Node> { isPlayerInAttackRange,isCooldownAttack, 
                                                attackAction, backOffAction,waitAction }),
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

        _animator.SetTrigger("Attack");

        Debug.Log(" tấn công");
        _lastAttackTime = Time.time;// cập nhật thời gian tấn công cuối cùng
        return NodeState.SUCCESS;// tấn công thành công
    }
    private NodeState BackOff()
    {
        if (targetPlayer == null) return NodeState.FAILURE;

        _agent.isStopped = false;

        Vector3 runDirection = transform.position - targetPlayer.position;
        Vector3 fleePosition = transform.position + runDirection.normalized * 5f;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(fleePosition, out hit, 10f, NavMesh.AllAreas))
        {
            _agent.SetDestination(hit.position);
            _animator.SetBool("isRunBack", true);

            // Nếu đã gần đến điểm lùi lại thì coi là lùi thành công
            if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
            {
                _animator.SetBool("isRunBack", false); // tắt animation lùi lại
                return NodeState.SUCCESS;
            }

            return NodeState.RUNNING;
        }

        Debug.LogWarning("Không tìm được điểm lùi hợp lệ");
        return NodeState.FAILURE;
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
        _animator.SetTrigger("Die");
        _agent.isStopped = true;
        Debug.Log("Hilichurl đã chết");
        return NodeState.SUCCESS;// đã chết thành công
    }
    private NodeState Idle()
    {
        _agent.isStopped = true;
        _animator.SetBool("IsRunning", false); // tắt animation chạy
                                                // Không set trigger attack hay die, vì idle là trạng thái mặc định
        return NodeState.SUCCESS;
    }
}

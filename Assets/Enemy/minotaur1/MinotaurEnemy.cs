// Enemy AI: Đứng yên → Phát hiện Player → Di chuyển đến gần khoảng cách tấn công → Đánh → Lùi lại → Chờ cooldown → Tấn công tiếp

using System.Collections.Generic;
using UnityEngine;

public class MinotaurEnemy : MonoBehaviour
{
    public Transform targetPlayer;
    public float detectionRange = 10f;
    public float attackRange = 2f;
    public float cooldownTime = 3f;
    public float currentCooldown = 0f;
    public float attackBackDistance = 2f;

    public float moveSpeed = 3f;

    private Node _rootNode;
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        SetupBehaviorTree();
    }

    private void Update()
    {
        if (currentCooldown > 0)
            currentCooldown -= Time.deltaTime;

        _rootNode?.Evaluate();
    }

    private void SetupBehaviorTree()
    {
        var isPlayerDetected = new ConditionNode(() => Vector3.Distance(transform.position, targetPlayer.position) <= detectionRange);
        var isInAttackRange = new ConditionNode(() => Vector3.Distance(transform.position, targetPlayer.position) <= attackRange);
        var isCooldownReady = new ConditionNode(() => currentCooldown <= 0f);

        var moveToPlayer = new ActionNode(MoveTowardPlayer);
        var attackPlayer = new ActionNode(Attack);
        var backAway = new ActionNode(BackAwayAfterAttack);

        _rootNode = new SelectorNode(new List<Node>
        {
            new SequenceNode(new List<Node>
            {
                isPlayerDetected,
                new SelectorNode(new List<Node>
                {
                    new SequenceNode(new List<Node>
                    {
                        isInAttackRange,
                        isCooldownReady,
                        attackPlayer,
                        backAway
                    }),
                    moveToPlayer
                })
            })
        });
    }

    private NodeState MoveTowardPlayer()
    {
        if (targetPlayer == null) return NodeState.FAILURE;
        transform.position = Vector3.MoveTowards(transform.position, targetPlayer.position, moveSpeed * Time.deltaTime);
        if (_animator != null) _animator.SetBool("isRunning", true);
        return NodeState.RUNNING;
    }

    private NodeState Attack()
    {
        Debug.Log("Enemy attacks player");
        if (_animator != null) _animator.SetTrigger("Attack");
        currentCooldown = cooldownTime;
        return NodeState.SUCCESS;
    }

    private NodeState BackAwayAfterAttack()
    {
        Vector3 dir = (transform.position - targetPlayer.position).normalized;
        Vector3 backPos = transform.position + dir * attackBackDistance;
        transform.position = Vector3.MoveTowards(transform.position, backPos, moveSpeed * Time.deltaTime);
        return NodeState.RUNNING;
    }
}

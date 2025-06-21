using UnityEngine;
using UnityEngine.AI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class NPCController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float patrolRadius = 10f;
    public float waitTime = 2f;

    [Header("Conversation Settings")]
    public float talkDistance = 3f;
    public float talkDuration = 3f;
    public int movesBeforeTalking = 3;

    [Header("UI")]
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;

    private NavMeshAgent agent;
    private Animator animator;

    private bool isTalking = false;
    private bool isWaiting = false;
    private bool isSeekingConversation = false;

    private int moveCount = 0;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);

        GetNewDestination();
    }

    void Update()
    {
        if (isTalking || isWaiting)
        {
            animator.SetFloat("Speed", 0f);
            return;
        }

        animator.SetFloat("Speed", agent.velocity.magnitude);

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            if (isSeekingConversation)
            {
                TryStartConversationWithNearbyNPC();
            }
            else
            {
                StartCoroutine(WaitAndMove());
            }
        }
    }

    void GetNewDestination()
    {
        if (moveCount >= movesBeforeTalking)
        {
            isSeekingConversation = true;
            return; // dừng chọn điểm mới, chuẩn bị đi tìm NPC khác
        }

        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
        randomDirection += transform.position;

        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, patrolRadius, 1))
        {
            agent.SetDestination(hit.position);
            moveCount++;
        }
    }

    IEnumerator WaitAndMove()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTime);

        if (moveCount >= movesBeforeTalking)
        {
            isSeekingConversation = true;
            GoToClosestNPC();
        }
        else
        {
            GetNewDestination();
        }

        isWaiting = false;
    }

    void GoToClosestNPC()
    {
        NPCController[] allNPCs = FindObjectsOfType<NPCController>();
        float closestDist = Mathf.Infinity;
        NPCController closestNPC = null;

        foreach (var npc in allNPCs)
        {
            if (npc == this || npc.IsTalking()) continue;

            float dist = Vector3.Distance(transform.position, npc.transform.position);
            if (dist < closestDist)
            {
                closestDist = dist;
                closestNPC = npc;
            }
        }

        if (closestNPC != null)
        {
            agent.SetDestination(closestNPC.transform.position);
        }
    }

    void TryStartConversationWithNearbyNPC()
    {
        NPCController[] allNPCs = FindObjectsOfType<NPCController>();
        foreach (var npc in allNPCs)
        {
            if (npc == this || npc.IsTalking()) continue;

            float dist = Vector3.Distance(transform.position, npc.transform.position);
            if (dist <= talkDistance && dist >= 1.5f)
            {
                isSeekingConversation = false;
                moveCount = 0;

                TryStartConversation(npc);
                npc.TryStartConversation(this);
                return;
            }
        }

        // Không tìm thấy → tiếp tục chọn NPC khác
        GoToClosestNPC();
    }

    public void TryStartConversation(NPCController other)
    {
        if (!isTalking && !other.isTalking)
        {
            StartCoroutine(ConversationRoutine(other));
        }
    }

    IEnumerator ConversationRoutine(NPCController other)
    {
        isTalking = true;
        agent.isStopped = true;
        animator.SetFloat("Speed", 0f);

        // Quay mặt về phía nhau
        Vector3 lookDir = (other.transform.position - transform.position).normalized;
        lookDir.y = 0f; // không thay đổi góc nhìn theo trục Y
        if (lookDir != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(lookDir);

        // Hiện UI
        if (dialoguePanel != null && dialogueText != null)
        {
            dialoguePanel.SetActive(true);
            dialogueText.text = $"{gameObject.name}: Xin chào {other.gameObject.name}!";
        }

        yield return new WaitForSeconds(talkDuration);

        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);

        agent.isStopped = false;
        GetNewDestination();
        isTalking = false;
    }


    public bool IsTalking()
    {
        return isTalking;
    }
}

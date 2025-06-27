using UnityEngine;
using UnityEngine.AI;
using TMPro;
using System.Collections;

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
    public NPCConversation conversation;

    private NavMeshAgent agent;
    private Animator animator;

    private bool isTalking = false;
    private bool isWaiting = false;
    private bool isSeekingConversation = false;
    private bool hasTalkedWith = false;

    private int moveCount = 0;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        conversation = GetComponent<NPCConversation>();


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
                TryStartConversationWithNearbyNPC();
            else
                StartCoroutine(WaitAndMove());
        }
    }

    void GetNewDestination()
    {
        if (moveCount >= movesBeforeTalking)
        {
            isSeekingConversation = true;
            GoToClosestNPC();
            return;
        }

        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius + transform.position;

        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, patrolRadius, NavMesh.AllAreas))
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
            Vector3 direction = (closestNPC.transform.position - transform.position).normalized;
            float safeDistance = talkDistance * 0.9f; // Stop at 90% of the talk distance

            Vector3 stopPosition = closestNPC.transform.position - direction * safeDistance;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(stopPosition, out hit, 1f, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
            }
        }
    }


    void TryStartConversationWithNearbyNPC()
    {
        if (hasTalkedWith) return;

        foreach (var npc in FindObjectsOfType<NPCController>())
        {
            if (npc == this || npc.IsTalking() || npc.hasTalkedWith) continue;

            float dist = Vector3.Distance(transform.position, npc.transform.position);
            if (dist <= talkDistance)
            {
                // Bắt đầu hội thoại giữa hai NPC
                isSeekingConversation = false;
                moveCount = 0;
                hasTalkedWith = true;
                npc.hasTalkedWith = true;

                StartCoroutine(ConversationRoutine(npc));
                return;
            }
        }
    }

    IEnumerator ConversationRoutine(NPCController other)
    {
        isTalking = true;
        agent.isStopped = true;
        other.ReceiveConversation(this); // Cho NPC kia dừng lại

        // Cả hai quay mặt về nhau
        yield return StartCoroutine(RotateTowards(other.transform));
        StartCoroutine(other.RotateTowards(transform)); // NPC kia cũng quay mặt lại

        // Hiện thoại
        if (dialoguePanel && dialogueText)
        {
            dialoguePanel.SetActive(true);
            dialogueText.text = $"{gameObject.name}: Xin chào {other.gameObject.name}!";
        }

        yield return new WaitForSeconds(talkDuration);

        if (dialoguePanel)
            dialoguePanel.SetActive(false);

        isTalking = false;
        hasTalkedWith = false;
        agent.isStopped = false;
        GetNewDestination();

        other.EndConversation(); // Cho NPC kia tiếp tục di chuyển
    }

    public void ReceiveConversation(NPCController from)
    {
        isTalking = true;
        agent.isStopped = true;
        animator.SetFloat("Speed", 0f);
    }
    public void BeginConversation()
    {
        isTalking = true;
        agent.isStopped = true;
        animator.SetFloat("Speed", 0f);
    }
    public void EndConversation()
    {
        isTalking = false;
        hasTalkedWith = false;
        agent.isStopped = false;
        GetNewDestination();
    }

    public IEnumerator RotateTowards(Transform target)
    {
        float rotSpeed = 5f;
        Vector3 dir = (target.position - transform.position).normalized;
        dir.y = 0f;

        Quaternion targetRot = Quaternion.LookRotation(dir);
        float t = 0;

        while (Quaternion.Angle(transform.rotation, targetRot) > 1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, t);
            t += Time.deltaTime * rotSpeed;
            yield return null;
        }
    }

    public bool IsTalking()
    {
        return isTalking;
    }

}

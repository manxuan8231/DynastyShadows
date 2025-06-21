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

    [Header("UI")]
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;

    private NavMeshAgent agent;
    private Animator animator;
    private bool isTalking = false;
    private bool isWaiting = false;

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
            StartCoroutine(WaitAndMove());
        }
    }

    void GetNewDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
        randomDirection += transform.position;

        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, patrolRadius, 1))
        {
            agent.SetDestination(hit.position);
        }
    }

    IEnumerator WaitAndMove()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTime);
        GetNewDestination();
        isWaiting = false;
    }

    public void TryStartConversation(NPCController other)
    {
        if (!isTalking && !other.isTalking)
        {
            StartCoroutine(ConversationRoutine(other));
            other.StartCoroutine(other.ConversationRoutine(this));
        }
    }

    public IEnumerator ConversationRoutine(NPCController other)
    {
        isTalking = true;
        agent.isStopped = true;
        animator.SetFloat("Speed", 0f);

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

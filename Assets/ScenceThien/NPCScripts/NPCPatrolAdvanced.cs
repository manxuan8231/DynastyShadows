using UnityEngine;
using TMPro;
using System.Collections;
using Pathfinding;

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

    private AIPath aiPath;
    private AIDestinationSetter destinationSetter;
    private Animator animator;

    private bool isTalking = false;
    private bool isWaiting = false;
    private bool isSeekingConversation = false;
    private bool hasTalkedWith = false;

    private int moveCount = 0;
    private Transform currentTarget;

    void Start()
    {
        aiPath = GetComponent<AIPath>();
        destinationSetter = GetComponent<AIDestinationSetter>();
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

        animator.SetFloat("Speed", aiPath.desiredVelocity.magnitude);

        if (!aiPath.pathPending && aiPath.reachedDestination)
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
        randomDirection.y = transform.position.y;

        GraphNode node = AstarPath.active.GetNearest(randomDirection).node;
        if (node != null && node.Walkable)
        {
            currentTarget = new GameObject("PatrolTarget").transform;
            currentTarget.position = (Vector3)node.position;
            destinationSetter.target = currentTarget;
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
            float safeDistance = talkDistance * 0.9f;
            Vector3 stopPosition = closestNPC.transform.position - direction * safeDistance;

            GraphNode node = AstarPath.active.GetNearest(stopPosition).node;
            if (node != null && node.Walkable)
            {
                currentTarget = new GameObject("TalkTarget").transform;
                currentTarget.position = (Vector3)node.position;
                destinationSetter.target = currentTarget;
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
        aiPath.canMove = false;
        other.ReceiveConversation(this);

        yield return StartCoroutine(RotateTowards(other.transform));
        StartCoroutine(other.RotateTowards(transform));

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
        aiPath.canMove = true;
        Destroy(currentTarget?.gameObject);
        GetNewDestination();

        other.EndConversation();
    }

    public void ReceiveConversation(NPCController from)
    {
        isTalking = true;
        aiPath.canMove = false;
        animator.SetFloat("Speed", 0f);
    }

    public void BeginConversation()
    {
        isTalking = true;
        aiPath.canMove = false;
        animator.SetFloat("Speed", 0f);
    }

    public void EndConversation()
    {
        isTalking = false;
        hasTalkedWith = false;
        aiPath.canMove = true;
        Destroy(currentTarget?.gameObject);
        GetNewDestination();
    }


    public IEnumerator RotateTowards(Transform target)
    {
        float rotSpeed = 5f;
        Vector3 dir = (target.position - transform.position).normalized;
        dir.y = 0f;

        Quaternion targetRot = Quaternion.LookRotation(dir);
        while (Quaternion.Angle(transform.rotation, targetRot) > 1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * rotSpeed);
            yield return null;
        }
    }

    public bool IsTalking()
    {
        return isTalking;
    }
}

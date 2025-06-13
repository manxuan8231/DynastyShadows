using System.Collections;
using System.Linq;
using TMPro;
using Unity.VisualScripting.FullSerializer;
using UnityEditor.Rendering.PostProcessing;
using UnityEngine;
using UnityEngine.AI;

public class NPCQuest : MonoBehaviour
{
    public float detectionRange = 15f; // Khoảng cách thấy enemy
    public float attackRange = 2f;     // Khoảng cách tấn công
    public float attackCooldown = 1.5f;

    private Transform targetEnemy;
    private float lastAttackTime = 0f;

    public Animator animator;
    public NavMeshAgent agent;
    public EnemyMap2_1 enemyMap2_1;
    public int killEnemy = 0;
    bool isHelp = false;
    public GameObject questionGameCanvas;
    public TMP_Text nameTxt;
    public TMP_Text contentText;
    public GameObject btnF;
    public GameObject PanelContent;
    public string[] contentTextQuest;
    public string[] nameTextQuest;
    bool isOpen;
    bool isTyping;
    bool isSkip;
    bool isWriteSkip;
    public bool isContent;
    bool hasFinishedDialogue = false; // THÊM BIẾN NÀY
    bool hasPlayedTalkingAnim = false;
    public bool quest2;




    Coroutine Coroutine;
    private void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        enemyMap2_1 = FindFirstObjectByType<EnemyMap2_1>();
        if (killEnemy >= 6)
        {
            isHelp = true;
        }
        FindClosestEnemy();
        if (targetEnemy != null)
        {
            float distance = Vector3.Distance(transform.position, targetEnemy.position);

            if (distance <= attackRange)
            {
                StopMoving();
                AttackEnemy();
            }
            else
            {
                MoveToTarget();
            }
        }
        else
        {
            StopMoving();
        }
        if (isHelp && !hasFinishedDialogue)
        {
            if (!hasPlayedTalkingAnim)
            {
                animator.SetTrigger("Talking");
                hasPlayedTalkingAnim = true;
            }

            if (isOpen)
            {
                questionGameCanvas.SetActive(true);
                btnF.SetActive(true);

                if (Input.GetKeyDown(KeyCode.F) && isContent)
                {
                    isContent = false;
                    btnF.SetActive(false);
                    PanelContent.SetActive(true);
                    Coroutine = StartCoroutine(ReadContent());
                }
            }
        }


    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            isOpen = true;
        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            isOpen = false;
        }
    }

    // Tìm enemy gần nhất trong khoảng detectionRange
    void FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("EnemyMap2_2");

        targetEnemy = enemies
            .Where(e => Vector3.Distance(transform.position, e.transform.position) <= detectionRange)
            .OrderBy(e => Vector3.Distance(transform.position, e.transform.position))
            .Select(e => e.transform)
            .FirstOrDefault();
    }

    void MoveToTarget()
    {
        if (targetEnemy != null)
        {

            agent.SetDestination(targetEnemy.position);
            animator.SetTrigger("Run");
        }
    }

    void StopMoving()
    {
        agent.ResetPath();
        animator.ResetTrigger("Run");
    }

    void AttackEnemy()
    {
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            lastAttackTime = Time.time;
            animator.SetTrigger("Attack");

        }
    }
    public void UpdateKillQuest()
    {
        killEnemy++;
    }
    private IEnumerator ReadContent()
    {

        for (int i = 0; i < contentTextQuest.Length; i++)
        {
            contentText.text = "";
            nameTxt.text = nameTextQuest.Length > i ? nameTextQuest[i] : "Đéo biết ai ??" ;
            isTyping = true;
            isWriteSkip = false;
            foreach (var content in contentTextQuest[i])
            {
                if(isSkip)
                {
                    contentText.text = contentTextQuest[i];
                    break;
                }
                contentText.text += content;

                yield return new WaitForSeconds(0.05f); // Thời gian giữa các ký tự

            }
            isTyping = false;
            isSkip = false;
            isWriteSkip = true;
            while (!isSkip)
            {
                yield return null;
            }
            isWriteSkip = false;
        
        }
        EndContent();
       


    }
    public void SkipBtn()
    {
        if (isTyping)
        {
            isSkip = true;
        }
        else if (isWriteSkip)
        {
            isSkip = true;
        }
    }
    public void EndContent()
    {
        quest2 = true;
        questionGameCanvas.SetActive(false);
        hasFinishedDialogue = true; // Đánh dấu đã xong hội thoại
        if (Coroutine != null)
        {
            StopCoroutine(Coroutine);
            
        }

    }
   
}

using Pathfinding;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class NPCQuest : MonoBehaviour
{
    public Animator animator;
    public EnemyMap2_1 enemyMap2_1;
    public Transform player;
    public Transform succesQuestPoint;
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
    public bool isActiveBtn = false;
   public bool hasFinishedDialogue = false; // THÊM BIẾN NÀY
    bool hasPlayedTalkingAnim = false;


    public GameObject trigger;
    Coroutine Coroutine;
    public bool isSitUp = false;
    public GameObject canvasNameNPC;
    [Header("Nhiệm vụ")]
    public GameObject canvasQuest;
    public TMP_Text questText;

    [Header("Trạng thái nhiệm vụ")]
    public GameObject stateCanvas;
    public TMP_Text stateText;
    public TMP_Text missionName;
    public Image iconState;
    public Sprite spirteState;

    public AudioCanvasState audioCanvasState;
    public SuccesQuest2 succesQuest2;
    public GameObject obj;
    AIPath aiPath;
    private void Start()
    {
        obj.SetActive(false); 
        canvasNameNPC.SetActive(false); // Ẩn canvas tên NPC ban đầu
        isContent = true;
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform; // Tìm player trong scene
        succesQuest2 = FindFirstObjectByType<SuccesQuest2>();
        aiPath = GetComponent<AIPath>();
    }

    void Update()
    {
        enemyMap2_1 = FindFirstObjectByType<EnemyMap2_1>();
        if (killEnemy >= 6 && !isSitUp)
        {
            if (isSitUp == false)
            {
                animator.SetTrigger("SitUp");
                isSitUp = true;
            }
            trigger.SetActive(false); // Tắt trigger khi đã hoàn thành nhiệm vụ
            isHelp = true;
        }

        if (isHelp && !hasFinishedDialogue)
        {
            if (isOpen && !isActiveBtn)
            {
                questionGameCanvas.SetActive(true);
                btnF.SetActive(true);

                if (Input.GetKeyDown(KeyCode.F) && isContent)
                {
                    if (!hasPlayedTalkingAnim)
                    {
                        animator.SetTrigger("Talking");
                        hasPlayedTalkingAnim = true; // Đánh dấu đã chơi animation Talking
                    }
                    isActiveBtn = true; // Đánh dấu nút đã được nhấn
                    isContent = false;
                    btnF.SetActive(false);
                    PanelContent.SetActive(true);
                    Coroutine = StartCoroutine(ReadContent());
                }
            }

        }
        if (succesQuest2 != null && succesQuest2.isQuest2Complete)
        {
            MoveToDoneQuest();
        }
        // 🧯 Ngắt follow nếu đã có lệnh đi đến điểm hoàn thành
        else if (hasFinishedDialogue)
        {
            FollowPlayer();
        }


    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            isOpen = true;
        }
        

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            isOpen = false;
            btnF.SetActive(false); // Ẩn nút F khi người chơi rời khỏi vùng trigger
        }
    }
        
    // Tìm enemy gần nhất trong khoảng detectionRange

    public void UpdateKillQuest()
    {
        killEnemy++;
    }
    private IEnumerator ReadContent()
    {
        PlayerControllerState playerControllerState = FindFirstObjectByType<PlayerControllerState>();
        playerControllerState.enabled = false; // Vô hiệu hóa điều khiển người chơi
        playerControllerState.animator.enabled = false; // Vô hiệu hóa animator của người chơi
        Cursor.lockState = CursorLockMode.None; // Mở khóa con trỏ chuột
        Cursor.visible = true; // Hiển thị con trỏ chuột
        for (int i = 0; i < contentTextQuest.Length; i++)
        {
            contentText.text = "";
            nameTxt.text = nameTextQuest.Length > i ? nameTextQuest[i] : "Đéo biết ai ??";
            isTyping = true;
            isWriteSkip = false;
            foreach (var content in contentTextQuest[i])
            {
                if (isSkip)
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
        canvasNameNPC.SetActive(true); // Hiển thị canvas tên NPC sau khi kết thúc hội thoại
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
        PlayerControllerState playerControllerState = FindFirstObjectByType<PlayerControllerState>();
        playerControllerState.enabled = true; // Bật lại điều khiển người chơi
        playerControllerState.animator.enabled = true; // Bật lại animator của người chơi
        Cursor.lockState = CursorLockMode.Locked; // Khóa con trỏ chuột
        Cursor.visible = false; // Ẩn con trỏ chuột
        questionGameCanvas.SetActive(false);
        PanelContent.SetActive(false);
        hasFinishedDialogue = true; // Đánh dấu đã xong hội thoại
        obj.SetActive(true); // Kích hoạt đối tượng khác nếu cần
        if (Coroutine != null)
        {
            StopCoroutine(Coroutine);

        }
        StartCoroutine(NewQuest()); // Bắt đầu nhiệm vụ mới



    }
    IEnumerator NewQuest()
    {
        yield return new WaitForSeconds(1f); // Thời gian chờ trước khi hiển thị nhiệm vụ mới
        canvasQuest.SetActive(true); // Hiển thị canvas nhiệm vụ
        questText.text = "Nhiệm vụ:Đưa dân làng vào vùng an toàn !";
        stateCanvas.SetActive(true); // Hiển thị canvas trạng thái nhiệm vụ
        audioCanvasState.PlayNewQuest();
        stateText.text = "Bạn vừa nhận nhiệm vụ mới !";
        missionName.text = "Đưa dân làng vào vùng an toàn !";
        iconState.sprite = spirteState; // Cập nhật biểu tượng trạng thái nhiệm vụ
        yield return new WaitForSeconds(3f); // Thời gian hiển thị nhiệm vụ
        stateCanvas.SetActive(false); // Ẩn canvas trạng thái nhiệm vụ sau khi hiển thị
    }
    void FollowPlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (hasFinishedDialogue)
        {
            if (distanceToPlayer > 5f)
            {
                aiPath.canSearch = true; // Bật AIPath để NPC có thể di chuyển
                aiPath.canMove = true; // Bật di chuyển
                aiPath.destination = player.position; // Di chuyển đến vị trí của người chơi
                animator.SetTrigger("Walk");
            }
            else
            {
                animator.SetTrigger("Idle");
            }
        }
    }
    
  public  void MoveToDoneQuest()
    {
        if(succesQuest2.isQuest2Complete == true)
        {
            float distanceToPoint = Vector3.Distance(transform.position, succesQuestPoint.position);
            if(distanceToPoint > 1f)
            {
                aiPath.canSearch = true; // Bật AIPath để NPC có thể di chuyển
                aiPath.canMove = true; // Bật di chuyển
                aiPath.destination = succesQuestPoint.position; // Di chuyển đến điểm hoàn thành nhiệm vụ
                animator.SetTrigger("Walk");
            }
            else
            {
                gameObject.SetActive(false); // Tắt NPC khi đã đến điểm hoàn thành nhiệm vụ
                obj.SetActive(false); // Tắt đối tượng khác nếu cần
            }

        }
    }
}

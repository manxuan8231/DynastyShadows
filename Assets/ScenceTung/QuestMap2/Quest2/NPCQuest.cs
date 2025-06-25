using System.Collections;
using System.Linq;
using TMPro;

using UnityEngine;
using UnityEngine.AI;

public class NPCQuest : MonoBehaviour
{
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
    public bool isActiveBtn = false;
    bool hasFinishedDialogue = false; // THÊM BIẾN NÀY
    bool hasPlayedTalkingAnim = false;
   
    
    public GameObject trigger;
    Coroutine Coroutine;
    public bool isSitUp = false;
    public bool isQuest2Complete = false;
    public GameObject canvasNameNPC;
    
    private void Start()
    {
        canvasNameNPC.SetActive(false); // Ẩn canvas tên NPC ban đầu
        isContent = true;
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
       
    }

    void Update()
    {
        enemyMap2_1 = FindFirstObjectByType<EnemyMap2_1>();
        if (killEnemy >= 6 && !isSitUp)
        {
            if(isSitUp == false)
            {
                animator.SetTrigger("SitUp");
                isSitUp = true;
            }
            Destroy(trigger);
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
        if (Coroutine != null)
        {
            StopCoroutine(Coroutine);
            
        }

    }
   
}

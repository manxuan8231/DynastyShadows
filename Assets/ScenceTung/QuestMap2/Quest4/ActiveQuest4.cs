using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActiveQuest4 : MonoBehaviour
{
    [Header("Canvas")]
    public GameObject questionGameCanvas;
    public TMP_Text nameTxt;
    public TMP_Text contentText;
    public GameObject btnF;
    public GameObject PanelContent;
    public string[] contentTextQuest;
    public string[] nameTextQuest;
    Coroutine Coroutine;
    [Header("Bool")]
    bool isOpen;
    bool isTyping;
    bool isSkip;
    bool isWriteSkip;
    public bool isContent;
    public bool isActiveBtn = false;
    public bool hasFinishedDialogue = false; // THÊM BIẾN NÀY
    bool hasPlayedTalkingAnim = false;
    public bool isActiveQuest4 = false;

    [Header("Trạng thái nhiệm vụ")]
    public GameObject stateCanvas;
    public TMP_Text stateText;
    public TMP_Text missionName;
    public Image iconState;
    public Sprite spirteState;

    [Header("Tham chiếu")]
    public AudioCanvasState audioCanvasState;
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isOpen && !isActiveBtn)
        {
            questionGameCanvas.SetActive(true);
            btnF.SetActive(true);

            if (Input.GetKeyDown(KeyCode.F) && isContent)
            {
                if (!hasPlayedTalkingAnim)
                {
                    animator.SetTrigger("Talk");
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
        isActiveQuest4 = true; // Đánh dấu nhiệm vụ 4 đã được kích hoạt
        animator.SetTrigger("Idle"); // Chuyển về trạng thái Idle sau khi kết thúc hội thoại
        if (Coroutine != null)
        {
            StopCoroutine(Coroutine);

        }



    }
   
}

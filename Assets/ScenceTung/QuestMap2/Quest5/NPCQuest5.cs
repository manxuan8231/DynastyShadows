using System.Collections;
using TMPro;
using UnityEngine;

public class NPCQuest5 : MonoBehaviour
{
    [Header("UI & Quest")]
    public GameObject questionGameCanvas;
    public TMP_Text nameTxt;
    public TMP_Text contentText;
    public GameObject btnF;
    public GameObject PanelContent;
    public string[] contentTextQuest;
    public string[] nameTextQuest;
    private Animator animator;
    public bool isOpen;
    public bool isContent;
    public bool hasFinishedDialogue = false; // THÊM BIẾN NÀY
    public bool hasPlayedTalkingAnim = false;
    public bool isTyping;
    public bool isSkip;
    public bool isWriteSkip;
    public bool isActiveBtn = false;
    public Coroutine Coroutine;
    public ContentQuest5 contentQuest5; // Tham chiếu đến ContentQuest5 để sử dụng hàm SkipBtn
    public GameObject transFormLibriary;




    void Start()
    {
        transFormLibriary.SetActive(false);
        contentQuest5 = FindFirstObjectByType<ContentQuest5>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isOpen && contentQuest5.isQuest5TextOpen == true && !isActiveBtn)
        {
            questionGameCanvas.SetActive(true); 
            btnF.SetActive(true);
            if (Input.GetKeyDown(KeyCode.F) && isContent)
            {
                isActiveBtn = true; // Đánh dấu nút đã được nhấn
                isContent = false ;
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
            questionGameCanvas.SetActive(false);
        }
    }
    private IEnumerator ReadContent()
    {
        btnF.SetActive(false);
        PlayerControllerState playerControllerState = FindFirstObjectByType<PlayerControllerState>();
        playerControllerState.enabled = false; // Vô hiệu hóa điều khiển người chơi
        playerControllerState.animator.enabled = false; // Vô hiệu hóa animator của người chơi
        Cursor.lockState = CursorLockMode.None; // Mở khóa con trỏ chuột
        Cursor.visible = true; // Hiển thị con trỏ chuột
        if (!hasPlayedTalkingAnim)
        {
            animator.SetTrigger("Talk");
            hasPlayedTalkingAnim = true; // Đảm bảo chỉ phát hoạt hình nói một lần

        }
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


        Debug.Log("Skip button clicked!");

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
        animator.ResetTrigger("Talk");
        transFormLibriary.SetActive(true); // Kích hoạt thư viện sau khi kết thúc hội thoại
        animator.SetTrigger("Idle"); // Trở về trạng thái idle sau khi kết thúc hội thoại
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

using UnityEngine;

public class InteractNPC : MonoBehaviour
{
    public bool isInteract = false;
    public bool isArrowTutoSkip = true; // Biến để kiểm soát trạng thái của mũi tên hướng dẫn
    public bool isArrowContent = true; // Biến để kiểm soát trạng thái của mũi tên hướng dẫn nội dung
    public bool isComplete = false; // Biến để kiểm soát trạng thái hoàn thành quest npc
    public GameObject interactNPCPanelArrowTuto;
    public GameObject interactNPCPanelContentTuto; // Mũi tên hướng dẫn nội dung
    //tham chieu
    private Quest1 quest1;
    private PlayerControllerState playerControllerState;
    void Start()
    {
        isInteract = false ;
        interactNPCPanelArrowTuto.SetActive( false);
        interactNPCPanelContentTuto.SetActive(false); // Ẩn mũi tên hướng dẫn nội dung ban đầu
        isArrowContent = true;
        quest1 = FindAnyObjectByType<Quest1>();
        playerControllerState = FindAnyObjectByType<PlayerControllerState>();
        isComplete = false;
    }

   
    void Update()
    {
        if (isInteract && isArrowTutoSkip && Input.GetKeyDown(KeyCode.F))
        {
           
            isArrowTutoSkip = false; // Đặt biến isArrowTutoSkip thành false để không hiển thị mũi tên nữa
            interactNPCPanelArrowTuto.SetActive(true);         
        }
       
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && isInteract == false)
        {
           
            isInteract = true;
          
        }
    }
    public void StartTutorialContent() // Bắt đầu hướng dẫn nội dung
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        playerControllerState.isController = false; // Tắt điều khiển của người chơi khi tương tác với NPC
        playerControllerState.animator.SetBool("isWalking", false);
        playerControllerState.animator.SetBool("isRunning", false);
        playerControllerState.animator.enabled = false; // Tắt animator để tránh các hành động không mong muốn
        interactNPCPanelContentTuto.SetActive(true); // Hiển thị mũi tên hướng dẫn nội dung
    }


    public void CloseBtSkipTuto()//button de thoat huong dan skip
    {
       
        interactNPCPanelArrowTuto.SetActive(false);
    }
    public void CloseBtContenTuto()//button de thoat huong dan conten
    {
        interactNPCPanelContentTuto.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerControllerState.isController = true;
        playerControllerState.animator.enabled = true;
        isComplete = true; // Đặt biến isComplete thành true khi hoàn thành hướng dẫn nội dung
    }

}

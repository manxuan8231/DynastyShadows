using System.Collections;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class TimeLineDesert : MonoBehaviour
{
    public CinemachineCamera scene1;//camera 1
    public CinemachineCamera scene2;//camera 2
  
    public GameObject effectDesert;//hieu ung bao cat

    public GameObject panelContent;
    public TextMeshProUGUI textContent;

    private bool isScene1Active = false;
    private bool isPlayer = true;
    public bool isScene2Active = false;

   
    //goi ham
    private QuestDesert5 questDesert; // Quest Desert 
    private PlayerController characterController;
    void Start()
    {
        characterController = FindAnyObjectByType<PlayerController>();
        questDesert = FindAnyObjectByType<QuestDesert5>(); // Lấy đối tượng QuestDesert5
        effectDesert.SetActive(false); // Ẩn hiệu ứng sa mạc ban đầu
        panelContent.SetActive(false); // Ẩn panel nội dung ban đầu
        textContent.enabled = false; // Ẩn nội dung văn bản ban đầu
        scene1.Priority = 0; // Đặt độ ưu tiên của camera scene1 là 0
    }

    void Update()
    {
        if (isScene1Active == true)
        {
            isScene1Active = false; // Đặt cờ để hủy kích hoạt scene1
           
            StartCoroutine(WaitScene1());
        }
        else if(isScene2Active == true)
        {
            isScene2Active = false; // Đặt cờ để hủy kích hoạt scene2
            StartCoroutine(WaitScene2());
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && isPlayer)
        {
            isScene1Active = true; // Đặt cờ để kích hoạt scene1
            isPlayer = false;
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isScene1Active = false; // Đặt cờ để hủy kích hoạt scene1
           
        }
    }

    public IEnumerator WaitScene1()
    {
        // Vô hiệu hóa các chức năng của nhân vật
        characterController.enabled = false; // Vô hiệu hóa CharacterController
        characterController.animator.SetBool("isWalking", false);
        characterController.animator.SetBool("isRunning", false);
       
        // Hiện chuột
        UnityEngine.Cursor.visible = true;
        UnityEngine.Cursor.lockState = CursorLockMode.None;

        scene1.Priority = 20;
        // Hiện hiệu ứng sa mạc
        effectDesert.SetActive(true);
        yield return new WaitForSeconds(2f);//---------------------
        // Hiện nội dung văn bản
        panelContent.SetActive(true); // Hiện panel nội dung
        textContent.enabled = true;
        textContent.text = "Hình như mình cảm thấy một khi tức kì lạ từ con đường này?";
        // Chờ 3 giây
        yield return new WaitForSeconds(3f);//---------------------
        textContent.text = "Trong giống như sự hận thù?";
        // Chờ 2 giây
        yield return new WaitForSeconds(3f);//---------------------
        textContent.text = "Oán Hận?";
        yield return new WaitForSeconds(2f);//---------------------
        textContent.text = "Nó mạnh đến mức mình có thể cảm nhận được một cách mạnh mẽ.";
        yield return new WaitForSeconds(3f);//---------------------
        textContent.text = "Mình nghĩ mình sẽ đi kiểm tra nó.";
        yield return new WaitForSeconds(2.5f);//---------------------
        Destroy(effectDesert,40f);
        characterController.enabled = true; // bat CharacterController
        questDesert.StartQuestDesert5();

        // tat chuot
        UnityEngine.Cursor.visible = false;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        // Ẩn nội dung văn bản
        panelContent.SetActive(false); // Ẩn panel nội dung
        textContent.enabled = false;
        // Đặt độ ưu tiên của camera scene1 về 0
        scene1.Priority = 0;
    
    }

    public IEnumerator WaitScene2()
    {
        // Hiện camera scene2
        scene2.Priority = 20; // Đặt độ ưu tiên của camera scene2 là 1
        Cursor.lockState = CursorLockMode.None; // Cho phép di chuyển chuột tự do
        Cursor.visible = true; //hien chuột
        characterController.enabled = false; // Vô hiệu hóa CharacterController
        characterController.animator.SetBool("isWalking", false);
        characterController.animator.SetBool("isRunning", false);
        panelContent.SetActive(true); // Hiện panel nội dung
        textContent.enabled = true; // Hiện nội dung văn bản
        textContent.text = "Không ngờ ở đây lại có các dấu vết còn sót lại của con người.";

        yield return new WaitForSeconds(3f); // Chờ trong 3 giây
        characterController.enabled = true; // Bật CharacterController
        Cursor.lockState = CursorLockMode.Locked; // Khóa chuột lại
        Cursor.visible = false; //an chuot
        scene2.Priority = 0;
        textContent.text = "...?";//
        questDesert.enemy.SetActive(true); // Kích hoạt kẻ thù---------------------------------
        questDesert.questNameText.text = $"Tiêu diệt hết kẻ thù bao vây"; // Hiển thị tên nhiệm vụ
        RenderSettings.fogDensity = 0.01f; //tăng độ mờ của sương mù

        yield return new WaitForSeconds(2f); // Chờ trong 3 giây
        textContent.text = "Không ổn rồi.";
       
        yield return new WaitForSeconds(2f); // Chờ trong 3 giây
        textContent.text = "Bị bao vay rồi.";
       

        yield return new WaitForSeconds(2f); // Chờ trong 3 giây
        panelContent.SetActive(false); // Ẩn panel nội dung
        textContent.enabled = false; // Ẩn nội dung văn bản


    }
}


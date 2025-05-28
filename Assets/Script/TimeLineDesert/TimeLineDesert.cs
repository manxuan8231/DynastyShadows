using System.Collections;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;

public class TimeLineDesert : MonoBehaviour
{
    public CinemachineCamera scene1;
    public GameObject effectDesert;

    public TextMeshProUGUI textContent;

    private bool isScene1Active = false;
    private bool isPlayer = true;

   
    public PlayerController characterController;
    void Start()
    {
        characterController = FindAnyObjectByType<PlayerController>();
      
        effectDesert.SetActive(false); // Ẩn hiệu ứng sa mạc ban đầu
        textContent.enabled = false; // Ẩn nội dung văn bản ban đầu
        scene1.Priority = 0; // Đặt độ ưu tiên của camera scene1 là 0
    }

    void Update()
    {
        if (isScene1Active == true)
        {
            isScene1Active = false; // Đặt cờ để hủy kích hoạt scene1
            StartCoroutine(WaitTimeline());
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

    public IEnumerator WaitTimeline()
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
        // tat chuot
        UnityEngine.Cursor.visible = false;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        // Ẩn nội dung văn bản
        textContent.enabled = false;
        // Đặt độ ưu tiên của camera scene1 về 0
        scene1.Priority = 0;
    
    }
}


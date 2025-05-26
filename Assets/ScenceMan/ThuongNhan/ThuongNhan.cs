using System.Collections;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;

public class ThuongNhan : MonoBehaviour
{
    public float waitTimeScene = 2f; 
    public float distance = 20f;// Khoảng cách tối đa để tương tác với NPC
    private bool isSceneActive = true; // Biến để kiểm tra xem scene đã được kích hoạt hay chưa
   
    public Transform playerTransform;// Tham chiếu đến Transform của người chơi
    public TextMeshProUGUI textHelp;
    public CinemachineCamera scene1;//
    public Animator animator;
    public AudioSource audioSource;
    public float enemy = 0;

    // Tham chiếu
    TurnInQuestThuongNhan turnInQuestThuongNhan; // Tham chiếu đến TurnInQuestThuongNhan
    void Start()
    {
        scene1.Priority = 0;
        playerTransform = GameObject.FindWithTag("Player").transform; // Lấy Transform của người chơi
        textHelp.gameObject.SetActive(false);// Hiển thị text
        turnInQuestThuongNhan =FindAnyObjectByType<TurnInQuestThuongNhan>(); // Lấy tham chiếu đến TurnInQuestThuongNhan
        animator = GetComponent<Animator>(); // Lấy tham chiếu đến Animator của NPC
        audioSource = GetComponent<AudioSource>(); // Lấy tham chiếu đến AudioSource của NPC
    }

   
    void Update()
    {
        float dis = Vector3.Distance(transform.position, playerTransform.position);// Tính khoảng cách giữa NPC và người chơi
        if(dis <= distance && isSceneActive == true)
        {
            isSceneActive = false;
            StartCoroutine(WaitScene(waitTimeScene));// Nếu khoảng cách nhỏ hơn hoặc bằng distance, bắt đầu coroutine
        }
    }

    public void UpdateKillEnemy(float amount)
    {
        enemy += amount; // Cập nhật số lượng kẻ thù đã tiêu diệt
        if (enemy >= 3) // Kiểm tra nếu số lượng kẻ thù đã tiêu diệt đạt yêu cầu
        {
            audioSource.enabled = false;
            turnInQuestThuongNhan.isContent = true; // Đặt trạng thái hội thoại là true
            turnInQuestThuongNhan.isButtonF = true; // Hiển thị nút F để tương tác với NPC
            animator.SetBool("Idle", true); 
        }
    }
    private IEnumerator WaitScene(float amount)
    {
        scene1.Priority = 10;// Đặt priority của camera hiện tại
        yield return new WaitForSeconds(2f);// Chờ 0.5 giây để camera chuyển đổi
        textHelp.gameObject.SetActive(true);// Hiển thị text
        textHelp.text = "Cứu!";
        yield return new WaitForSeconds(1f);// Chờ 1 giây để người chơi có thời gian đọc
        textHelp.text = "Ai đó cứu tôi với!";
        
        yield return new WaitForSeconds(amount);// Chờ 2 giây
        scene1.Priority = 0;// Trả về priority ban đầu
        textHelp.gameObject.SetActive(false);// Hiển thị text
    }
}

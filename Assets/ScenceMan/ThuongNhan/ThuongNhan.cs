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
    
    void Start()
    {
        scene1.Priority = 0;
        playerTransform = GameObject.FindWithTag("Player").transform; // Lấy Transform của người chơi
        
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
    private IEnumerator WaitScene(float amount)
    {
        textHelp.text = "Cứu";
        textHelp.gameObject.SetActive(true);// Hiển thị text
        yield return new WaitForSeconds(1f);// Chờ 1 giây để người chơi có thời gian đọc
        textHelp.text = "Ai đó cứu tôi với";
        yield return new WaitForSeconds(1f);// Chờ 1 giây để người chơi có thời gian đọc
        scene1.Priority = 10;// Đặt priority của camera hiện tại
        yield return new WaitForSeconds(amount);// Chờ 2 giây
        scene1.Priority = 0;// Trả về priority ban đầu
    }
}

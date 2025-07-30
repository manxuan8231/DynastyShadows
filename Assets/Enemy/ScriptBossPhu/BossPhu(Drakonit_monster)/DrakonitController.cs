
using TMPro;
using Unity.Cinemachine;

using UnityEngine;
using UnityEngine.AI;


// Script điều khiển hành vi của enemy Drakonit bằng State Machine
public class DrakonitController : MonoBehaviour
{
    // Các biến điều chỉnh trong Inspector
    public float chaseRange = 100f;   //khoan cach thay player
    public float attackRange = 2f;   // Khoảng cách tấn công
    public float skillRange = 20;    // Khoảng cách sử dụng kỹ năng
    public int damage = 50;
    public bool isSkill = false; // Kiểm tra trạng skill
    public bool isAttack = false;
    public bool isRunning = false; // Kiểm tra trạng thái chạy
    public bool isWalking = true; // Kiểm tra trạng thái đi bộ khi ở gần player
    public bool isWalkLeft = false;//dng để cập nhập vị trí liên tục
    public bool isWalkRight = false;//dng để cập nhập vị trí liên tục
    public bool isCamera = false; // Kiểm tra trạng thái camera
    //EffectSkill
    public GameObject skillEffect1; // Hiệu ứng kỹ năng 1
   
    public GameObject skillEffect3; // Hiệu ứng kỹ năng 3
    public GameObject auraSkill1;
    public GameObject auraSkill2;

    //vi tri spawn effect
    public Transform effectSpawnPointSkill1; // Vị trí spawn hiệu ứng 1
  
    public Transform effectSpawnPointSkill3; // Vị trí spawn hiệu ứng 3

    //effect Attacl
    public GameObject effectHandR; 
    public GameObject effectHandL; 

  
    //getcomponent
    public Transform player;         
    public Animator animator;        
    public NavMeshAgent agent;       
    public Rigidbody rb; // Rigidbody của enemy

    //camera cutscenboss
    public CinemachineCamera cutScene1;
    public CinemachineCamera cutScene2;
    public CinemachineCamera cutScene3;

    //vùng chặn lại khi thấy pllayer 
    public GameObject blockZone; 
   
    //text
    public TextMeshProUGUI textConten; // Text hoi thoai
    public GameObject imgBietDanh; // GameObject chứa text
    // Biến tham chiếu đến DrakonitDameZone
    public DrakonitDameZone dameZone;
    private DrakonitController enemy; // Biến tham chiếu đến DrakonitController
    public QuestMainBacLam questMainBacLam;
    public DrakonitHp1 drakonitHp1; // Biến tham chiếu đến DrakonitHp
    // Trạng thái hiện tại
    private DrakonitState currentState; 
    void Start()
    {
        // Lấy component DrakonitDameZone từ enemy
        dameZone = FindAnyObjectByType<DrakonitDameZone>();
        enemy = FindAnyObjectByType<DrakonitController>();
        questMainBacLam = FindAnyObjectByType<QuestMainBacLam>();
        animator = GetComponent<Animator>();
        player = FindClosestPlayer(); 
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        drakonitHp1 = GetComponent<DrakonitHp1>();
        //effect skill
        auraSkill1.SetActive(false); // Tắt hiệu ứng kỹ năng 1
        auraSkill2.SetActive(false); // Tắt hiệu ứng kỹ năng 2
        effectHandR.SetActive(false); // Tắt hiệu ứng tay phải
        effectHandL.SetActive(false); // Tắt hiệu ứng tay trái
        //vùng chặn lại
        blockZone.SetActive(false);
        //text
        textConten.enabled = false; // ẩn text hoi thoai
        imgBietDanh.SetActive(false); // ẩn text biêt danh
        isCamera = false; // Bắt đầu không ở trạng thái camera
        // Bắt đầu với trạng thái camera
        ChangeState(new DrakonitCameraState(this));
    }

    void Update()
    {
        player = FindClosestPlayer(); // Tìm player gần nhất
        // Gọi hàm Updat của trạng thái hiện tại 
        currentState?.Update();
       
    }

    // Hàm chuyển trạng thái
    public void ChangeState(DrakonitState newState)
    {
        currentState?.Exit();     // Thoát trạng thái cũ 
        currentState = newState;  // Gán trạng thái mới
        currentState.Enter();     // Kích hoạt trạng thái mới
    }

   
    


    //bắt sự kiện event từ animator skill
    public void SpawnEffectSkill1()
    {
        Quaternion rotation = Quaternion.LookRotation(transform.forward);
        GameObject effect = Instantiate(skillEffect1, effectSpawnPointSkill1.position, rotation);
        Destroy(effect, 3f); // Hủy hiệu ứng sau 2 giây
    }
    public void SpawnEffectSkill3()
    {
        transform.LookAt(player); // Xoay về phía player
        Quaternion rotation = Quaternion.LookRotation(transform.forward);
        GameObject effect = Instantiate(skillEffect3, effectSpawnPointSkill3.position, rotation);

        // Lấy Rigidbody từ chính object mới tạo ra
        Rigidbody effectRb = effect.GetComponent<Rigidbody>();
        if (effectRb != null)
        {
            effectRb.AddForce(transform.forward * 30f, ForceMode.Impulse); // Bắn ra phía trước
        }

        Destroy(effect, 4f);
    }

    // Hàm này được gọi từ animator khi tấn công
    public void beginDame()
    {
        dameZone.beginDame();
    }
    public void endDame()
    {
        dameZone.endDame();
    }
    //tim play gan nhat
    Transform FindClosestPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
    Transform closest = null;
    float minDistance = Mathf.Infinity;

        foreach (GameObject go in players)
        {
            float dist = Vector3.Distance(transform.position, go.transform.position);
            if (dist<minDistance)
            {
                minDistance = dist;
                closest = go.transform;
            }
        }

        return closest;
    }
}

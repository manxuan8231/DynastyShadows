using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.AI;

// Script điều khiển hành vi của enemy Drakonit bằng State Machine
public class DrakonitController : MonoBehaviour
{
    // Các biến điều chỉnh trong Inspector
    public float chaseRange = 10f;   //khoan cach thay player
    public float attackRange = 2f;   // Khoảng cách tấn công
    public float skillRange = 50f;    // Khoảng cách sử dụng kỹ năng
    public int damage = 50;

    //EffectSkill
    public GameObject skillEffect1; // Hiệu ứng kỹ năng 1
    public GameObject skillEffect2; // Hiệu ứng kỹ năng 2
    public GameObject auraSkill1;
    public GameObject auraSkill2;
    //vi tri spawn effect
    public Transform effectSpawnPointSkill1; // Vị trí spawn hiệu ứng 1
    public Transform effectSpawnPointSkill2; // Vị trí spawn hiệu ứng 2

    //getcomponent
    public Transform player;         
    public Animator animator;        
    public NavMeshAgent agent;       

    //camera cutscenboss
    public CinemachineCamera cutScene1;
    public CinemachineCamera cutScene2;
    public CinemachineCamera cutScene3;

    private DrakonitState currentState; // Trạng thái hiện tại
    void Start()
    {
       
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform; 
        agent = GetComponent<NavMeshAgent>();
        auraSkill1.SetActive(false); // Tắt hiệu ứng kỹ năng 1
        auraSkill2.SetActive(false); // Tắt hiệu ứng kỹ năng 2
        // Bắt đầu với trạng thái camera
        ChangeState(new DrakonitCameraState(this));
    }

    void Update()
    {
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


    //bắt sự kiện event từ animator
    public void SpawnEffectSkill1()
    {
        Quaternion rotation = Quaternion.LookRotation(transform.forward);
        GameObject effect = Instantiate(skillEffect1, effectSpawnPointSkill1.position, rotation);
        Destroy(effect, 3f); // Hủy hiệu ứng sau 2 giây
    }
    public void SpawnEffectSkill2()
    {
        Quaternion rotation = Quaternion.LookRotation(transform.forward); 
        GameObject effect = Instantiate(skillEffect2, effectSpawnPointSkill2.position, rotation);
        Destroy(effect, 3f); // Hủy hiệu ứng sau 2 giây
    }
}

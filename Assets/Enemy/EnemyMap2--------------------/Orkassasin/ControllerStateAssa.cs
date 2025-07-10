using Pathfinding;
using System.IO;
using UnityEngine;

public class ControllerStateAssa : MonoBehaviour
{
    // Trạng thái hiện tại
    private AssasinState currentState;
    //tim pl va thong so 
    public GameObject player;
    public float playerRange = 100f;
    public float stopRange = 4f;

    //cooldown attack current   
    public float coolDownAttack = 4f;
    public float lastAttackTime = -4f;
    public float stepAttack = 0f;


    //tham chieu
    public AIPath aiPath;
    public Animator animator;
    private EvenAnimatorAssa evenAnimatorAssa;
    private void Start()
    {
        player = GameObject.FindWithTag("Player");
       
        aiPath = GetComponent<AIPath>();
        animator = GetComponent<Animator>();
        evenAnimatorAssa = FindAnyObjectByType<EvenAnimatorAssa>();
        ChangeState(new CurrentStateAssa(this));
    }
    private void Update()
    {
       
        currentState?.Update();
    }
    public void ChangeState(AssasinState newState)
    {
        currentState?.Exit();     // Thoát trạng thái cũ 
        currentState = newState;  // Gán trạng thái mới
        currentState.Enter();     // Kích hoạt trạng thái mới
    }
    

}

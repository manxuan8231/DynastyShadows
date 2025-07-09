using Pathfinding;
using System.IO;
using UnityEngine;

public class ControllerStateAssa : MonoBehaviour
{
    // Trạng thái hiện tại
    private AssasinState currentState;
    public GameObject player;
    public float playerRange = 100f;
    public float distancePLAndEnemy;

    //tham chieu
    public AIPath aiPath;
    public Animator animator;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        aiPath = GetComponent<AIPath>();
        animator = GetComponent<Animator>();

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
    public void DistancePLAndEnemy()
    {
        distancePLAndEnemy = Vector3.Distance(transform.position, player.transform.position);
    }
}

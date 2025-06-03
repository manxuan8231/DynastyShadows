using NUnit.Framework.Constraints;
using System.Collections;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Boss1Controller : MonoBehaviour
{
    public Boss1State currentState;
    public Animator anmt;
    public NavMeshAgent agent;
    public Transform player;
    public float attackTimer = -9f;
    public float attackCooldown = 9;
    public bool isAttacking = false;
    public float radius = 100f;
    public float attackRange = 5f;
    public float skillRange = 15f;
   public float skillTimer = 0;
   public float skillCooldown = 25;
    public bool isUsingSkill = false;
    public GameObject skill1Prefabs;
    public GameObject skill2Prefabs;
    public Transform skill2Pos;

    public Boss1HP hp;
    private void Awake()
    {
        hp = FindAnyObjectByType<Boss1HP>();
        agent = GetComponent<NavMeshAgent>();
        anmt = GetComponent<Animator>();

        ChangState(new Boss1IdleState(this));
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        skill1Prefabs.SetActive(false);
      
    }
    void Start()
    {
       

    }

    // Update is called once per frame
    void Update()   
    {
        currentState?.Update();
        

    }
    public void ChangState(Boss1State newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }
  

  public void Skill2()
    {  
      
        transform.LookAt(player.position);
        GameObject bullet = Instantiate(skill2Prefabs, skill2Pos.position, Quaternion.identity);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * 10f , ForceMode.Impulse);
        Destroy(skill2Prefabs, 6f);
        isUsingSkill = false;
    }
}

using NUnit.Framework.Constraints;
using System.Collections;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;

public class Boss1Controller : MonoBehaviour
{
    public Boss1State currentState;
    public Animator anmt;
    public NavMeshAgent agent;
    public Transform player;
    public float radius = 100f;
    public float attackRange = 5f;
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
  

  
}

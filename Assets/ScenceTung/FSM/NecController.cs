using System;
using UnityEngine;
using UnityEngine.AI;

public class NecController : MonoBehaviour
{
    public INecState currentState;
    public Animator anmt;
    public NavMeshAgent agent;
    public Transform player;
    public float radius = 35f;
     void Start()
    {
        anmt = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        ChangState(new NecIdleState(this));
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }


     void Update()
    {
        currentState?.Update();
    }


    public void ChangState(INecState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }
    
    

}

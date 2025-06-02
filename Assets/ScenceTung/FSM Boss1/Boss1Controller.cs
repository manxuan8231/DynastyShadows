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
 
    void Start()
    {
     
        ChangState(new Boss1IdleState(this));

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

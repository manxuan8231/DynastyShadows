using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IdleState : IState
{
    public void Enter()
    {
        Debug.Log("Idle Enter State");
    }

    public void Execute()
    {
        Debug.Log("Idle Execute State");
    }
    
    public void Exit()
    {
        Debug.Log("Idle Exit State");
    }
}

public class MovingState : IState
{
    public void Enter()
    {
        Debug.Log("Moving Enter State");
    }

    public void Execute()
    {
        Debug.Log("Moving Execute State");
    }
    
    public void Exit()
    {
        Debug.Log("Moving Exit State");
    }
}

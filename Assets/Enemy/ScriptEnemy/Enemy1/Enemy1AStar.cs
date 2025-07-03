using Pathfinding;
using System.Collections;
using UnityEngine;

public class Enemy1AStar : MonoBehaviour
{
    public Transform target; // thường là Player
    private AIPath aiPath;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform; // Tìm đối tượng Player trong scene
        aiPath = GetComponent<AIPath>();
        aiPath.canSearch = true;
        aiPath.canMove = true;
        aiPath.maxSpeed = 3.5f; // tốc độ chạy
    }

    void Update()
    {
        if (target != null)
        {
            aiPath.destination = target.position;
        }
    }

}

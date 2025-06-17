using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathFinding : MonoBehaviour
{
    public PathFinding cameFrom;
    public List<PathFinding> connections;

    public float gScore;
    public float hScore;
    
    public float Fscore()
    {
        return gScore + hScore;
    }
}

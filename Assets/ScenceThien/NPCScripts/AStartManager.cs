using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class AStartManager : MonoBehaviour
{
    public static AStartManager instance;
    
    public void Awake()
    {
        instance = this;
    }

    public List<PathFinding> GeneratePath(PathFinding start, PathFinding end)
    {
        List<PathFinding> openSet = new List<PathFinding>();
        foreach(PathFinding n in FindObjectsByType<PathFinding>(FindObjectsSortMode.None))
        {
            n.gScore = float.MaxValue;
        }
        start.gScore = 0;
        start.hScore = Vector3.Distance(start.transform.position, end.transform.position);
        openSet.Add(start);

        while (openSet.Count > 0) 
        {
            int lowestF = default;

            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].Fscore() < openSet[lowestF].Fscore())
                {
                    lowestF = i;
                }
            }
            PathFinding currentPath = openSet[lowestF];
            openSet.RemoveAt(lowestF);

            if (currentPath == end)
            {
                List<PathFinding> path = new List<PathFinding>();
                path.Insert(0, end);

                while (currentPath != start) 
                {
                    currentPath = currentPath.cameFrom;
                    path.Add(currentPath);
                }

                path.Reverse();
                return path;
            }
            foreach(PathFinding connectPath in currentPath.connections)
            {
                float heldScore = connectPath.gScore + Vector3.Distance(currentPath.transform.position, connectPath.transform.position);

                if (heldScore < connectPath.gScore)
                {
                    connectPath.cameFrom = currentPath;
                    connectPath.gScore = heldScore;
                    connectPath.hScore = Vector3.Distance(connectPath.transform.position, end.transform.position);

                    if (!openSet.Contains(connectPath))
                    {
                        openSet.Add(connectPath);
                    }
                }
            }
        }
        return null;
    }
}

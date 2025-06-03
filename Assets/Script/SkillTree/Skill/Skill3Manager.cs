using System.Collections;
using UnityEngine;

public class Skill3Manager : MonoBehaviour
{
    public Transform[] spawnClonePL;
    public string clonePLTag = "PlayerClone"; // Tag của clone player
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            StartCoroutine(SpawnClone());
        }
    }

    public IEnumerator SpawnClone()
    {
        for (int i = 0; i < Mathf.Min(5, spawnClonePL.Length); i++)
        {
            Vector3 spawnPosition = spawnClonePL[i].position;
            GameObject clone = ObjPoolingManager.Instance.GetEnemyFromPool(clonePLTag, spawnPosition);
            clone.transform.rotation = spawnClonePL[i].rotation;
            Destroy(clone, 5f); 
            yield return new WaitForSeconds(0.3f); 
        }
    }
}

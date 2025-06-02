using System.Collections;
using UnityEngine;

public class Skill3Manager : MonoBehaviour
{
    public GameObject clonePrefab;        // Prefab của phân thân
    public Transform playerTransform;     // Vị trí của người chơi
    public int numberOfClones = 5;        // Số lượng phân thân
    public float delayBetweenClones = 1f; // Thời gian giữa mỗi lần sinh
    public float spawnRadius = 2f;        // Khoảng cách phân thân xuất hiện quanh player

    private bool isSpawning = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha3) && !isSpawning)
        {
            StartCoroutine(SpawnClones());
        }
    }

    IEnumerator SpawnClones()
    {
        isSpawning = true;

        for (int i = 0; i < numberOfClones; i++)
        {
            Vector3 spawnOffset = Random.onUnitSphere;
            spawnOffset.y = 0f; // tránh sinh phân thân trên đầu
            spawnOffset = spawnOffset.normalized * spawnRadius;

            Vector3 spawnPos = playerTransform.position + spawnOffset;

            GameObject clone = Instantiate(clonePrefab, spawnPos, playerTransform.rotation);

            // Optionally: hủy clone sau vài giây (VD: 5s)
            Destroy(clone, 5f);

            yield return new WaitForSeconds(delayBetweenClones);
        }

        isSpawning = false;
    }
}

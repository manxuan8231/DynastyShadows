using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PoolConfig
{
    public string tag;
    public GameObject prefab;
    public int size;
}

public class ObjPoolingManager : MonoBehaviour
{
    public static ObjPoolingManager Instance;

    public List<PoolConfig> poolConfigs;
    public Transform poolRoot; // Gán OBJPOOL ở đây

    private Dictionary<string, Queue<GameObject>> poolDictionary = new Dictionary<string, Queue<GameObject>>();
    private Dictionary<string, Transform> parentDictionary = new Dictionary<string, Transform>();

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        CreatePools();
    }

    void CreatePools()
    {
        foreach (PoolConfig config in poolConfigs)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            // Tạo parent riêng cho mỗi loại enemy
            GameObject parentObj = new GameObject(config.tag);
            parentObj.transform.SetParent(poolRoot);
            parentDictionary[config.tag] = parentObj.transform;

            for (int i = 0; i < config.size; i++)
            {
                GameObject obj = Instantiate(config.prefab, parentObj.transform);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(config.tag, objectPool);
        }
    }

    public GameObject GetEnemyFromPool(string tag, Vector3 position)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"Không tìm thấy pool cho tag: {tag}");
            return null;
        }

        Queue<GameObject> pool = poolDictionary[tag];

        if (pool.Count > 0)
        {
            GameObject obj = pool.Dequeue();
            obj.transform.SetParent(parentDictionary[tag]); // Gắn vào parent đúng loại
            obj.transform.position = position;
            obj.SetActive(true);
            return obj;
        }
        else
        {
            Debug.LogWarning($"Pool '{tag}' đã hết!");
            return null;
        }
    }

    public void ReturnToPool(string tag, GameObject obj)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"Không thể trả về pool, tag không tồn tại: {tag}");
            return;
        }
        
        obj.SetActive(false);
        poolDictionary[tag].Enqueue(obj);
    }
}
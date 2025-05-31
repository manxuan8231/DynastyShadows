    using System.Collections.Generic;
    using UnityEngine;

    public class EnemyPoolManager : MonoBehaviour
    {
        public static EnemyPoolManager Instance;

        public GameObject enemyPrefab;

        public int poolSize = 30;

        private Queue<GameObject> enemyPool = new Queue<GameObject>();

        void Awake()
        {
            Instance = this;
            CreatePool();
        }

        void CreatePool()
        {
            for (int i = 0; i < poolSize; i++)
            {
                GameObject obj = Instantiate(enemyPrefab);
                obj.SetActive(false);
                enemyPool.Enqueue(obj);
            }
        }

        public GameObject GetEnemyFromPool(Vector3 position)
        {
            if (enemyPool.Count > 0)
            {
                GameObject obj = enemyPool.Dequeue();
                obj.transform.position = position;
                obj.SetActive(true);
                return obj;
            }
            else
            {
                Debug.LogWarning("Pool hết enemy!");
                return null;
            }
        }

        public void ReturnToPool(GameObject obj)
        {
            obj.SetActive(false);
            enemyPool.Enqueue(obj);
        }
    }

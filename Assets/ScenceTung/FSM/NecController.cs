
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NecController : MonoBehaviour
{
    public INecState currentState;
    public Animator anmt;
    public NavMeshAgent agent;
    public Transform player;
    public float radius = 35f;
    public float attackRange = 4.5f;
    public float checkEnemyCount = 0;
    public NecHp necHp;
    // các biến để xài hàm triệu hồi
    public Transform[] spawnPoints;
    public int enemyCountToSpawn = 10;
    public bool hasSpawned = false;
    public bool isSkill2 = false;
    public GameObject skill2;
    public Vector3 playerPos;
   
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
        necHp = FindAnyObjectByType<NecHp>();
        skill2.SetActive(false);
       
        
    }


     void Update()
    {
        playerPos = transform.position;
        currentState?.Update();
        if (Input.GetKeyDown(KeyCode.T))
        {
            necHp.TakeDame(150);
            Debug.Log("Đã trừ máu");
        }

    }


    public void ChangState(INecState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }


    //spawn enwmy
    public void SpawnEnemiesInstantly()
    { 
    for (int i = 0; i < enemyCountToSpawn; i++)
     {
      Transform randomPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
      EnemyPoolManager.Instance.GetEnemyFromPool(randomPoint.position);
    } 
      
        
    }

    //check số lượng
    public void EnemyCount()
    {
        checkEnemyCount++;
    }

    public void SpawnSKill2()
    {
        Vector3 playerPos = transform.position;
        GameObject obj = Instantiate(skill2, playerPos, Quaternion.identity);
        Destroy(obj,10f);
    }
}

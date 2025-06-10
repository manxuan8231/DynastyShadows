
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
    public bool isSkill1 = false;
    public GameObject skill3;
    public bool isSKill3 = false;

    public Vector3 playerPos;
    public DameZoneWeapon damezoneWP;
    public BoxCollider offDame;
    public NecAudioManager audioManager;
    public QuestDesert5 questDesert5;

    void Start()
    {
        questDesert5 = FindAnyObjectByType<QuestDesert5>();
        audioManager = FindAnyObjectByType<NecAudioManager>();
        damezoneWP = FindAnyObjectByType<DameZoneWeapon>();
        anmt = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        ChangState(new NecIdleState(this));
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }  
        necHp = FindAnyObjectByType<NecHp>();
       
       
        
    }


     void Update()
    {
        if(necHp.curhp <= 0)
        {
            ChangState(new DeathNecState(this));
           
        }
        playerPos = player.transform.position;
        currentState?.Update();
        
       

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
            Vector3 spawnPoint = randomPoint.position;
            ObjPoolingManager.Instance.GetEnemyFromPool("Enemy1", spawnPoint);


        }
    }

    //check số lượng
    public void EnemyCount()
    {
        checkEnemyCount++;
    }

    public void SpawnSKill2()
    {
        if(!isSkill1 && !isSKill3)
        {
            audioManager.audioSource.PlayOneShot(audioManager.skill2Sound);
            Vector3 playerPos = player.transform.position;
            GameObject obj = Instantiate(skill2, playerPos, Quaternion.identity);
            Destroy(obj, 6f);
        }
     
    }

    public void SpawnSkill3()
    {
        if(isSkill1 && isSKill3)
        {
            audioManager.audioSource.PlayOneShot(audioManager.skill3Sound);

            Vector3 playerPos = player.transform.position;
            GameObject obj = Instantiate(skill3, playerPos, Quaternion.identity);
            Destroy(obj, 6f);
        }
    }

    public void BeginDame()
    {
        damezoneWP.beginDame();

    }

    public void EndDame()
    {
        damezoneWP.endDame();
    }
    public void TakeDame(float damage)
    {
        necHp.curhp -= damage;

        necHp.curhp = Mathf.Clamp(necHp.curhp, 0, necHp.maxhp);
        necHp.sliderHp.value = necHp.curhp;
        necHp.textHp.text = $"{necHp.curhp}/{necHp.maxhp}";
        Debug.Log("NecController TakeDame: " + necHp.curhp);
        if(necHp.curhp <= 0)
        {
            questDesert5.UpdateKillBoss(1);
        }
    }
}

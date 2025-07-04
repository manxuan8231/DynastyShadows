// Enemy AI: Đứng yên → Phát hiện Player → Di chuyển đến gần khoảng cách tấn công → Đánh → Lùi lại → Chờ cooldown → Tấn công tiếp

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class MinotaurEnemy : MonoBehaviour, IDamageable
{
    public Transform targetPlayer;//người chơi
    public Vector3 startPosition;
    public CapsuleCollider box;
    public float detectionRange = 10f;//phát hiện người chơi
    public float attackRange = 2f;// khoảng cách tấn công người chơi
    public Slider sliderHp;
    public float currentHealth = 100f;// máu hiện tại 
    public float maxHealth = 100f;// máu tối đa
    public bool isDead = false;// trạng thái chết của kẻ thù

    //hit
    private bool _isHit = false;       // có đang bị đánh không
    private bool _canBeHit = true;     // dùng cooldown giữa các lần bị đánh
   
    private int _hitIndex = 0;         // 0 = Hit1, 1 = Hit2
   //parry
    public Coroutine _parryCoroutine;
    public int _parryCount = 0;       // tích điểm parry
    public int _maxParryCount = 5;       // tích điểm parry
    public AudioClip parrySound;
    //cooldown tấn công
    public float attackCooldown = 4f;// thời gian chờ giữa các đợt tấn công
    public float _lastAttackTime = -4f;// thời gian tấn công cuối cùng
    //spawn exp
    public GameObject[] expPrefab;
    //tham chieu
    public AudioManager audioManager;
    public DameZoneMinotaur damezone; 
    public PlayerStatus playerStatus;
    private MinotaurEnemy minotaurEnemy;
    public SlowMotionDodgeEvent slowMotionDodgeEvent;
    public TutorialManager tutorialManager;
    //cây hành vi
    private Node _rootNode;// Cây hành vi gốc
    public Animator _animator;
    public NavMeshAgent _agent;

    void Start()
    {
         SetupBehaviorTree();// thiết lập cây hành vi
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        audioManager = FindAnyObjectByType<AudioManager>();
        targetPlayer = FindClosestPlayer(); // Tìm người chơi gần nhất
        currentHealth = maxHealth; // Khởi tạo máu hiện tại bằng máu tối đa
        sliderHp.maxValue = currentHealth; // Thiết lập giá trị tối đa của thanh máu
        sliderHp.value = currentHealth; // Thiết lập giá trị hiện tại của thanh máu
        box = GetComponent<CapsuleCollider>();
        damezone = FindAnyObjectByType<DameZoneMinotaur>(); // Lấy tham chiếu đến DrakonitDameZone trong con của MinotaurEnemy
        playerStatus = FindAnyObjectByType<PlayerStatus>();
        minotaurEnemy = GetComponent<MinotaurEnemy>();
        slowMotionDodgeEvent = FindAnyObjectByType<SlowMotionDodgeEvent>();
        tutorialManager = FindAnyObjectByType<TutorialManager>();
        startPosition = transform.position; // Lưu vị trí bắt đầu của MinotaurEnemy
    }

    void Update()
    {
        if (isDead) return; // Nếu đã chết, không thực hiện hành động nào khác    
        targetPlayer = FindClosestPlayer(); // Cập nhật người chơi gần nhất mỗi khung hình
        _rootNode?.Evaluate();
    }
   
    private void SetupBehaviorTree()
    {
        //xu ly die-------------------
        var isDead = new ConditionNode(() => currentHealth <= 0);// kiểm tra xem  đã chết hay chưa
        var deathAction = new ActionNode(PlayDeathAnimation);// hành động khi  chết

        //xu ly hit------------------------
         var isHit = new ConditionNode(IsHitEnemy);// kiểm tra xem có bị đánh hay không
         var hitAction = new ActionNode(() =>
        {
            _agent.isStopped = true;

            if (_hitIndex == 0)
            {
                _animator.SetTrigger("Hit1");
                _hitIndex = 1;
            }
            else
            {
                _animator.SetTrigger("Hit2");
                _hitIndex = 0;
            }

            _isHit = false; // reset trạng thái đã xử lý hit
            return NodeState.SUCCESS;
        });
       //parry
        var isParry = new ConditionNode(() => _parryCount >= _maxParryCount);
        var parryAction = new ActionNode(() =>
        {
           
            _animator.SetTrigger("parry");
            audioManager.AudioSource.PlayOneShot(parrySound);
           // playerStatus.TakeHealthStun(10);
             _parryCount = 0;
            return NodeState.SUCCESS;
        });

        // xu ly di chuyển đến người chơi -----------------
        var isMove = new ConditionNode(ISPlayerMove);  
        var moveToPlayerAction = new ActionNode(MoveToPlayer);
        // quay về vị trí ban đầu nếu người chơi ra khỏi tầm nhìn
        var lostSight = new ConditionNode(PlayerOutOfSight);
        var returnToStart = new ActionNode(ReturnToStartPosition);

        //xu ly tấn công người chơi---------------------
        var isPlayerInAttackRange = new ConditionNode(IsPlayerInAttackRange);// kiểm tra xem người chơi có trong khoảng tấn công hay không
        var isCooldownAttack = new ConditionNode(() => Time.time >= _lastAttackTime + attackCooldown);// kiểm tra cooldown tấn công                                                                               
        var attackAction = new ActionNode(AttackPlayer);// hành động tấn công người chơi    
        var waitAction = new ActionNode(WaitForCooldown); // Viết hàm đứng yên chờ cooldown

        // hành vi idle------------------
        var idleAction = new ActionNode(Idle); // hành vi idle

        // Cây hành vi gốc
        _rootNode = new SelectorNode(new List<Node>
        {
            new SequenceNode(new List<Node> { isDead, deathAction }),
            new SequenceNode(new List<Node> { isHit, hitAction }),
            new SequenceNode(new List<Node> { isParry, parryAction }),
            new SequenceNode(new List<Node> { isMove, moveToPlayerAction }),
            new SequenceNode(new List<Node> { isPlayerInAttackRange, isCooldownAttack, attackAction, waitAction }),
            new SequenceNode(new List<Node> { lostSight, returnToStart }),
            idleAction
        });

    }
    //move-----------------
    private bool ISPlayerMove()// phát hiện người chơi
    {
        return targetPlayer != null && Vector3.Distance(transform.position, targetPlayer.position) <= detectionRange &&
                                       Vector3.Distance(transform.position, targetPlayer.position) >=  attackRange;
    }

    private NodeState MoveToPlayer()
    {
        if (targetPlayer == null) return NodeState.FAILURE;
        targetPlayer = FindClosestPlayer(); // Tìm người chơi gần nhất
        _agent.isStopped = false;
        _agent.SetDestination(targetPlayer.position);
        _animator.SetBool("IsRunning", true);
        return NodeState.RUNNING;
    }

    //quay ve vị trí ban đầu
    private bool PlayerOutOfSight()
    {
        if (targetPlayer == null) return true;

        float distance = Vector3.Distance(transform.position, targetPlayer.position);

        // Nếu player ra khỏi phạm vi detection → quay về liền
        return distance > detectionRange;
    }
    private NodeState ReturnToStartPosition()
    {
        _agent.isStopped = false;
        _animator.SetBool("IsRunning", true);
        _agent.SetDestination(startPosition);

        float distToStart = Vector3.Distance(transform.position, startPosition);

        if (distToStart <= 3f)
        {
            _animator.SetBool("IsRunning", false);
            return NodeState.SUCCESS;
        }

        return NodeState.RUNNING;
    }

    //attack----------
    private bool IsPlayerInAttackRange()// khoảng cách tấn công người chơi
    {
        return targetPlayer != null && Vector3.Distance(transform.position,
                                                         targetPlayer.position) <= attackRange;
    }
   
    private NodeState AttackPlayer()
    {
        if (targetPlayer == null) return NodeState.FAILURE;
        _animator.SetBool("IsRunning", false); // tắt animation chạy
        _agent.isStopped = true;
        transform.LookAt(targetPlayer);
        if (currentHealth > 50) {
            _animator.SetTrigger("Attack");
        }
        else if(currentHealth <= 50 && currentHealth > 0) {
            _animator.SetTrigger("Attack2");
        }
            Debug.Log(" tấn công");
        _lastAttackTime = Time.time;// cập nhật thời gian tấn công cuối cùng
        return NodeState.SUCCESS;// tấn công thành công
    }


    private NodeState WaitForCooldown()
    {
        _agent.isStopped = true;
        _animator.SetBool("IsRunning", false);
        return NodeState.RUNNING;
    }
    
    private NodeState Idle()
    {
        _agent.isStopped = true;
        _animator.SetBool("IsRunning", false); // tắt animation chạy
                                                // Không set trigger attack hay die, vì idle là trạng thái mặc định
        return NodeState.SUCCESS;
    }

    Transform FindClosestPlayer()// tìm người chơi gần nhất
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        Transform closestPlayer = null;
        float closestDistance = Mathf.Infinity;
        foreach (GameObject player in players)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPlayer = player.transform;
            }
        }
        return closestPlayer;
    }

    // chết----------------  
    private NodeState PlayDeathAnimation()
    {
        if (isDead == false)
        {
            
            isDead = true; // đánh dấu là đã chết
            _animator.SetTrigger("Die");
            _agent.isStopped = true;
            box.enabled = false; // tắt collider để không va chạm với người chơi
           
            Debug.Log(" đã chết");

        }
        return NodeState.SUCCESS;// đã chết thành công
    }
    public void TakeDamage(float damage)
    {
       
        Debug.Log($"Minotaur nhận {damage} sát thương");
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Đảm bảo máu không âm
        sliderHp.value = currentHealth; // Cập nhật thanh máu     
        _parryCount++;
        Debug.Log($"tích điểm parry: {_parryCount}");
        if(_parryCoroutine != null)
        {
            StopCoroutine(_parryCoroutine);
        }
        _parryCoroutine = StartCoroutine(ResetParryAfterDelay(2f));//reset parry sau 2 giây
        if (currentHealth > 0 )
        {
            _isHit = true;
            _canBeHit = false; // khóa đánh tiếp
            StartCoroutine(HitCooldown()); // cooldown sau khi bị đánh
        }
        else 
        {
            _animator.enabled = true;
             _agent.enabled = true;
            minotaurEnemy.enabled = true;
            //hoan thanh huong dan 
            if (tutorialManager != null) 
            {
                tutorialManager.UpdateEnemyTutorial(1);
            }
            StartCoroutine(WaitAnimatorDie()); // Bắt đầu coroutine để xử lý animation chết
        }
        
        
    }
   

    public IEnumerator WaitAnimatorDie()
    {
       
        yield return new WaitForSeconds(2f); // Thời gian chờ để animation chết hoàn thành
         //spawn exp
        for (int i = 0; i < expPrefab.Length; i++)
        {
            GameObject exp = Instantiate(expPrefab[i], transform.position, Quaternion.identity);
            Destroy(exp, 5f);
        }
        currentHealth = maxHealth; // Đặt lại máu về tối đa
        sliderHp.value = currentHealth; // Cập nhật thanh máu về giá trị tối đa
        isDead = false; // Đặt lại trạng thái chết
        _agent.isStopped = false;
        box.enabled = true; // Bật lại collider để có thể va chạm với người chơi
        yield return null;
        ObjPoolingManager.Instance.ReturnToPool("Minotaur1", gameObject); // Trả về pool sau khi animation chết hoàn thành
    }
    //hit------------------
    private bool IsHitEnemy()
    {
        return _isHit && _canBeHit;
    }
    private IEnumerator HitCooldown()
    {
        yield return new WaitForSeconds(0.1f); // thời gian giữa các lần bị đánh
        _canBeHit = true;
    }
    //pary
    private IEnumerator ResetParryAfterDelay(float delay)
    {
    yield return new WaitForSeconds(delay);
    _parryCount = 0;
    Debug.Log("Reset điểm parry do không bị đánh trong 2 giây");
    }

    //damezone----------------
    public void beginDame()
    {
        Debug.Log("bat box dame");
        damezone.beginDame(); // Bắt đầu vùng sát thương
    }
    public void endDame()
    {
        damezone.endDame(); // Kết thúc vùng sát thương
    }
    public void TriggerDodge()
    {
        if (slowMotionDodgeEvent.isOneSlow == false)
        {
            Debug.Log("da gay dame");
        }
        if (slowMotionDodgeEvent != null && !slowMotionDodgeEvent.isDodgeWindowActive && slowMotionDodgeEvent.isOneSlow == true)
        {
            
            slowMotionDodgeEvent.isDodgeWindowActive = true;
        }
      
    }

}

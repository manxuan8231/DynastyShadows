using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour
{
    public int firstCritChance;
    //xử lý tăng vàng
    [SerializeField]
    public TMP_Text goldQuantityTxt;
    public int gold = 0; // Số vàng hiện tại
    //xử lý stat
    public TMP_Text statHP;
    public TMP_Text statMana;
    public TMP_Text statDame;
    public TMP_Text statCrit;
    public TMP_Text statCritChance;
    //preview item stat----------------------------------------
    [SerializeField]
    private TMP_Text hpPre, manaPre, damePre, critPre, critChancePre;
    [SerializeField]
    private Image previewImage;
    [SerializeField]
    private GameObject selectedItemStats;
    [SerializeField]
    private GameObject selectedItemImage;
    public  TMP_Text itemName;
    //xử lý máu------------------------------------------
    public Slider sliderHp;
    public float currentHp ;
    public float maxHp ;
    public TextMeshProUGUI textHealth;

    //xu lý exp-----------------------------------------------
    public Slider expSlider; 
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI[] scoreText;
    public GameObject effectLevelUp;//effect level up
    public AudioClip audioLevelUp;
    
    public int currentLevel = 1;        // Cấp hiện tại
    public float currentExp = 0f;         // EXP hiện tại
    private float expToNextLevel = 50f;        // EXP yêu cầu để lên cấp tiếp theo
    public float expIncreasePerLevel = 50f;
    //score------------------------------------------
    public int score = 0; // Điểm số hiện tại
    public int scorePerLevel = 2;   // Điểm cộng khi lên cấp 1
    

    //xử lý mana------------------------------------------
    public Slider sliderMana;
    public float currentMana;
    public float maxMana ;
    public TextMeshProUGUI textMana;

    //xu lý dame------------------------------------------
    public int criticalDamage = 100; // +600% damage khi crit
    public int criticalChance = 20;  // 20% tỉ lệ crit
    public int baseDamage = 50; // dame aattack mac dinh
    public TextMeshProUGUI textHitDamage;
    public TextMeshProUGUI textHitChance;
    public TextMeshProUGUI textBaseDamage;

    //xu ly toc do chay------------------------------------------
    public float speedRun = 15f;
    public TextMeshProUGUI textSpeed;
  
    //khoi tao------------------------------------------
   
    private AudioSource audioSource;
   

    //efffect stun------------------------------------------
    public GameObject effectStun;
    public AudioClip audioHit;
    public AudioClip audioStun;
    //tham chieu ------------------------------------------
    private PlayerControllerState playerController; // Tham chiếu đến PlayerController
    private ComboAttack comboAttack; // Tham chiếu đến ComboAttack
    void Start()
    {     
        //khởi tạo hp
        currentHp = maxHp;
        sliderHp.maxValue = currentHp;
        textHealth.text = ((int)currentHp).ToString() + " / " + ((int)maxHp).ToString();

        //level and score
        expSlider.maxValue = expToNextLevel;
        expSlider.value = currentExp;
        levelText.text = currentLevel.ToString();
        for (int i = 0; i < scoreText.Length; i++)
        {
            scoreText[i].text = "" + score.ToString();
        }
        
        //mana
        currentMana = maxMana;
        sliderMana.maxValue = currentMana;
        textMana.text = ((int)currentMana).ToString() + " / " + ((int)maxMana).ToString();
        // 
        textHitDamage.text = $"{criticalDamage}%";
        textHitChance.text = $"{criticalChance}%";
        textBaseDamage.text = $"{baseDamage}";
        firstCritChance = criticalChance;
        //thaam chieu

        audioSource = GetComponent<AudioSource>();
        playerController = FindAnyObjectByType<PlayerControllerState>();
        comboAttack = FindAnyObjectByType<ComboAttack>();
        //tat hieu uung
        effectStun.SetActive(false);
        effectLevelUp.SetActive(false);
        // gold
        goldQuantityTxt.text = gold.ToString();
        UpdateUI(); 


    }
    //update lại toàn bộ
    public void UpdateUI()
    {
        statHP.text = maxHp.ToString();
        statMana.text = maxMana.ToString();
        statDame.text = baseDamage.ToString();
        statCrit.text = criticalDamage.ToString() + "%";
        statCritChance.text = criticalChance.ToString() + "%";

    }
    //preview stat item
    public void PreviewEquipmentItem(int hp, int mana,int dame,int crit,int critChance,Sprite itemImage,string itemName )
    {
        hpPre.text = hp.ToString();
        manaPre.text = mana.ToString();
        damePre.text = dame.ToString();
        critPre.text = crit.ToString() + "%";
        critChancePre.text = critChance.ToString() + "%";

        previewImage.sprite = itemImage;
       this.itemName.text = itemName;
        selectedItemImage.SetActive(true);
        selectedItemStats.SetActive(true);
    }

    public void TurnOffPreviewStats()
    {
        selectedItemImage.SetActive(false);
        selectedItemStats.SetActive(false);
    }
    void Update()
    {      
        RegenerateMana();//hồi mana dần
        UpdateUI(); // Cập nhật UI mỗi frame
       
    }

    //hp
    public void TakeHealth(float amount)//bị lấy hp
    {
        currentHp -= amount;
        currentHp = Mathf.Clamp(currentHp, 0, maxHp);
        sliderHp.value = currentHp;
        textHealth.text = ((int)currentHp).ToString() + " / " + ((int)maxHp).ToString();
       
       
        if (currentHp <= 0)
        {
            Destroy(gameObject);

        }
    }
    public void TakeHealthStun(float amount)//bị lấy hp
    {
        currentHp -= amount;
        currentHp = Mathf.Clamp(currentHp, 0, maxHp);
        sliderHp.value = currentHp;
        textHealth.text = ((int)currentHp).ToString() + " / " + ((int)maxHp).ToString();
        StartCoroutine(WaitStun(4f)); // gọi hàm WaitHit với thời gian 0.5 giây
        audioSource.PlayOneShot(audioHit);
        if (currentHp <= 0)
        {
            Destroy(gameObject);

        }
    }
    public void BuffHealth(float amount)
    {
        currentHp += amount;
        currentHp = Mathf.Clamp(currentHp, 0, maxHp);
        sliderHp.value = currentHp;
        textHealth.text = ((int)currentHp).ToString() + " / " + ((int)maxHp).ToString();

    }
    public void UpMaxHealth(float amount)//tăng giới hạn hp
    {
        maxHp += amount;
        currentHp = Mathf.Clamp(currentHp, 0, maxHp);
        sliderHp.maxValue = maxHp;
        textHealth.text = ((int)currentHp).ToString() + " / " + ((int)maxHp).ToString();

    }

    //mana
    public void TakeMana(float amount)//bị lấy mana
    {
        currentMana -= amount;
        sliderMana.value = currentMana;
        currentMana = Mathf.Clamp(currentMana, 0, maxMana);
        textMana.text = ((int)currentMana).ToString() + " / " + ((int)maxMana).ToString();

    }
    public void BuffMana(float amount)//hồi mana
    {
        currentMana += amount;
        sliderMana.value = currentMana;
        currentMana = Mathf.Clamp(currentMana, 0, maxMana);
        textMana.text = ((int)currentMana).ToString() + " / " + ((int)maxMana).ToString();
    }
    public void UpMaxMana(float amount)//tăng giới hạn hp
    {
        maxMana += amount;
        currentMana = Mathf.Clamp(currentMana, 0, maxMana);
        sliderMana.maxValue = maxMana;
        textMana.text = ((int)currentMana).ToString() + " / " + ((int)maxMana).ToString();

    }

    // Hàm cộng thêm EXP
    public void AddExp(float amount)
    {
        currentExp += amount;

        if (currentExp >= expToNextLevel)
        {
            LevelUp();
        }
        //update UI
        if (expSlider != null)
        {
            expSlider.maxValue = expToNextLevel;
            expSlider.value = currentExp;
        }

        if (levelText != null)
        {
            levelText.text = currentLevel.ToString();
        }

        if (scoreText != null)
        {
            for (int i = 0; i < scoreText.Length; i++)
            {
                scoreText[i].text = "" + score.ToString();
            }
        }
    }
    public void TakeScore(int amount)
    {
        score -= amount;
        score = Mathf.Clamp(score, 0, int.MaxValue);
        for (int i = 0; i < scoreText.Length; i++)
        {
            scoreText[i].text = "" + score.ToString();
        }

    }
    //tăng mana dần
    public void RegenerateMana()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            TakeMana(100 * Time.deltaTime); // trừ dần theo thời gian
        }
        else 
        {
            BuffMana(100 * Time.deltaTime); // cộng dần theo thời gian   
        }
    }

    // --- Nâng chỉ số ---
    public void UpCriticalHitDamage(int amount)
    {
        criticalDamage += amount;
        textHitDamage.text = $"{criticalDamage}%";
    }

    public void UpCriticalHitChance(int amount)
    {
        criticalChance += amount;
        textHitChance.text = $"{criticalChance}%";
    }

    public void UpBaseDamage(int amount)
    {
        baseDamage += amount;
        textBaseDamage.text = $"{baseDamage}";
    }




    // --- Tính damage --------------- và xử lý các logic
    public float CalculateFinalDamage()
    {
        bool isCritical = Random.value < (criticalChance / 100f); //có 20 % cơ hội chí mạng.
        float multiplier = isCritical ? (1f + criticalDamage / 100f) : 1f;//Nếu là đòn chí mạng, nhân sát thương thêm theo phần trăm criticalDamage.
        float damage = baseDamage * multiplier;//Tính sát thương cuối cùng sau khi xét chí mạng.
        
        if (isCritical)
            Debug.Log("💥 Chí mạng! Gây " + damage + " sát thương.");
        else
            Debug.Log("Gây " + damage + " sát thương thường.");

        return damage;
    }

    private IEnumerator WaitStun(float time)
    {
        playerController.animator.SetBool("Stun", true);
        effectStun.SetActive(true);
        playerController.isController = false; // Ngừng điều khiển nhân vật
        comboAttack.isAttack = false; // Ngừng tấn công
        audioSource.PlayOneShot(audioStun); // Phát âm thanh bị stun
        yield return new WaitForSeconds(time);

        comboAttack.isAttack = true; // Bật lại tấn công
        playerController.isController = true; // Bật lại điều khiển nhân vật
        playerController.animator.SetBool("Stun", false);
        effectStun.SetActive(false);
    }


    void LevelUp()
    {
        currentExp = 0f; // Reset EXP về 0
        currentLevel++;
        expToNextLevel += expIncreasePerLevel; // Tăng EXP cần thiết cho cấp sau

        score += scorePerLevel; // Cộng score cố định
        Debug.Log("Level Up! Now level " + currentLevel + " | Score: " + score);
    }
    public void IncreasedGold(int value)
    {
        gold += value;
     goldQuantityTxt.text = value.ToString();

    }

    //hàm buff dame trong vòng 60 giây
    public void BuffDamage(int value, float duration)
    {
        baseDamage += value;
        textBaseDamage.text = $"{baseDamage}";
        StartCoroutine(ResetDamageAfterDuration(value, duration));
    }
    private IEnumerator ResetDamageAfterDuration(int value, float duration)
    {
        yield return new WaitForSeconds(duration);
        baseDamage -= value;
        textBaseDamage.text = $"{baseDamage}";
        Debug.Log("Buff damage đã hết hạn.");
    }

    public IEnumerator WaitLevelUp()
    {
        effectLevelUp.SetActive(true);
        audioSource.PlayOneShot(audioLevelUp);
        yield return new WaitForSeconds(1.5f);
        effectLevelUp.SetActive(false);
    }
}

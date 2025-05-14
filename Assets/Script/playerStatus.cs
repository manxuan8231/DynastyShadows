using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour
{
    //xử lý máu
    public Slider sliderHp;
    public float currentHp ;
    public float maxHp = 2000f;
    public TextMeshProUGUI textHealth;

    //xử lý mana skill
    public Slider sliderManaSkill;
    public float currentManaSkill;
    public float maxManaSkill = 2000f;
    public TextMeshProUGUI textManaSkill;

    //xử lý mana
    public Slider sliderMana;
    public float currentMana;
    public float maxMana = 2000f;
    public TextMeshProUGUI textMana;

    //khoi tao
    private Animator animator;
    private AudioSource audioSource;
    public AudioClip audioHit;

    void Start()
    {
       
        //khởi tạo hp
        currentHp = maxHp;
        sliderHp.maxValue = currentHp;
        textHealth.text = ((int)currentHp).ToString() + " / " + ((int)maxHp).ToString();

        //
        currentManaSkill = maxManaSkill;
        sliderManaSkill.maxValue = currentManaSkill;
        textManaSkill.text = ((int)currentManaSkill).ToString() + " / " + ((int)maxManaSkill).ToString();
        //
        currentMana = maxMana;
        sliderMana.maxValue = currentMana;
        textMana.text = ((int)currentMana).ToString() + " / " + ((int)maxMana).ToString();

        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

       
    }

    
    void Update()
    {
        
        RegenerateMana();//hồi mana dần
    }

    //hp
    public void TakeHealth(float amount)//bị lấy hp
    {
        currentHp -= amount;
        currentHp = Mathf.Clamp(currentHp, 0, maxHp);
        sliderHp.value = currentHp;
        textHealth.text = ((int)currentHp).ToString() + " / " + ((int)maxHp).ToString();

        // animator.SetTrigger("Hit");
        audioSource.PlayOneShot(audioHit);
        if (currentHp <= 0)
        {
            Destroy(gameObject);

        }
    }
    public void AddHealth(float amount)
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

    //manaskill
    public void TakeManaSkill(float amount)//bị lấy manaskill
    {
        currentManaSkill -= amount;
        currentManaSkill = Mathf.Clamp(currentManaSkill, 0, maxManaSkill);
        sliderManaSkill.value = currentManaSkill;
        textManaSkill.text = ((int)currentManaSkill).ToString() + " / " + ((int)maxManaSkill).ToString();

    }
    public void AddManaSkill(float amount)
    {
        currentManaSkill += amount;
        currentManaSkill = Mathf.Clamp(currentManaSkill, 0, maxManaSkill);
        sliderManaSkill.value = currentManaSkill;
        textManaSkill.text = ((int)currentManaSkill).ToString() + " / " + ((int)maxManaSkill).ToString();

    }

    public void UpMaxManaSkill(float amount)//tăng giới hạn hp
    {
        maxManaSkill += amount;
        currentManaSkill = Mathf.Clamp(currentManaSkill, 0, maxManaSkill);
        sliderManaSkill.maxValue = maxManaSkill;
        textManaSkill.text = ((int)currentManaSkill).ToString() + " / " + ((int)maxManaSkill).ToString();

    }

    //mana
    public void TakeMana(float amount)//bị lấy mana
    {
        currentMana -= amount;
        sliderMana.value = currentMana;
        currentMana = Mathf.Clamp(currentMana, 0, maxMana);
        textMana.text = ((int)currentMana).ToString() + " / " + ((int)maxMana).ToString();

    }
    public void AddMana(float amount)//hồi mana
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
        sliderMana.maxValue = maxManaSkill;
        textMana.text = ((int)currentMana).ToString() + " / " + ((int)maxMana).ToString();

    }


    public void RegenerateMana()//tăng mana dần
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            TakeMana(100 * Time.deltaTime); // trừ dần theo thời gian
        }
        else 
        {
            AddMana(100 * Time.deltaTime); // cộng dần theo thời gian   
        }
    }
}

using UnityEngine;

public class ComboAttack : MonoBehaviour
{
    private Animator animator;

    [Header("Thời gian cooldown cho mỗi đòn combo")]
    [SerializeField] private float attack1Cooldown = 0.5f;
    [SerializeField] private float attack2Cooldown = 0.6f;
    [SerializeField] private float attack3Cooldown = 0.7f;

    private int comboStep = 0;
    private float nextAttackTime = 0f;

    public bool isAttack = false;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Chỉ cho phép tấn công nếu đã hết thời gian cooldown
        if (Input.GetMouseButtonDown(0) && Time.time >= nextAttackTime && isAttack)
        {
            OnAttack();
        }

        // Nếu người chơi không nhấn trong 1.2s thì reset combo
        if (Time.time >= nextAttackTime + 1.2f)
        {
            comboStep = 0;
        }
    }

    void OnAttack()
    {
        comboStep++;

        if (comboStep == 1)
        {
            animator.SetTrigger("Attack1");
            nextAttackTime = Time.time + attack1Cooldown;
        }
        else if (comboStep == 2)
        {
            animator.SetTrigger("Attack2");
            nextAttackTime = Time.time + attack2Cooldown;
        }
        else if (comboStep == 3)
        {
            animator.SetTrigger("Attack3");
            nextAttackTime = Time.time + attack3Cooldown;
            comboStep = 0; // Reset combo sau đòn 3
        }
        else
        {
            comboStep = 0;
        }
    }
}

using UnityEngine;
using UnityEngine.AI;

public class BossPhu1HP : MonoBehaviour
{
    public float currentHealth;
    public float maxHealth = 100f;
    
    BossPhu1 bossPhu1;
    private void Start()
    {
        currentHealth = maxHealth;
        bossPhu1 = GetComponent<BossPhu1>();
    }

    public void TakeDame(float Damage)
    {
        currentHealth -= Damage;
        bossPhu1.hasTakenDamage = true;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            TakeDame(10);
        }
    }
}

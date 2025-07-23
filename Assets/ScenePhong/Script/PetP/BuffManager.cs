using UnityEngine;
using System.Collections;

public enum BuffType
{
    Damage,
    Armor,
    Heal,
    Mana   // ✅ Thêm loại buff Mana
}

public class BuffManager : MonoBehaviour
{
    public PlayerStatus playerStats;

    public void BuffDamage()
    {
        Debug.Log("Pet buff dame!");
        playerStats.BuffDamage(50, 5f); // +20 damage trong 5 giây
    }

    public void BuffArmor()
    {
        Debug.Log("Pet buff giáp!");
        playerStats.UpMaxHealth(30f);
        StartCoroutine(RemoveTempArmor(30f, 5f));
    }

    public void BuffHP()
    {
        Debug.Log("Pet hồi máu!");
        playerStats.BuffHealth(300f);
    }

    public void Buffmana()
    {
        Debug.Log("Pet hồi mana!");
        playerStats.BuffMana(500f);
    }

    private IEnumerator RemoveTempArmor(float value, float duration)
    {
        yield return new WaitForSeconds(duration);
        playerStats.maxHp -= value;
        playerStats.currentHp = Mathf.Clamp(playerStats.currentHp, 0, playerStats.maxHp);
        playerStats.sliderHp.maxValue = playerStats.maxHp;
        playerStats.sliderHp.value = playerStats.currentHp;
        playerStats.textHealth.text = ((int)playerStats.currentHp).ToString() + " / " + ((int)playerStats.maxHp).ToString();
        Debug.Log("Giáp buff đã hết hạn.");
    }
}

using UnityEngine;
using System.Collections;

public enum BuffType
{
    Damage,
    Armor,
    Heal
}

public class BuffManager : MonoBehaviour
{
    public PlayerStatus playerStats;

    public void BuffDamage()
    {
        Debug.Log("Pet buff dame!");
        // Gọi hàm đã có trong PlayerStatus: BuffDamage(value, duration)
        playerStats.BuffDamage(20, 5f); // +20 damage trong 5 giây
    }

    public void BuffArmor()
    {
        Debug.Log("Pet buff giáp!");
        // Tăng máu ảo như buff giáp (có thể tùy chỉnh logic riêng)
        playerStats.UpMaxHealth(30f); // Giả sử tạm thời tăng max HP như tăng giáp
        StartCoroutine(RemoveTempArmor(30f, 5f));
    }

    public void BuffHP()
    {
        Debug.Log("Pet hồi máu!");
        playerStats.BuffHealth(300f); // Hồi 25 máu
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

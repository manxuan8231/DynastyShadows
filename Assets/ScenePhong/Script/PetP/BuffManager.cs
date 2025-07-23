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

    void Awake()
    {
        // Tự động gán nếu chưa có
        if (playerStats == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                playerStats = playerObj.GetComponent<PlayerStatus>();

            // Nếu vẫn chưa tìm được
            if (playerStats == null)
                playerStats = FindAnyObjectByType<PlayerStatus>();
        }
    }

    public void BuffDamage()
    {
        if (playerStats == null) return;
        Debug.Log("Pet buff dame!");
        playerStats.BuffDamage(50, 5f); // +50 damage trong 5 giây
    }

    public void BuffArmor()
    {
        if (playerStats == null) return;
        Debug.Log("Pet buff giáp!");
        playerStats.UpMaxHealth(30f);
        StartCoroutine(RemoveTempArmor(30f, 5f));
    }

    public void BuffHP()
    {
        if (playerStats == null) return;
        Debug.Log("Pet hồi máu!");
        playerStats.BuffHealth(300f);
    }

    public void Buffmana()
    {
        if (playerStats == null) return;
        Debug.Log("Pet hồi mana!");
        playerStats.BuffMana(500f);
    }

    private IEnumerator RemoveTempArmor(float value, float duration)
    {
        yield return new WaitForSeconds(duration);
        if (playerStats == null) yield break;

        playerStats.maxHp -= value;
        playerStats.currentHp = Mathf.Clamp(playerStats.currentHp, 0, playerStats.maxHp);
        playerStats.sliderHp.maxValue = playerStats.maxHp;
        playerStats.sliderHp.value = playerStats.currentHp;
        playerStats.textHealth.text = ((int)playerStats.currentHp).ToString() + " / " + ((int)playerStats.maxHp).ToString();
        Debug.Log("Giáp buff đã hết hạn.");
    }
}

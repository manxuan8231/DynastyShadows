using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemSO baseItemSO; // Template gốc
    public OpenInventory OpenInventory;
    public WeaponStatsProfile[] rarityProfiles; // <-- Kéo các profile độ hiếm vào đây

    private void Start()
    {
        OpenInventory = FindAnyObjectByType<OpenInventory>();
        GenerateRandomStatsIfWeapon();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            int leftOverItems = OpenInventory.AddItem(
                baseItemSO.itemName,
                1,
                baseItemSO.itemIcon,
                baseItemSO.itemDescription,
                baseItemSO
            );

            if (leftOverItems <= 0)
                Destroy(gameObject);
        }
    }

    private void GenerateRandomStatsIfWeapon()
    {
        if (baseItemSO.itemType != ItemType.Weapon) return;

        // Chọn độ hiếm random
        Rarity randomRarity = (Rarity)Random.Range(0, 4);

        // Lấy profile tương ứng
        WeaponStatsProfile profile = GetStatsProfileByRarity(randomRarity);
        if (profile == null)
        {
            Debug.LogWarning("Không tìm thấy profile cho độ hiếm: " + randomRarity);
            return;
        }

        // Tạo bản sao mới
        ItemSO newWeapon = ScriptableObject.CreateInstance<ItemSO>();
        newWeapon.itemName = baseItemSO.itemName;
        newWeapon.itemIcon = baseItemSO.itemIcon;
        newWeapon.itemType = baseItemSO.itemType;

        // Random stats theo profile
        newWeapon.sharpness = Random.Range(profile.minSharpness, profile.maxSharpness + 1);
        newWeapon.critRate = Random.Range(profile.minCritRate, profile.maxCritRate);
        newWeapon.critDamage = Random.Range(profile.minCritDamage, profile.maxCritDamage);
        newWeapon.rarity = profile.rarity;

        // Mô tả vũ khí
        newWeapon.itemDescription =
            $"Sắc bén: {newWeapon.sharpness}\n" +
            $"Tỉ lệ chí mạng: {newWeapon.critRate:F1}%\n" +
            $"Sát thương chí mạng: {newWeapon.critDamage:F1}%\n" +
            $"Độ hiếm: {newWeapon.rarity}";

        baseItemSO = newWeapon;
    }

    private WeaponStatsProfile GetStatsProfileByRarity(Rarity rarity)
    {
        foreach (var profile in rarityProfiles)
        {
            if (profile.rarity == rarity)
                return profile;
        }
        return null;
    }
}

using System.Collections.Generic;
using UnityEngine;

public class SkinManager : MonoBehaviour
{
    public static SkinManager Instance { get; private set; }

    [System.Serializable]
    public class SkinData
    {
        public ItemType itemType;
        public SkinnedMeshRenderer renderer;
        public Material common;
        public Material rare;
        public Material epic;
        public Material legendary;
        public Material special;
    }

    public List<SkinData> skins;

    private Dictionary<ItemType, SkinnedMeshRenderer> renderers = new();
    private Dictionary<ItemType, Dictionary<Rarity, Material>> skinMaterials = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Nếu bạn muốn nó tồn tại giữa các scene
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Gán data từ list vào dictionary
        foreach (var skin in skins)
        {
            renderers[skin.itemType] = skin.renderer;

            skinMaterials[skin.itemType] = new Dictionary<Rarity, Material>
            {
                { Rarity.Common, skin.common },
                { Rarity.Rare, skin.rare },
                { Rarity.Epic, skin.epic },
                { Rarity.Legendary, skin.legendary },
                { Rarity.Special, skin.special }
            };
        }
    }

    public void ApplySkin(ItemType type, Rarity rarity)
    {
        if (renderers.TryGetValue(type, out var renderer) &&
            skinMaterials.TryGetValue(type, out var matDict) &&
            matDict.TryGetValue(rarity, out var mat))
        {
            renderer.material = mat;
        }
        else
        {
            Debug.LogWarning($"Không tìm thấy skin cho {type} - {rarity}");
        }
    }
}

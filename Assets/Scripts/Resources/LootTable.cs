
using UnityEngine;
using System.Collections.Generic;
using System.Linq; // Cần thiết cho các hàm xử lý danh sách

public enum LootType
{
    Currency,      
    PowerUp,       
    HealthPack,    
    ShieldPack,    
    CraftingMaterial, 
    QuestItem      
}

[System.Serializable]
public class LootDrop
{
    [Header("Item Definition")]
    [Tooltip("Vật phẩm sẽ rơi ra (kéo Prefab vào đây)")]
    public GameObject itemPrefab;

    [Tooltip("Loại vật phẩm để dễ quản lý và lọc")]
    public LootType type = LootType.PowerUp;

    [Header("Drop Chance & Quantity")]
    [Tooltip("Trọng số rơi. Giá trị càng cao, tỉ lệ rơi càng lớn so với các vật phẩm khác.")]
    [Range(0.01f, 100f)]
    public float dropWeight = 50f;

    [Tooltip("Số lượng tối thiểu sẽ rơi ra trong một lần")]
    [Range(1, 100)]
    public int minAmount = 1;

    [Tooltip("Số lượng tối đa sẽ rơi ra trong một lần")]
    [Range(1, 100)]
    public int maxAmount = 1;

    public LootDrop(GameObject prefab, float weight, int min, int max)
    {
        itemPrefab = prefab;
        dropWeight = weight;
        minAmount = min;
        maxAmount = max;
    }
}

[CreateAssetMenu(fileName = "NewLootTable", menuName = "Game Data/Loot Table")]
public class LootTable : ScriptableObject
{
    [Header("--- Drop Configuration ---")]
    [Tooltip("Danh sách các vật phẩm LUÔN LUÔN rơi ra khi được kích hoạt")]
    public List<LootDrop> guaranteedDrops;

    [Tooltip("Danh sách các vật phẩm CÓ THỂ rơi ra theo tỉ lệ ngẫu nhiên")]
    public List<LootDrop> randomizedDrops;


    public List<(GameObject item, int quantity)> GetDrops(float magicFind = 0f)
    {
        var droppedItems = new List<(GameObject, int)>();

        if (guaranteedDrops != null)
        {
            foreach (var drop in guaranteedDrops)
            {
                if (drop.itemPrefab != null)
                {
                    int quantity = Random.Range(drop.minAmount, drop.maxAmount + 1);
                    droppedItems.Add((drop.itemPrefab, quantity));
                }
            }
        }

        if (randomizedDrops != null && randomizedDrops.Count > 0)
        {
            (GameObject item, int quantity) randomDrop = GetSingleRandomDrop(magicFind);
            if (randomDrop.item != null)
            {
                droppedItems.Add(randomDrop);
            }
        }

        return droppedItems;
    }


    private (GameObject, int) GetSingleRandomDrop(float magicFind)
    {
        if (randomizedDrops == null || randomizedDrops.Count == 0)
        {
            return (null, 0);
        }

        float totalWeight = 0;
        foreach (var drop in randomizedDrops)
        {
            float effectiveWeight = drop.dropWeight * (1f + magicFind);
            totalWeight += effectiveWeight;
        }

        float randomValue = Random.Range(0, totalWeight);
        float currentWeight = 0f;

        foreach (var drop in randomizedDrops)
        {
            float effectiveWeight = drop.dropWeight * (1f + magicFind);
            currentWeight += effectiveWeight;
            if (randomValue <= currentWeight)
            {
                int quantity = Random.Range(drop.minAmount, drop.maxAmount + 1);
                return (drop.itemPrefab, quantity);
            }
        }

        return (null, 0); // Trường hợp dự phòng
    }


    private void OnValidate()
    {
        if (guaranteedDrops != null)
        {
            foreach (var drop in guaranteedDrops)
            {
                if (drop.minAmount > drop.maxAmount)
                {
                    drop.minAmount = drop.maxAmount;
                    Debug.LogWarning($"LootTable '{this.name}': Min amount > Max amount trong Guaranteed Drops đã được sửa lại.");
                }
            }
        }
        if (randomizedDrops != null)
        {
            foreach (var drop in randomizedDrops)
            {
                if (drop.minAmount > drop.maxAmount)
                {
                    drop.minAmount = drop.maxAmount;
                    Debug.LogWarning($"LootTable '{this.name}': Min amount > Max amount trong Randomized Drops đã được sửa lại.");
                }
            }
        }

        bool missingPrefab = guaranteedDrops.Any(d => d.itemPrefab == null) || randomizedDrops.Any(d => d.itemPrefab == null);
        if (missingPrefab)
        {
            Debug.LogError($"LootTable '{this.name}': Có một hoặc nhiều vật phẩm bị thiếu Prefab!");
        }
    }
}
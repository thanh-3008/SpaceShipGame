using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class LootDrop
{
    [Tooltip("Vật phẩm sẽ rơi ra (kéo Prefab vào đây)")]
    public GameObject itemPrefab;

    [Tooltip("Tỉ lệ rơi (càng cao càng dễ rơi)")]
    [Range(0.01f, 100f)]
    public float dropChance;
}

[CreateAssetMenu(fileName = "NewLootTable", menuName = "Game Data/Loot Table")]
public class LootTable : ScriptableObject
{
    [Header("Loot Drop Configuration")]
    [Tooltip("Danh sách tất cả các vật phẩm có thể rơi ra từ bảng này")]
    public List<LootDrop> possibleDrops;

    public GameObject GetRandomDrop()
    {
        float totalChance = 0f;
        foreach (var drop in possibleDrops)
        {
            totalChance += drop.dropChance;
        }

        float randomValue = Random.Range(0, totalChance);

        foreach (var drop in possibleDrops)
        {
            if (randomValue <= drop.dropChance)
            {
                return drop.itemPrefab;
            }
            randomValue -= drop.dropChance;
        }
        return null; // Không rơi ra gì cả
    }
}
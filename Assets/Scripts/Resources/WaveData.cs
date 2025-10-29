using UnityEngine;

[System.Serializable]
public class EnemyGroup
{
    [Tooltip("Loại kẻ địch (kéo Prefab vào đây)")]
    public GameObject enemyPrefab;

    [Tooltip("Số lượng kẻ địch trong nhóm này")]
    [Range(1, 100)] // Tăng giới hạn lên 100
    public int count;

    [Header("Group Spawning")]
    [Tooltip("Thời gian giãn cách giữa mỗi kẻ địch (giây). Đặt là 0 để thả tất cả cùng lúc.")]
    [Range(0f, 5f)]
    public float timeBetweenSpawns = 0.5f;

    [Tooltip("(Tùy chọn) Chỉ định điểm thả lính (ví dụ: 0, 1, 2...).")]
    public int spawnPointIndex = 0; // Hữu ích nếu bạn có nhiều điểm spawn
}
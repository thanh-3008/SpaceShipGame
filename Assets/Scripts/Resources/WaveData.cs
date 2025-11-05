using UnityEngine;

public enum SpawnFormation
{
    Line_Horizontal, 
    Line_Vertical,   
    V_Formation,     
    Circle,          
    Random_In_Area   
}

[System.Serializable]
public class EnemyGroup
{
    [Tooltip("Mô tả về nhóm này, ví dụ: 'Tàu cảm tử'")]
    public string groupName;

    [Tooltip("Loại kẻ địch (kéo Prefab vào đây)")]
    public GameObject enemyPrefab;

    [Tooltip("Số lượng kẻ địch trong nhóm này")]
    [Range(1, 100)] // Tăng giới hạn lên 100
    public int count;

    [Tooltip("Đội hình xuất hiện")]
    public SpawnFormation formation;

    [Tooltip("Thời gian chờ trước khi thả nhóm này (giây)")]
    public float spawnDelay;
}

[CreateAssetMenu(fileName = "NewEnemyWave", menuName = "Game Data/Enemy Wave")]
public class WaveData : ScriptableObject
{
    [Header("Wave Information")]
    [Tooltip("Tên của đợt tấn công, ví dụ: 'Wave 1 - The Scouts'")]
    public string waveName;

    [TextArea(3, 5)]
    [Tooltip("Mô tả về đợt tấn công này")]
    public string waveDescription;

    [Tooltip("Độ khó của wave này, dùng để cân bằng game")]
    [Range(1, 10)]
    public int difficultyRating;

    [Header("Wave Composition")]
    [Tooltip("Danh sách các nhóm kẻ địch sẽ xuất hiện tuần tự trong đợt này")]
    public List<EnemyGroup> enemyGroups;

    [Header("Wave Timing")]
    [Tooltip("Thời gian chờ trước khi đợt này bắt đầu")]
    public float delayBeforeWaveStarts = 3.0f;

    [Tooltip("Thời gian chờ sau khi đợt này kết thúc để bắt đầu đợt mới")]
    public float delayAfterWaveEnds = 5.0f;

    public int GetTotalEnemyCount()
    {
        int total = 0;
        foreach (var group in enemyGroups)
        {
            total += group.count;
        }
        return total;
    }

    public float GetEstimatedWaveDuration()
    {
        float duration = 0;
        foreach (var group in enemyGroups)
        {
            duration += group.spawnDelay;
        }
        return duration;
    }
}
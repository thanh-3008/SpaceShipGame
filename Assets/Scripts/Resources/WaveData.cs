using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class EnemyGroup
{
    [Tooltip("Loại kẻ địch (kéo Prefab vào đây)")]
    public GameObject enemyPrefab;

    [Tooltip("Số lượng kẻ địch trong nhóm này")]
    [Range(1, 50)]
    public int count;

    [Tooltip("Thời gian chờ trước khi thả nhóm này (giây)")]
    public float spawnDelay;
}

[CreateAssetMenu(fileName = "NewEnemyWave", menuName = "Game Data/Enemy Wave")]
public class WaveData : ScriptableObject
{
    [Header("Wave Configuration")]
    [Tooltip("Tên của đợt tấn công, ví dụ: 'Wave 1 - Scouts'")]
    public string waveName;

    [Tooltip("Danh sách các nhóm kẻ địch sẽ xuất hiện trong đợt này")]
    public List<EnemyGroup> enemyGroups;

    [Header("Wave Timing")]
    [Tooltip("Thời gian chờ trước khi đợt này bắt đầu")]
    public float delayBeforeWaveStarts = 3.0f;

    [Tooltip("Thời gian chờ sau khi đợt này kết thúc để bắt đầu đợt mới")]
    public float delayAfterWaveEnds = 5.0f;
}
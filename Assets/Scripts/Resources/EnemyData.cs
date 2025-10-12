using UnityEngine;

public enum MovementPattern
{
    Straight,
    SineWave,
    Kamikaze,
    Static
}

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "Game Data/Enemy Data")]
public class EnemyData : ScriptableObject
{
    [Header("Enemy Info")]
    public string enemyName = "New Enemy";

    [Header("Stats")]
    [Range(10, 5000)]
    public float maxHealth = 100f;

    [Range(1f, 20f)]
    public float moveSpeed = 5f;

    public int scoreValue = 10;

    [Header("Behavior")]
    public MovementPattern movementPattern = MovementPattern.Straight;

    [Header("Visuals & SFX")]
    public GameObject enemyPrefab;
    public GameObject explosionEffect;
    public AudioClip hitSound;
    public AudioClip explosionSound;
}
using UnityEngine;
using System.Collections.Generic;


public enum MovementPattern
{
    Static,         
    Straight,       
    SineWave,       
    ChasePlayer,    
    Patrol,         
    Kamikaze        
}


public enum AttackPattern
{
    None,           
    SingleShot,     
    SpreadShot,     
    BurstFire,      
    ChargeBeam,     
    ContactDamage   
}


public enum DamageType
{
    Kinetic,    
    Energy,     
    Explosive,  
    EMP         
}


[CreateAssetMenu(fileName = "NewEnemyData", menuName = "Game Data/Enemy Data")]
public class EnemyData : ScriptableObject
{
    [Header("--- General Info ---")]
    [Tooltip("Tên của loại kẻ địch, ví dụ: 'Interceptor', 'Dreadnought'")]
    public string enemyName = "New Enemy";
    [TextArea(3, 5)]
    [Tooltip("Mô tả về kẻ địch này cho mục đích thiết kế")]
    public string designNotes;

    [Header("--- Core Stats ---")]
    [Tooltip("Lượng máu tối đa")]
    [Range(10, 20000)]
    public float maxHealth = 100f;

    [Tooltip("Lượng giáp. Sát thương sẽ trừ vào giáp trước khi trừ vào máu.")]
    [Range(0, 10000)]
    public float armor = 0f;

    [Tooltip("Điểm nhận được khi tiêu diệt")]
    [Range(0, 10000)]
    public int scoreValue = 10;


    [Header("--- Movement AI ---")]
    [Tooltip("Mô hình di chuyển chính của kẻ địch")]
    public MovementPattern movementPattern = MovementPattern.Straight;

    [Tooltip("Tốc độ di chuyển")]
    [Range(1f, 50f)]
    public float moveSpeed = 5f;

    [Tooltip("Tốc độ xoay (độ mỗi giây)")]
    [Range(10f, 720f)]
    public float turnSpeed = 180f;


    [Header("--- Combat AI ---")]
    [Tooltip("Mô hình tấn công của kẻ địch")]
    public AttackPattern attackPattern = AttackPattern.SingleShot;

    [Tooltip("Prefab của viên đạn hoặc vũ khí kẻ địch sử dụng")]
    public GameObject projectilePrefab;

    [Tooltip("Sát thương gây ra bởi mỗi đòn tấn công")]
    [Range(1, 1000)]
    public float damage = 10f;

    [Tooltip("Loại sát thương gây ra")]
    public DamageType damageType = DamageType.Kinetic;

    [Tooltip("Khoảng cách tối thiểu để bắt đầu tấn công")]
    [Range(1f, 100f)]
    public float attackRange = 15f;

    [Tooltip("Thời gian chờ giữa các lần tấn công (giây)")]
    [Range(0.1f, 10f)]
    public float attackCooldown = 2f;


    [Header("--- Resistances & Weaknesses ---")]
    [Tooltip("Hệ số kháng sát thương Kinetic (1 = không kháng, >1 = yếu, <1 = kháng)")]
    [Range(0f, 2f)]
    public float kineticResistance = 1.0f;

    [Tooltip("Hệ số kháng sát thương Energy")]
    [Range(0f, 2f)]
    public float energyResistance = 1.0f;

    [Tooltip("Hệ số kháng sát thương Explosive")]
    [Range(0f, 2f)]
    public float explosiveResistance = 1.0f;


    [Header("--- Drops & Rewards ---")]
    [Tooltip("Bảng vật phẩm sẽ rơi ra khi kẻ địch này bị tiêu diệt")]
    public LootTable lootTable;


    [Header("--- Visuals & SFX ---")]
    [Tooltip("Prefab của model tàu địch (bao gồm cả hình ảnh, collider...)")]
    public GameObject enemyPrefab;

    [Tooltip("Prefab của hiệu ứng nổ khi bị phá hủy")]
    public GameObject explosionEffect;

    [Tooltip("Âm thanh khi bị bắn trúng")]
    public AudioClip hitSound;

    [Tooltip("Âm thanh khi bị phá hủy")]
    public AudioClip explosionSound;

    [Tooltip("Âm thanh khi tấn công")]
    public AudioClip attackSound;


    public float CalculateDamageTaken(float incomingDamage, DamageType type)
    {
        float multiplier = 1.0f;
        switch (type)
        {
            case DamageType.Kinetic:
                multiplier = kineticResistance;
                break;
            case DamageType.Energy:
                multiplier = energyResistance;
                break;
            case DamageType.Explosive:
                multiplier = explosiveResistance;
                break;
        }
        return incomingDamage * multiplier;
    }
}
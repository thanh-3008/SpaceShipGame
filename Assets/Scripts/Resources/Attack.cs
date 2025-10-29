using UnityEngine;
using System.Collections;

public class BossAttack : MonoBehaviour
{
    public enum BossState { Chasing, Attacking, Cooldown, Idle }
    [Header("Trạng thái")]
    public BossState currentState = BossState.Idle;

    [Header("Thiết lập mục tiêu")]
    public Transform playerTarget;
    public float detectionRange = 20f; 
    public float rotationSpeed = 5f;

    [Header("Thiết lập Tấn công")]
    public float attackRange = 3f; 
    public float attackCooldown = 2f; 
    private float lastAttackTime = 0f;

    [Header("Tấn công Cận chiến (Melee)")]
    public int meleeDamage = 20;

    [Header("Tấn công Tầm xa (Ranged)")]
    public GameObject projectilePrefab; 
    public Transform firePoint; 
    public float projectileSpeed = 15f;

    private Animator animator;

    void Start()
    {
        if (playerTarget == null) { }
    }
}
using UnityEngine;
using System.Collections;

public class BossAttack : MonoBehaviour
{
    // Trạng thái của Boss
    public enum BossState { Chasing, Attacking, Cooldown, Idle }
    [Header("Trạng thái")]
    public BossState currentState = BossState.Idle;

    [Header("Thiết lập mục tiêu")]
    public Transform playerTarget;
    public float detectionRange = 20f; // Khoảng cách phát hiện người chơi
    public float rotationSpeed = 5f;

    [Header("Thiết lập Tấn công")]
    public float attackRange = 3f; // Khoảng cách để tấn công
    public float attackCooldown = 2f; // Thời gian chờ giữa các đòn tấn công
    private float lastAttackTime = 0f;

    [Header("Tấn công Cận chiến (Melee)")]
    public int meleeDamage = 20;

    [Header("Tấn công Tầm xa (Ranged)")]
    public GameObject projectilePrefab; // Prefab của đạn
    public Transform firePoint; // Vị trí bắn đạn
    public float projectileSpeed = 15f;

    // Tham chiếu
    private Animator animator;

    void Start()
    {
        // Tự động tìm người chơi nếu chưa được gán
        if (playerTarget == null)
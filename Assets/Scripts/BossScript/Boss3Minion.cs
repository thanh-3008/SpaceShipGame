using UnityEngine;

// GẮN SCRIPT NÀY LÊN PREFAB ONG CON
// (Cùng với script BossController)
public class Boss3_Minion : MonoBehaviour
{
    // Trạng thái của ong con
    private enum MinionState
    {
        MovingToOrbit, // Đang bay tới điểm lơ lửng
        Hovering,      // Đang lơ lửng
        Attacking,     // Đang lao tới mục tiêu
        WaitingAtTarget, // ĐÃ TỚI MỤC TIÊU, ĐANG CHỜ
        ReturningToOrbit // ĐANG BAY VỀ VỊ TRÍ LƠ LỬNG
    }
    private MinionState currentState;

    [Header("Chỉ Số (AI)")]
    public float damageOnImpact = 20f;
    public float hoverMoveSpeed = 5f;  // Tốc độ bay tới/về điểm lơ lửng
    public float attackMoveSpeed = 15f; // Tốc độ lao tới
    [Tooltip("Thời gian (giây) ong con ở lại vị trí tấn công")]
    public float attackLingerTime = 1.0f; // Chờ 1 giây

    [Header("Hovering (Lơ lửng)")]
    public float hoverPatrolRange = 1f;
    public float hoverPatrolSpeed = 2f;

    [HideInInspector]
    public Transform assignedOrbitPoint; // Vị trí lơ lửng
    private Vector2 attackDashTarget; // Vị trí tấn công
    private Vector2 orbitCenterPosition; // Vị trí trung tâm lơ lửng
    private float hoverTimer;
    private float waitTimer; // Bộ đếm thời gian chờ

    void Update()
    {
        switch (currentState)
        {
            // (Tất cả các case... giữ nguyên)
            case MinionState.MovingToOrbit:
                if (assignedOrbitPoint != null)
                {
                    transform.position = Vector2.MoveTowards(transform.position, assignedOrbitPoint.position, hoverMoveSpeed * Time.deltaTime);
                    if (Vector2.Distance(transform.position, assignedOrbitPoint.position) < 0.1f)
                    {
                        currentState = MinionState.Hovering;
                        orbitCenterPosition = assignedOrbitPoint.position;
                        hoverTimer = 0f;
                    }
                }
                else { Die(); }
                break;
            case MinionState.Hovering:
                if (assignedOrbitPoint != null)
                {
                    hoverTimer += Time.deltaTime * hoverPatrolSpeed;
                    float offset = Mathf.Sin(hoverTimer) * hoverPatrolRange;
                    transform.position = new Vector2(orbitCenterPosition.x + offset, orbitCenterPosition.y);
                }
                else { Die(); }
                break;
            case MinionState.Attacking:
                transform.position = Vector2.MoveTowards(transform.position, attackDashTarget, attackMoveSpeed * Time.deltaTime);
                if (Vector2.Distance(transform.position, attackDashTarget) < 0.1f)
                {
                    currentState = MinionState.WaitingAtTarget;
                    waitTimer = attackLingerTime;
                }
                break;
            case MinionState.WaitingAtTarget:
                waitTimer -= Time.deltaTime;
                if (waitTimer <= 0)
                {
                    currentState = MinionState.ReturningToOrbit;
                }
                break;
            case MinionState.ReturningToOrbit:
                if (assignedOrbitPoint != null)
                {
                    transform.position = Vector2.MoveTowards(transform.position, orbitCenterPosition, hoverMoveSpeed * Time.deltaTime);
                    if (Vector2.Distance(transform.position, orbitCenterPosition) < 0.1f)
                    {
                        currentState = MinionState.Hovering;
                        hoverTimer = 0f;
                    }
                }
                else { Die(); }
                break;
        }
    }

    /// <summary>
    /// Boss gọi hàm này khi triệu hồi
    /// </summary>
    public void Initialize(Transform orbitPoint)
    {
        this.assignedOrbitPoint = orbitPoint;
        this.currentState = MinionState.MovingToOrbit;
    }

    /// <summary>
    /// Boss gọi hàm này khi dùng Skill 4
    /// </summary>
    public void LaunchAttack(Vector2 targetPosition)
    {
        this.attackDashTarget = targetPosition;
        this.currentState = MinionState.Attacking;
    }

    /// <summary>
    /// (MỚI) Kiểm tra xem ong con có đang rảnh (lơ lửng) không
    /// </summary>
    public bool IsHovering()
    {
        return currentState == MinionState.Hovering;
    }

    // --- SỬA ĐỔI HÀM VA CHẠM ---
    void OnTriggerEnter2D(Collider2D other)
    {
        // BÂY GIỜ GÂY SÁT THƯƠNG BẤT KỂ TRẠNG THÁI
        if (other.CompareTag("Player"))
        {
            BossController health = GetComponent<BossController>();
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                // Dùng damageOnImpact
                player.TakeDame(damageOnImpact);
                health.TakeDame(health.maxHealth / 2);
            }       
        }
    }
    // ----------------------------

    /// <summary>
    /// Hàm này sẽ được BossController (gắn CÙNG 1 GameObject) gọi khi hết máu
    /// </summary>
    public void Die()
    {
        currentState = MinionState.Attacking; // Dừng Update
        assignedOrbitPoint = null;
        Destroy(gameObject);
        // (BossController sẽ tự gọi Destroy)
    }
}
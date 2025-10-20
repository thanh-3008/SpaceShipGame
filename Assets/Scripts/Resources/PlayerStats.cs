using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerStats", menuName = "Game Data/Player Stats")]
public class PlayerStats : ScriptableObject
{
    [Header("Ship Information")]
    [Tooltip("Tên loại phi thuyền, ví dụ: 'Seraph MKII'")]
    public string shipName;

    [Tooltip("Icon hiển thị trên UI chọn tàu")]
    public Sprite shipIcon;

    [TextArea(3, 5)]
    [Tooltip("Mô tả ngắn về phi thuyền, cốt truyện, điểm mạnh/yếu")]
    public string shipDescription;

    [Header("Defensive Stats")]
    [Tooltip("Máu tối đa của thân tàu")]
    [Range(50, 5000)]
    public float maxHealth = 500f;

    [Tooltip("Giáp tối đa / Năng lượng khiên")]
    [Range(0, 5000)]
    public float maxShield = 250f;

    [Tooltip("Thời gian chờ (giây) sau khi bị tấn công trước khi khiên bắt đầu hồi")]
    [Range(0f, 10f)]
    public float shieldRechargeDelay = 3.0f;

    [Tooltip("Tốc độ hồi khiên mỗi giây")]
    [Range(0, 500)]
    public float shieldRegenRate = 10f;

    [Tooltip("Tỉ lệ giảm sát thương nhận vào (0 = 0%, 1 = 100%)")]
    [Range(0f, 0.9f)]
    public float damageResistance = 0f;

    [Header("Offensive Stats")]
    [Tooltip("Sát thương cơ bản của mỗi viên đạn")]
    [Range(5, 200)]
    public int baseDamage = 10;

    [Tooltip("Tốc độ bắn cơ bản (phát đạn mỗi giây)")]
    [Range(1f, 30f)]
    public float fireRate = 4f;

    [Tooltip("Tỉ lệ chí mạng (0 = 0%, 1 = 100%)")]
    [Range(0f, 1f)]
    public float critChance = 0.05f;

    [Tooltip("Bội số sát thương khi chí mạng (2.0 = 200% sát thương)")]
    [Range(1.5f, 5f)]
    public float critDamageMultiplier = 2.0f;

    [Header("Mobility & Utility")]
    [Tooltip("Tốc độ di chuyển cơ bản")]
    [Range(1f, 30f)]
    public float moveSpeed = 10f;

    [Tooltip("Tốc độ xoay của tàu (độ mỗi giây)")]
    [Range(10f, 720f)]
    public float turnSpeed = 360f;

    [Tooltip("Bán kính tàu tự động hút vật phẩm")]
    [Range(0f, 10f)]
    public float pickupRadius = 2f;

    [Header("Visual & Audio Effects")]
    [Tooltip("Prefab của hiệu ứng động cơ")]
    public GameObject thrusterEffectPrefab;

    [Tooltip("Âm thanh khi bắn đạn")]
    public AudioClip weaponFireSound;


    public float CalculateTheoreticalDPS()
    {
        float averageDamagePerShot = baseDamage * (1 - critChance) + (baseDamage * critDamageMultiplier * critChance);
        return averageDamagePerShot * fireRate;
    }

    public float GetEffectiveHealth()
    {
        if (damageResistance >= 1) return float.MaxValue;
        return (maxHealth + maxShield) / (1 - damageResistance);
    }
}
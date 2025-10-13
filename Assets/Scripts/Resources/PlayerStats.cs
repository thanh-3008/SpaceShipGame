using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerStats", menuName = "Game Data/Player Stats")]
public class PlayerStats : ScriptableObject
{
    [Header("Ship Info")]
    [Tooltip("Tên loại phi thuyền, ví dụ: 'Seraph MKII'")]
    public string shipName;

    [TextArea(3, 5)]
    [Tooltip("Mô tả ngắn về phi thuyền")]
    public string shipDescription;

    [Header("Core Stats")]
    [Tooltip("Máu tối đa")]
    [Range(50, 2000)]
    public float maxHealth = 500f;

    [Tooltip("Giáp tối đa / Năng lượng khiên")]
    [Range(0, 2000)]
    public float maxShield = 250f;

    [Tooltip("Tốc độ hồi khiên mỗi giây")]
    [Range(0, 100)]
    public float shieldRegenRate = 10f;

    [Tooltip("Tốc độ di chuyển cơ bản")]
    [Range(1f, 30f)]
    public float moveSpeed = 10f;

    [Header("Weapon Stats")]
    [Tooltip("Sát thương cơ bản của đạn")]
    [Range(5, 100)]
    public int baseDamage = 10;

    [Tooltip("Tốc độ bắn cơ bản (phát đạn mỗi giây)")]
    [Range(1f, 20f)]
    public float fireRate = 4f;
}
using UnityEngine;

[CreateAssetMenu(fileName = "New Upgrade", menuName = "Upgrades/Upgrade Data")]
public class UpgradeData : ScriptableObject
{
    public string upgradeName;
    [TextArea]
    public string description;
    public Sprite icon;

    // Thêm các thuộc tính khác nếu cần, ví dụ:
    // public float damageMultiplier;
    // public float speedBonus;
}
using UnityEngine;

public enum UpgradeType
{
    Normal,
    Pro,
    VinhVien
}

[CreateAssetMenu(fileName = "New Upgrade", menuName = "Upgrades/Upgrade Data")]
public class UpgradeData : ScriptableObject
{
    public string upgradeName;
    [TextArea]
    public string description;
    public Sprite icon;

    public UpgradeType type = UpgradeType.Normal;

    public int maxChosse = 5; // Số lần tối đa có thể chọn nâng cấp này

    public Color color ;

    public UpgradeData upgradePro; // Tham chiếu đến UpgradeData của phiên bản Pro
   
}
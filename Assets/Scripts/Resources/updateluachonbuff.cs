using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class UpgradeOption
{
    public string upgradeName;
    public string description;
    public UnityEngine.Sprite icon; // Ghi rõ UnityEngine.Sprite để không bị nhầm lẫn
    public System.Action onSelect;
}
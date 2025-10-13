using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UpgradeManager : MonoBehaviour
{
    public GameObject upgradeCardPrefab; // Prefab thẻ nâng cấp
    public Transform cardContainer;      // Nơi chứa 3 thẻ (một panel ngang)
    public GameObject upgradePanel;      // Panel tổng (ẩn/hiện toàn bộ UI)

    private List<GameObject> activeCards = new List<GameObject>();

    void Start()
    {
        upgradePanel.SetActive(false);
    }

    public void ShowUpgrades()
    {
        upgradePanel.SetActive(true);

        // Tạo 3 lựa chọn mẫu
        CreateCard(new UpgradeOption
        {
            upgradeName = "+1 Speed",
            description = "Tốc độ di chuyển của tàu tăng +1",
            icon = Resources.Load<Sprite>("Icons/speed"),
            onSelect = () => {
                Debug.Log("Chọn buff Speed");
                // Gọi logic tăng speed tại đây
                upgradePanel.SetActive(false);
            }
        });

        CreateCard(new UpgradeOption
        {
            upgradeName = "+20% Damage",
            description = "Tăng sát thương kỹ năng +20%",
            icon = Resources.Load<Sprite>("Icons/damage"),
            onSelect = () => {
                Debug.Log("Chọn buff Damage");
                // Gọi logic tăng damage tại đây
                upgradePanel.SetActive(false);
            }
        });

        CreateCard(new UpgradeOption
        {
            upgradeName = "+20% Crit Damage",
            description = "Tăng sát thương chí mạng +20%",
            icon = Resources.Load<Sprite>("Icons/crit"),
            onSelect = () => {
                Debug.Log("Chọn buff Crit Damage");
                // Gọi logic tăng crit damage tại đây
                upgradePanel.SetActive(false);
            }
        });
    }

    void CreateCard(UpgradeOption option)
    {
        GameObject card = Instantiate(upgradeCardPrefab, cardContainer);
        card.transform.Find("Icon").GetComponent<Image>().sprite = option.icon;
        card.transform.Find("Title").GetComponent<Text>().text = option.upgradeName;
        card.transform.Find("Description").GetComponent<Text>().text = option.description;

        Button btn = card.transform.Find("Button").GetComponent<Button>();
        btn.onClick.AddListener(() => option.onSelect());

        activeCards.Add(card);
    }

    public void ClearCards()
    {
        foreach (var card in activeCards)
            Destroy(card);
        activeCards.Clear();
    }
}

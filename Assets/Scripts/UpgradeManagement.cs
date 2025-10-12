
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManagement : MonoBehaviour
{
    public GameObject panelUpgrade;
    public List<UpgradeData> allUpgrades;

    public TextMeshProUGUI textName1, textName2, textName3;
    public TextMeshProUGUI textMoTa1, textMoTa2, textMoTa3;
    public Image img1, img2, img3;
    public Button btn1, btn2, btn3;

    private PlayerLevel playerLevel;
    private List<UpgradeData> selectUpgrade;
    private PlayerController playerController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerLevel = FindObjectOfType<PlayerLevel>();
        playerLevel.onLevelUp.AddListener(ShowUpgradePanel);
        playerController = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ShowUpgradePanel()
    {
        Time.timeScale = 0f;
        panelUpgrade.SetActive(true);
        selectUpgrade = getRandomUpgrade(3);
        for(int i = 0; i < selectUpgrade.Count; i++)
        {
            UpgradeCardUI(i, selectUpgrade[i]);
        }
        
    }

    public void UpgradeCardUI(int cardIndex, UpgradeData upgradeData)
    {
        switch(cardIndex)
        {
            case 0:
                textName1.text = upgradeData.upgradeName;
                textMoTa1.text = upgradeData.description;
                img1.sprite = upgradeData.icon;
                btn1.onClick.RemoveAllListeners();
                btn1.onClick.AddListener(() => SelectUpgrade(0));
                break;
            case 1:
                textName2.text = upgradeData.upgradeName;
                textMoTa2.text = upgradeData.description;
                img2.sprite = upgradeData.icon;
                btn2.onClick.RemoveAllListeners();
                btn2.onClick.AddListener(() => SelectUpgrade(1));
                break;
            case 2:
                textName3.text = upgradeData.upgradeName;
                textMoTa3.text = upgradeData.description;
                img3.sprite = upgradeData.icon;
                btn3.onClick.RemoveAllListeners();
                btn3.onClick.AddListener(() => SelectUpgrade(2));
                break;
        }
    }

    public List<UpgradeData> getRandomUpgrade(int count)
    {
        List<UpgradeData> upgradeAvailable = new List<UpgradeData>(allUpgrades);
        List<UpgradeData> selectedUpgrades = new List<UpgradeData>();
        for (int i = 0;i < count ; i++)
        {
            if(upgradeAvailable.Count == 0)
            {
                break;
            }
            int randomIndex = Random.Range(0, upgradeAvailable.Count);
            selectedUpgrades.Add(upgradeAvailable[randomIndex]);
            upgradeAvailable.RemoveAt(randomIndex);
        }
        return selectedUpgrades;
    }

    public void SelectUpgrade(int index)
    {
        UpgradeData selectedUpgrade = selectUpgrade[index];
        // Áp dụng hiệu ứng của nâng cấp đã chọn cho người chơi
        if (selectedUpgrade != null)
        {
            Debug.Log("Selected Upgrade: " + selectedUpgrade.upgradeName);
            // Thêm logic áp dụng nâng cấp cho người chơi ở đây
            if (selectedUpgrade.upgradeName == "+1 Speed")
            {
                playerController.speed += 1f;
            }
            else if (selectedUpgrade.upgradeName == "+25 HP")
            {
                playerController.thanhmauToiDa += 25f;
                playerController.thanhmauhientai += 25f;
                playerController.thanhmau.capnhatthanhmau(playerController.thanhmauhientai, playerController.thanhmauToiDa);
            }
            else if (selectedUpgrade.upgradeName == "+20% Damage")
            {
                playerController.damecongthem +=0.2f;
            }
            else if (selectedUpgrade.upgradeName == "+20% Crit Rate")
            {
                playerController.critRate += 0.2f;
            }
            else if (selectedUpgrade.upgradeName == "+20% Crit Dame")
            {
                playerController.critDame += 0.2f;
            }
            else if (selectedUpgrade.upgradeName == "+20 Giáp")
            {
                playerController.Giap += 20f;
            }

            panelUpgrade.SetActive(false);
            Time.timeScale = 1f;
        }
    }
}

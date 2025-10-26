using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq; // <-- ADDED: Cần thiết cho việc lọc danh sách

public class UpgradeManagement : MonoBehaviour
{
    public GameObject panelUpgrade;
    public List<UpgradeData> statsUpgrades;
    public List<UpgradeData> normalUpgrades;
    public List<UpgradeData> skillUpgrades;

    public TextMeshProUGUI textName1, textName2, textName3;
    public TextMeshProUGUI textMoTa1, textMoTa2, textMoTa3;
    public Image img1, img2, img3;
    public Image imgPanel1, imgPanel2, imgPanel3;
    public Button btn1, btn2, btn3;

    private Dictionary<UpgradeData, int> soUpgradesDangCo = new Dictionary<UpgradeData, int>();

    private PlayerLevel playerLevel;
    private List<UpgradeData> selectUpgrade;
    private PlayerController playerController;
    private SpawnSuriken spawnSuriken;
    private int soLanChonStat = 0;
    private int soLanChonSkill = 0;
    private GameObject tauTuanTra;
    private SpawnTauTuanTra spawnTauTuanTra;
    public GameObject StarManagement1, StarManagement2, StarManagement3;
    public GameObject StarPro1, StarPro2, StarPro3;

    // --- ADDED: Biến điều chỉnh tỉ lệ ưu tiên ---
    [Tooltip("Tỉ lệ (0.0 - 1.0) ưu tiên chọn kỹ năng đã sở hữu.")]
    [Range(0f, 1f)]
    public float ownedSkillBiasChance = 0.6f; // 70% cơ hội
    // ------------------------------------------

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerLevel = FindObjectOfType<PlayerLevel>();
        playerLevel.onLevelUp.AddListener(ShowUpgradePanel);
        playerController = FindObjectOfType<PlayerController>();
        spawnSuriken = FindObjectOfType<SpawnSuriken>();
        tauTuanTra = GameObject.Find("SpawnTauTuanTra");
        spawnTauTuanTra = tauTuanTra.GetComponent<SpawnTauTuanTra>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void CheckUpgrade(UpgradeData selectCurrentUpgarade)
    {
        int currentCount = 0;
        if (soUpgradesDangCo.ContainsKey(selectCurrentUpgarade))
        {
            soUpgradesDangCo[selectCurrentUpgarade]++;
            currentCount = soUpgradesDangCo[selectCurrentUpgarade];
        }
        else
        {
            soUpgradesDangCo.Add(selectCurrentUpgarade, 1);
            currentCount = 1;
        }
    }

    public void ShowUpgradePanel()
    {
        Time.timeScale = 0f;
        panelUpgrade.SetActive(true);
        selectUpgrade = getRandomUpgrade(3);
        for (int i = 0; i < selectUpgrade.Count; i++)
        {
            UpgradeCardUI(i, selectUpgrade[i]);
        }

    }

    public void UpgradeCardUI(int cardIndex, UpgradeData upgradeData)
    {
        switch (cardIndex)
        {
            case 0:
                StarManagement1.SetActive(false);
                StarPro1.SetActive(false);
                textName1.text = upgradeData.upgradeName;
                textMoTa1.text = upgradeData.description;
                img1.sprite = upgradeData.icon;
                imgPanel1.color = upgradeData.color;
                btn1.onClick.RemoveAllListeners();
                btn1.onClick.AddListener(() => SelectUpgrade(0));
                if (upgradeData.type == UpgradeType.Normal)
                {
                    StarManagement1.SetActive(true);
                    StarManagement1.GetComponent<StarManagement>().HienThiStar(soUpgradesDangCo.ContainsKey(upgradeData) ? soUpgradesDangCo[upgradeData] : 0);
                }
                if (upgradeData.type == UpgradeType.Pro)
                {
                    StarPro1.SetActive(true);
                }
                break;
            case 1:
                StarManagement2.SetActive(false);
                StarPro2.SetActive(false);
                textName2.text = upgradeData.upgradeName;
                textMoTa2.text = upgradeData.description;
                img2.sprite = upgradeData.icon;
                imgPanel2.color = upgradeData.color;
                btn2.onClick.RemoveAllListeners();
                btn2.onClick.AddListener(() => SelectUpgrade(1));
                if (upgradeData.type == UpgradeType.Normal)
                {
                    StarManagement2.SetActive(true);
                    StarManagement2.GetComponent<StarManagement>().HienThiStar(soUpgradesDangCo.ContainsKey(upgradeData) ? soUpgradesDangCo[upgradeData] : 0);
                }
                if (upgradeData.type == UpgradeType.Pro)
                {
                    StarPro2.SetActive(true);
                }
                break;
            case 2:
                StarManagement3.SetActive(false);
                StarPro3.SetActive(false);
                textName3.text = upgradeData.upgradeName;
                textMoTa3.text = upgradeData.description;
                img3.sprite = upgradeData.icon;
                imgPanel3.color = upgradeData.color;
                btn3.onClick.RemoveAllListeners();
                btn3.onClick.AddListener(() => SelectUpgrade(2));
                if (upgradeData.type == UpgradeType.Normal)
                {
                    StarManagement3.SetActive(true);
                    StarManagement3.GetComponent<StarManagement>().HienThiStar(soUpgradesDangCo.ContainsKey(upgradeData) ? soUpgradesDangCo[upgradeData] : 0);
                }
                if (upgradeData.type == UpgradeType.Pro)
                {
                    StarPro3.SetActive(true);
                }
                break;
        }
    }

    // --- ADDED: Hàm trợ giúp để chọn kỹ năng có ưu tiên ---
    private UpgradeData GetBiasedSkill(List<UpgradeData> availableSkills)
    {
        // 1. Tìm các kỹ năng vừa có sẵn VÀ người chơi đã sở hữu
        List<UpgradeData> ownedAvailableSkills = availableSkills
            .Where(skill => soUpgradesDangCo.ContainsKey(skill))
            .ToList();

        // 2. Quyết định xem có chọn từ danh sách ưu tiên hay không
        bool pickFromOwned = false;
        if (ownedAvailableSkills.Count > 0 && Random.value < ownedSkillBiasChance)
        {
            // Random.value trả về một số float từ 0.0 đến 1.0
            pickFromOwned = true;
        }

        // 3. Chọn kỹ năng
        if (pickFromOwned)
        {
            // Lấy ngẫu nhiên từ danh sách ĐÃ SỞ HỮU
            int randomIndex = Random.Range(0, ownedAvailableSkills.Count);
            return ownedAvailableSkills[randomIndex];
        }
        else
        {
            // Lấy ngẫu nhiên từ danh sách ĐẦY ĐỦ
            // (Cũng xử lý trường hợp ownedAvailableSkills.Count == 0)
            int randomIndex = Random.Range(0, availableSkills.Count);
            return availableSkills[randomIndex];
        }
    }
    // ----------------------------------------------------


    public List<UpgradeData> getRandomUpgrade(int count)
    {

        List<UpgradeData> upgradeStatsAvailable = new List<UpgradeData>(statsUpgrades);
        List<UpgradeData> upgradeSkillAvailable = new List<UpgradeData>(skillUpgrades);
        List<UpgradeData> upgradeNormalAvailable = new List<UpgradeData>(this.normalUpgrades);
        List<UpgradeData> selectedUpgrades = new List<UpgradeData>();
        for (int i = 0; i < count; i++)
        {
            if (upgradeStatsAvailable.Count == 0 && upgradeSkillAvailable.Count == 0)
            {
                break;
            }
            int randomIndex = Random.Range(0, 2);
            if (soLanChonSkill >= 2 && randomIndex == 1)
                randomIndex = 0; // ép chọn stat
            if (soLanChonStat >= 2 && randomIndex == 0)
                randomIndex = 1; // ép chọn skill

            if (randomIndex == 0 && soLanChonStat < 2)
            {

                if (upgradeStatsAvailable.Count != 0)
                {
                    int randomStatIndex = Random.Range(0, upgradeStatsAvailable.Count);
                    selectedUpgrades.Add(upgradeStatsAvailable[randomStatIndex]);
                    upgradeStatsAvailable.RemoveAt(randomStatIndex);
                    soLanChonStat++;
                }
                else
                {
                    int RandomNormalUpgrade = Random.Range(0, upgradeNormalAvailable.Count);
                    selectedUpgrades.Add(upgradeNormalAvailable[RandomNormalUpgrade]);
                    upgradeNormalAvailable.RemoveAt(RandomNormalUpgrade);
                }

            }
            else if (randomIndex == 0 && soLanChonStat >= 2)
            {
                if (upgradeSkillAvailable.Count != 0)
                {
                    // --- MODIFIED: Sử dụng logic ưu tiên ---
                    UpgradeData chosenSkill = GetBiasedSkill(upgradeSkillAvailable);
                    selectedUpgrades.Add(chosenSkill);
                    upgradeSkillAvailable.Remove(chosenSkill); // Xóa theo đối tượng
                    // ------------------------------------
                    soLanChonSkill++;
                }
                else
                {
                    int RandomNormalUpgrade = Random.Range(0, upgradeNormalAvailable.Count);
                    selectedUpgrades.Add(upgradeNormalAvailable[RandomNormalUpgrade]);
                    upgradeNormalAvailable.RemoveAt(RandomNormalUpgrade);
                }
            }
            if (randomIndex == 1 && soLanChonSkill < 2)
            {
                if (upgradeSkillAvailable.Count != 0)
                {
                    // --- MODIFIED: Sử dụng logic ưu tiên ---
                    UpgradeData chosenSkill = GetBiasedSkill(upgradeSkillAvailable);
                    selectedUpgrades.Add(chosenSkill);
                    upgradeSkillAvailable.Remove(chosenSkill); // Xóa theo đối tượng
                    // ------------------------------------
                    soLanChonSkill++;
                }
                else
                {
                    int RandomNormalUpgrade = Random.Range(0, upgradeNormalAvailable.Count);
                    selectedUpgrades.Add(upgradeNormalAvailable[RandomNormalUpgrade]);
                    upgradeNormalAvailable.RemoveAt(RandomNormalUpgrade);
                }
            }
            else if (randomIndex == 1 && soLanChonSkill >= 2)
            {
                if (upgradeStatsAvailable.Count != 0)
                {
                    int randomStatIndex = Random.Range(0, upgradeStatsAvailable.Count);
                    selectedUpgrades.Add(upgradeStatsAvailable[randomStatIndex]);
                    upgradeStatsAvailable.RemoveAt(randomStatIndex);
                    soLanChonStat++;
                }
                else
                {
                    int RandomNormalUpgrade = Random.Range(0, upgradeNormalAvailable.Count);
                    selectedUpgrades.Add(upgradeNormalAvailable[RandomNormalUpgrade]);
                    upgradeNormalAvailable.RemoveAt(RandomNormalUpgrade);
                }
            }
        }
        soLanChonStat = 0;
        soLanChonSkill = 0;
        return selectedUpgrades;
    }

    public void OnClickSelectUpgrades(UpgradeData selectUpgrade)
    {
        int currenCount = soUpgradesDangCo.ContainsKey(selectUpgrade) ? soUpgradesDangCo[selectUpgrade] : 0;

        if (currenCount == selectUpgrade.maxChosse)
        {
            if (skillUpgrades.Contains(selectUpgrade))
            {
                skillUpgrades.Remove(selectUpgrade);
                if (selectUpgrade.upgradePro != null)
                {
                    skillUpgrades.Add(selectUpgrade.upgradePro);
                }
            }
            if (statsUpgrades.Contains(selectUpgrade))
            {
                statsUpgrades.Remove(selectUpgrade);
                if (selectUpgrade.upgradePro != null)
                {
                    statsUpgrades.Add(selectUpgrade.upgradePro);
                }
            }
        }
    }
    public void SelectUpgrade(int index)
    {
        UpgradeData selectedUpgrade = selectUpgrade[index];
        CheckUpgrade(selectedUpgrade);
        OnClickSelectUpgrades(selectedUpgrade);
        // Áp dụng hiệu ứng của nâng cấp đã chọn cho người chơi
        if (selectedUpgrade != null)
        {
            Debug.Log("Selected Upgrade: " + selectedUpgrade.upgradeName);
            // Thêm logic áp dụng nâng cấp cho người chơi ở đây
            if (selectedUpgrade.upgradeName == "+1 Speed")
            {
                playerController.speed += 1f;
            }
            else if (selectedUpgrade.upgradeName == "+25 Max HP")
            {
                playerController.thanhmauToiDa += 25f;
                playerController.thanhmauhientai += 25f;
                playerController.thanhmau.capnhatthanhmau(playerController.thanhmauhientai, playerController.thanhmauToiDa);
            }
            else if (selectedUpgrade.upgradeName == "+20% Damage")
            {
                playerController.damecongthem += 0.2f;
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
            else if (selectedUpgrade.upgradeName == "+1 Shuriken")
            {
                spawnSuriken.ThemSuriken();
            }
            else if (selectedUpgrade.upgradeName == "Ultimate Shuriken")
            {
                spawnSuriken.NangCapCuoi();
            }
            else if (selectedUpgrade.upgradeName == "Hạm Đội Tuần Tra")
            {

                if (spawnTauTuanTra != null)
                {
                    if (soUpgradesDangCo[selectedUpgrade] > 1)
                    {
                        spawnTauTuanTra.NangCapTauTuanTra();
                    }
                    else { spawnTauTuanTra.StartBatDauTauTuanTra(); }
                }
            }
            else if (selectedUpgrade.upgradeName == "Đội Hình Song Hành")
            {

                spawnTauTuanTra.soTau += 1;
            }
            else if (selectedUpgrade.upgradeName == "+20 HP")
            {
                playerController.thanhmauhientai += 20f;
                if (playerController.thanhmauhientai > playerController.thanhmauToiDa)
                {
                    playerController.thanhmauhientai = playerController.thanhmauToiDa;
                }
                playerController.thanhmau.capnhatthanhmau(playerController.thanhmauhientai, playerController.thanhmauToiDa);
            }
            else if (selectedUpgrade.upgradeName == "+10 Star")
            {
                playerController.CongDiem(10);

                playerController.damegoc += 10;
            }
            else if (selectedUpgrade.upgradeName == "Aura Farming")
            {
                if (soUpgradesDangCo[selectedUpgrade] > 1)
                {
                    GameObject auraManagementObj = GameObject.FindGameObjectWithTag("AuraManagement");
                    AuraManagement auraManagement = auraManagementObj.GetComponent<AuraManagement>();
                    auraManagement.GetComponentInChildren<Aura>().NangCapAura();

                }
                else
                {
                    GameObject auraManagementObj = GameObject.FindGameObjectWithTag("AuraManagement");
                    AuraManagement auraManagement = auraManagementObj.GetComponent<AuraManagement>();
                    auraManagement.KichHoatAura();
                }
            }
            else if (selectedUpgrade.upgradeName == "Hào Quang Niết Bàn")
            {
                GameObject auraManagementObj = GameObject.FindGameObjectWithTag("AuraManagement");
                AuraManagement auraManagement = auraManagementObj.GetComponent<AuraManagement>();
                auraManagement.GetComponentInChildren<Aura>().NangCapCuoiAura();
            }
            else if (selectedUpgrade.upgradeName == "Hỏa Lực Tăng Cường")
            {
                spawndan spawnDan = playerController.GetComponentInChildren<spawndan>();
                spawnDan.NangCapThemDan();
            }
            else if (selectedUpgrade.upgradeName == "Phán Quyết Cuối Cùng")
            {
                spawndan spawnDan = playerController.GetComponentInChildren<spawndan>();
                spawnDan.NangCapCuoi();
            }
            else if (selectedUpgrade.upgradeName == "Thiên Phạt")
            {
                if (soUpgradesDangCo[selectedUpgrade] > 1)
                {
                    SpawnKyNangThienThach spawnKyNangThienThach = playerController.GetComponentInChildren<SpawnKyNangThienThach>();
                    spawnKyNangThienThach.NangCap();

                }
                else
                {
                    GameObject spawn = GameObject.FindGameObjectWithTag("SpawnThienThachManagement");
                    SpawnThienThachManagement spawnThienThachManagement = spawn.GetComponent<SpawnThienThachManagement>();
                    spawnThienThachManagement.BatSkill();
                }
            }
            else if (selectedUpgrade.upgradeName == "Vũ Điệu Hủy Diệt")
            {
                SpawnKyNangThienThach spawnKyNangThienThach = playerController.GetComponentInChildren<SpawnKyNangThienThach>();
                spawnKyNangThienThach.NangCapCuoi();
            }
            else if (selectedUpgrade.upgradeName == "Trợ Thủ Tinh Anh") // Tên bạn đặt cho skill
            {
                if (soUpgradesDangCo[selectedUpgrade] > 1)
                {
                    // Nâng cấp các lần sau
                    playerController.NangCapTroThu();
                }
                else
                {
                    // Lần đầu tiên chọn
                    playerController.KichHoatTroThu();
                }
            }
            else if (selectedUpgrade.upgradeName == "Song Sinh Sát Thủ") // Tên nâng cấp Pro
            {
                playerController.KichHoatTroThuCuoi();
            }

            else
            {
                Debug.LogWarning("Upgrade not recognized: " + selectedUpgrade.upgradeName);
            }


            panelUpgrade.SetActive(false);
            Time.timeScale = 1f;

        }
    }
}
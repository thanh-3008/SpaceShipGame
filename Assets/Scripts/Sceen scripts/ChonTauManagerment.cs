using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ChonTau : MonoBehaviour
{
    private const int LOADING_SCENE_INDEX = 4; // Index của Loading Scene

    public ThongTinTau[] ThongTinTau;
    private int currentShipIndex;
    private int totalCurrency;

    [Header("UI Tàu")]
    public Image HinhAnhTau;
    public TextMeshProUGUI NameTau;
    public TextMeshProUGUI DescriptionTau;
    public Image HinhAnhSkill1;
    public Image HinhAnhSkill2;
    public TextMeshProUGUI MoTaSkill1;
    public TextMeshProUGUI MoTaSkill2;
    public TextMeshProUGUI NameSkill1;
    public TextMeshProUGUI NameSkill2;

    [Header("Panels")]
    public GameObject selectionPanel;

    // --- THÊM MỚI: UI Cửa hàng ---
    [Header("UI Cửa Hàng")]
    public TextMeshProUGUI totalCurrencyText; // Text hiển thị tổng tiền
    public TextMeshProUGUI shipCostText;      // Text hiển thị giá tàu
    public GameObject selectButton;           // Nút "Chọn"
    public GameObject buyButton;              // Nút "Mua"
    // ---------------------------------


    void Awake()
    {
        selectionPanel.SetActive(true);
    }

    void Start()
    {
        currentShipIndex = 0;

        // --- CẬP NHẬT: Xử lý tiền và mở khóa tàu ---

        // Đảm bảo tàu đầu tiên (index 0) luôn được mở khóa
        PlayerPrefs.SetInt("ShipUnlocked_0", 1); // 1 = đã mở khóa
        PlayerPrefs.Save();

        // Lấy tổng tiền
        totalCurrency = PlayerPrefs.GetInt("TotalCurrency", 0);
        totalCurrencyText.text = totalCurrency.ToString(); // Hiển thị tiền

        // -----------------------------------------

        if (ThongTinTau != null && ThongTinTau.Length > 0)
        {
            HienThiChonTau();
        }
        else
        {
            Debug.LogWarning("Chưa gán dữ liệu cho ThongTinTau!");
        }
    }

    public void NextShip()
    {
        currentShipIndex++;
        if (currentShipIndex >= ThongTinTau.Length)
        {
            currentShipIndex = 0;
        }
        HienThiChonTau();
    }

    public void PreviousShip()
    {
        currentShipIndex--;
        if (currentShipIndex < 0)
        {
            currentShipIndex = ThongTinTau.Length - 1;
        }
        HienThiChonTau();
    }

    // --- HÀM NÀY ĐƯỢC VIẾT LẠI HOÀN TOÀN ---
    public void HienThiChonTau()
    {
        if (ThongTinTau == null || ThongTinTau.Length == 0) return;
        if (currentShipIndex < 0 || currentShipIndex >= ThongTinTau.Length) return;

        ThongTinTau tauhientai = ThongTinTau[currentShipIndex];

        // 1. Cập nhật thông tin tàu (như cũ)
        HinhAnhTau.sprite = tauhientai.Sprite;
        NameTau.text = tauhientai.Name;
        DescriptionTau.text = tauhientai.Description;
        MoTaSkill1.text = tauhientai.DescriptionSkill1;
        MoTaSkill2.text = tauhientai.DescriptionSkill2;
        NameSkill1.text = tauhientai.NameSkill1;
        NameSkill2.text = tauhientai.NameSkill2;
        HinhAnhSkill1.sprite = tauhientai.spiteskill1;
        HinhAnhSkill2.sprite = tauhientai.spiteskill2;

        // 2. Cập nhật UI tiền
        totalCurrency = PlayerPrefs.GetInt("TotalCurrency", 0);
        totalCurrencyText.text = totalCurrency.ToString();

        // 3. Kiểm tra trạng thái Mở khóa (Unlock)
        // Dùng PlayerPrefs với key "ShipUnlocked_INDEX"
        // Giá trị 0 = khóa, 1 = mở
        bool isUnlocked = PlayerPrefs.GetInt("ShipUnlocked_" + currentShipIndex, 0) == 1;

        if (isUnlocked)
        {
            // TÀU ĐÃ MỞ KHÓA
            selectButton.SetActive(true);
            buyButton.SetActive(false);
            shipCostText.text = "ĐÃ SỞ HỮU";
        }
        else
        {
            // TÀU ĐANG BỊ KHÓA
            selectButton.SetActive(false);
            buyButton.SetActive(true);
            shipCostText.text = "Giá: " + tauhientai.cost.ToString();

            // Lấy component Button của nút Mua
            Button buyBtnComponent = buyButton.GetComponent<Button>();
            if (buyBtnComponent != null)
            {
                // Kiểm tra xem có đủ tiền mua không
                if (totalCurrency >= tauhientai.cost)
                {
                    // Đủ tiền -> Cho phép nhấn nút Mua
                    buyBtnComponent.interactable = true;
                }
                else
                {
                    // Không đủ tiền -> Tắt nút Mua (màu xám)
                    buyBtnComponent.interactable = false;
                }
            }
        }
    }

    // --- THÊM MỚI: Hàm cho nút MUA ---
    // Gán hàm này vào onClick của buyButton
    public void BuyShip()
    {
        ThongTinTau tauDeMua = ThongTinTau[currentShipIndex];
        int cost = tauDeMua.cost;

        // Kiểm tra lại xem có đủ tiền không
        if (totalCurrency >= cost)
        {
            // 1. Trừ tiền
            totalCurrency -= cost;
            PlayerPrefs.SetInt("TotalCurrency", totalCurrency);

            // 2. Mở khóa tàu
            PlayerPrefs.SetInt("ShipUnlocked_" + currentShipIndex, 1);
            PlayerPrefs.Save();

            // 3. Cập nhật lại toàn bộ UI
            Debug.Log("Mua thành công tàu: " + tauDeMua.Name);
            HienThiChonTau();
        }
        else
        {
            Debug.LogWarning("Không đủ tiền!");
        }
    }

    public void BackMenu()
    {
        LoadingScreen.Next_Scene = 0;
        SceneManager.LoadScene(LOADING_SCENE_INDEX);
    }

    // --- CẬP NHẬT: Hàm cho nút CHỌN ---
    // Gán hàm này vào onClick của selectButton
    public void SelectShip()
    {
        // Kiểm tra lần cuối xem tàu này đã mở khóa chưa
        // (Mặc dù nút Mua đã ẩn, nhưng kiểm tra vẫn an toàn hơn)
        bool isUnlocked = PlayerPrefs.GetInt("ShipUnlocked_" + currentShipIndex, 0) == 1;

        if (isUnlocked)
        {
            PlayerPrefs.SetInt("SelectedShipIndex", currentShipIndex);
            PlayerPrefs.Save();
            Debug.Log("Selected ship: " + ThongTinTau[currentShipIndex].Name);

            LoadingScreen.Next_Scene = 1;
            SceneManager.LoadScene(LOADING_SCENE_INDEX);
        }
        else
        {
            Debug.LogError("LỖI: Đang cố chọn tàu chưa mở khóa!");
        }
    }
}
using TMPro;
using UnityEngine;

public class DamePopUpGenerator : MonoBehaviour
{
    public static DamePopUpGenerator Instance;
    public GameObject PopUpPrefab;

    public void Awake()
    {
        Instance = this;
    }

    // << THAY ĐỔI 1: Thêm tham số 'bool isCrit'
    public void CreatePopUp(Vector3 position, float damage, bool isCrit)
    {
        int damageValue = Mathf.RoundToInt(damage);
        Color textColor;
        float scaleMultiplier;

        // << THAY ĐỔI 2: Đơn giản hóa logic
        if (isCrit)
        {
            // Nếu là chí mạng
            textColor = Color.red; // Màu đỏ nổi bật
            scaleMultiplier = 1.5f; // To hơn bình thường
        }
        else
        {
            // Nếu là sát thương thường
            textColor = Color.white;
            scaleMultiplier = 1.0f; // Kích thước bình thường
        }

        Internal_CreatePopUp(position, damageValue.ToString(), textColor, scaleMultiplier);
    }

    // ---- HÀM CHO HỒI MÁU ----
    public void CreatePopUpHeal(Vector3 position, float healAmount)
    {
        int healValue = Mathf.RoundToInt(healAmount);
        Internal_CreatePopUp(position, "+" + healValue.ToString(), Color.green, 1.2f);
    }

    // ... (Các hàm khác giữ nguyên) ...
    #region Other Popup Types and Internal Creation
    // ---- HÀM CHO MẤT MÁU ----
    public void CreateHealthLossPopUp(Vector3 position, float lossAmount)
    {
        int lossValue = Mathf.RoundToInt(lossAmount);
        Internal_CreatePopUp(position, lossValue.ToString(), Color.magenta, 1.0f);
    }

    // ---- HÀM DÙNG CHUNG ĐỂ TẠO POPUP ----
    private void Internal_CreatePopUp(Vector3 position, string text, Color color, float scaleMultiplier)
    {
        // 1. Tạo pop-up
        GameObject popUp = Instantiate(PopUpPrefab, position, Quaternion.identity);

        // 2. Gán text và màu
        var temp = popUp.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        temp.text = text;
        temp.color = color;

        // 3. Tính toán kích thước mong muốn
        Vector3 desiredScale = Vector3.one * 0.03f * scaleMultiplier;

        // 4. Lấy script animation và truyền trực tiếp giá trị vào
        DamagePopUpAnimation animationScript = popUp.GetComponent<DamagePopUpAnimation>();
        if (animationScript != null)
        {
            animationScript.Initialize(desiredScale);
        }
    }
    #endregion
}
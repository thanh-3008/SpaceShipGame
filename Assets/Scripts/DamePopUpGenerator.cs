// File: DamePopUpGenerator.cs
using TMPro;
using UnityEngine;

public class DamePopUpGenerator : MonoBehaviour
{
    public static DamePopUpGenerator Instance;
    public GameObject PopUpPrefab;

    // XÓA BIẾN NÀY ĐI, NÓ LÀ NGUỒN GỐC CỦA LỖI
    // public Vector3 originScaleDame;

    public void Awake()
    {
        Instance = this;
    }

    // XÓA HÀM START() ĐI, KHÔNG CẦN NỮA

    // ---- HÀM CHO SÁT THƯƠNG ----
    public void CreatePopUp(Vector3 position, float damage)
    {
        int damageValue = Mathf.RoundToInt(damage);
        Color textColor;
        float scaleMultiplier = 1f;

        if (damageValue < 500)
        {
            textColor = Color.white;
            scaleMultiplier = 1f;
        }
        else if (damageValue < 3000)
        {
            textColor = Color.yellow;
            scaleMultiplier = 1.5f;
        }
        else if (damageValue < 10000)
        {
            textColor = new Color(1.0f, 0.64f, 0.0f); // Cam
            scaleMultiplier = 2.2f;
        }
        else
        {
            textColor = Color.red;
            scaleMultiplier = 3f;
        }

        Internal_CreatePopUp(position, damageValue.ToString(), textColor, scaleMultiplier);
    }

    // ---- HÀM CHO HỒI MÁU ----
    public void CreatePopUpHeal(Vector3 position, float healAmount)
    {
        int healValue = Mathf.RoundToInt(healAmount);
        Internal_CreatePopUp(position, healValue.ToString(), Color.green, 1.0f);
    }

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
        Vector3 desiredScale = Vector3.one * 0.03f * scaleMultiplier; // Giữ lại base size 0.03f của bạn

        // 4. Lấy script animation và truyền trực tiếp giá trị vào
        DamagePopUpAnimation animationScript = popUp.GetComponent<DamagePopUpAnimation>();
        if (animationScript != null)
        {
            animationScript.Initialize(desiredScale);
        }
    }
}
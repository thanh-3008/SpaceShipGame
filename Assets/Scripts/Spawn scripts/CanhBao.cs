// File: CanhBao.cs
using UnityEngine;
using System.Collections;

public class CanhBao : MonoBehaviour
{
    private CanvasGroup panelCanhBao;
    private bool dangChay = false; // Biến cờ để chống lỗi

    void Awake()
    {
        panelCanhBao = GetComponent<CanvasGroup>();
        if (panelCanhBao != null)
        {
            panelCanhBao.alpha = 0; // Ẩn panel khi bắt đầu
        }
    }

    // Hàm này được gọi từ script SpawnTauMe
    public void ActiveStartCanhBao()
    {
        // Phải dùng StartCoroutine để gọi một IEnumerator
        // và kiểm tra xem nó có đang chạy không
        if (!dangChay)
        {
            StartCoroutine(ThucHienNhay());
        }
    }

    private IEnumerator ThucHienNhay()
    {
        dangChay = true;

        float tongThoiGian = 3f;
        float tocDoNhay = 0.5f;

        // Dùng biến cục bộ 'timer' thay cho 'bodem' để an toàn hơn
        for (float timer = 0; timer < tongThoiGian; timer += tocDoNhay)
        {
            panelCanhBao.alpha = 1;
            yield return new WaitForSeconds(tocDoNhay / 2);
            panelCanhBao.alpha = 0;
            yield return new WaitForSeconds(tocDoNhay / 2);
        }

        panelCanhBao.alpha = 0;
        dangChay = false;
    }
}
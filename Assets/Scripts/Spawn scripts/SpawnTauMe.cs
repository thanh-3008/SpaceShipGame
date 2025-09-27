// File: SpawnTauMe.cs
using UnityEngine;

public class SpawnTauMe : MonoBehaviour
{
    // Kéo các đối tượng vào đây trong Inspector
    public GameObject tauMePrefab;
    public PlayerController player;
    public CanhBao canhBaoUI; // Thay thế CanvasGroup bằng CanhBao

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Spawn();
        }
    }

    public void Spawn()
    {
        // Không cần GetComponent nữa vì đã có tham chiếu trực tiếp
        if (player == null)
        {
            Debug.LogError("Chưa gán Player vào script SpawnTauMe!");
            return;
        }

        // LỖI LOGIC Ở ĐÂY: Phải so sánh Nộ với Nộ, không phải Nộ với Máu
        if (player.thanhNoHienTai >= player.thanhNoToiDa)
        {
            // Tạo tàu mẹ
            Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y, 0);
            Instantiate(tauMePrefab, spawnPosition, Quaternion.identity);

            // Reset thanh nộ
            player.thanhNoHienTai = 0f;

            // Kích hoạt cảnh báo
            if (canhBaoUI != null)
            {
                canhBaoUI.ActiveStartCanhBao();
            }
            else
            {
                Debug.LogError("Chưa gán Panel Cảnh Báo vào script SpawnTauMe!");
            }
        }
    }
}
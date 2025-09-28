using UnityEngine;

public class SpawnDanIon : MonoBehaviour
{
    // Kéo các đối tượng vào đây trong Inspector
    public GameObject DanIon;
    public PlayerController player;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
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
        if (player.thanhNoHienTai >= player.thanhNoToiDa )
        {
            // Tạo dan ion
            Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            Instantiate(DanIon, spawnPosition, Quaternion.identity);

            // Reset thanh nộ
            player.thanhNoHienTai -= 100f;          
        }
    }
}
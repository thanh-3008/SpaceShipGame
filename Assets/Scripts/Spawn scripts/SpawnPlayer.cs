using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{
    public ThongTinTau[] ThongTinTau;   // danh sách prefab tàu
    public Transform spawnPoint;        // vị trí spawn

    void Start()
    {
        // Lấy lại tàu đã chọn từ PlayerPrefs
        int selectedIndex = PlayerPrefs.GetInt("SelectedShipIndex", 0);

        // Spawn prefab của tàu đã chọn
        if (selectedIndex >= 0 && selectedIndex < ThongTinTau.Length)
        {
            Instantiate(ThongTinTau[selectedIndex].Tau, spawnPoint.position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Không tìm thấy tàu được chọn, spawn tàu mặc định.");
            Instantiate(ThongTinTau[0].Tau, spawnPoint.position, Quaternion.identity);
        }
    }
}

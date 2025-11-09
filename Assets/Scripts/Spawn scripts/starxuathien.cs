using UnityEngine;

public class starxuathien : MonoBehaviour
{
    public GameObject star;
    public GameObject buffTenLuaPrefab; // THÊM MỚI: Kéo prefab buff tên lửa vào đây
    public float timer = 5f;

    [Tooltip("Tỉ lệ xuất hiện buff tên lửa (ví dụ: 0.1 = 10%)")]
    [Range(0f, 1f)]
    public float tiLeBuffTenLua = 0.1f; // THÊM MỚI: 10% cơ hội ra buff tên lửa

    void Start()
    {
        // (Không cần gì ở đây)
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            // 1. Tính toán vị trí xuất hiện trước
            Vector3 spawnPosition = new Vector3(Random.Range(-6f, 6f), transform.position.y, 0f);

            // 2. Quyết định xem nên spawn gì
            if (Random.value < tiLeBuffTenLua) // Random.value trả về một số từ 0.0 đến 1.0
            {
                // Nếu số ngẫu nhiên < 0.1 (tức là 10% cơ hội)
                // -> Spawn Buff Tên Lửa
                Instantiate(buffTenLuaPrefab, spawnPosition, Quaternion.identity);
            }
            else
            {
                // Ngược lại (90% cơ hội còn lại)
                // -> Spawn Star (Ngôi sao)
                Instantiate(star, spawnPosition, Quaternion.identity);
            }

            // 3. Reset timer
            timer = 5f;
        }
    }
}
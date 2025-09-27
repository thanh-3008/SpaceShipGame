using System.Collections;
using UnityEngine;

public class spawndanpro : MonoBehaviour
{
    public GameObject danPrefab; // Đổi tên cho rõ nghĩa
    public float spawnInterval = 0.1f; // Khoảng cách giữa mỗi lần bắn

    private Coroutine spawningCoroutine; // Để theo dõi coroutine đang chạy
    public GameObject Audio;

    // Hàm này sẽ được PlayerController gọi để bắt đầu hiệu ứng
    public void ActivateBuff(float duration)
    {
        // Nếu đang có buff cũ, hủy nó đi để bắt đầu buff mới
        if (spawningCoroutine != null)
        {
            StopCoroutine(spawningCoroutine);
        }

        // Bắt đầu coroutine bắn đạn và lưu nó lại
        spawningCoroutine = StartCoroutine(SpawnRoutine(duration));
    }

    // Coroutine: Logic chính của việc bắn đạn trong một khoảng thời gian
    private IEnumerator SpawnRoutine(float duration)
    {
        float timer = 0f;

        // Vòng lặp bắn đạn cho đến khi hết thời gian (duration)
        while (timer < duration)
        {
            AudioManagement audioManager = Audio.GetComponent<AudioManagement>();
            audioManager.PlaySfx(audioManager.tiengdanpro); // Phát âm thanh bắn đạn
            // Tạo ra viên đạn tại vị trí của player
            Instantiate(danPrefab, transform.position, danPrefab.transform.rotation);

            // Đợi một chút trước khi bắn viên tiếp theo
            yield return new WaitForSeconds(spawnInterval);

            // Cập nhật thời gian đã trôi qua
            timer += spawnInterval;
        }

        // Hết thời gian, coroutine kết thúc và việc bắn đạn dừng lại
        spawningCoroutine = null;
    }
}
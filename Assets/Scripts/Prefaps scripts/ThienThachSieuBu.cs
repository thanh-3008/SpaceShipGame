using UnityEngine;

public class ThienThachSieuBu : MonoBehaviour
{
    private spawnthienthach spawn; // Vẫn giữ biến này, nhưng không cần kéo thả trong Editor
    private thienthachdichuyen thienthachScript;
    void Start()
    {
        // Tự động tìm đối tượng duy nhất trong Scene có script "spawnthienthach"
        spawn = FindObjectOfType<spawnthienthach>();
        thienthachScript = FindAnyObjectByType<thienthachdichuyen>();
    }

    public void OnDestroy()
    {
        // Kiểm tra để chắc chắn là đã tìm thấy trước khi gọi
        if (spawn != null)
        {
            spawn.Resumetime();
           
        }
    }
}
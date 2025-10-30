using UnityEngine;

// GẮN SCRIPT NÀY VÀO PREFAB "hieuungno" CỦA BẠN
public class huyHieuUngNo : MonoBehaviour
{
    [Tooltip("Thời gian tồn tại của hiệu ứng (giây). Hãy chỉnh cho khớp với thời gian chạy animation nổ.")]
    public float lifetime = 2.0f;

    void Start()
    {
        // Tự động gọi hàm Destroy(gameObject) sau 'lifetime' giây
        Destroy(gameObject, lifetime);
    }
}


using System.Collections;
using UnityEngine;
public class SuperBoss_HolyGround : MonoBehaviour
{
    public float warningTime = 2.0f;
    public GameObject warningVisual; // Kéo Sprite "Vòng tròn cảnh báo" vào đây
    public GameObject explosionCollider; // Kéo "Explosion_Trigger" (con) vào đây

    void Start()
    {
        if (explosionCollider == null || warningVisual == null)
        {
            Debug.LogError("Holy Ground Prefab thiếu Visual hoặc Collider!");
            Destroy(gameObject);
            return;
        }
        explosionCollider.SetActive(false);
        warningVisual.SetActive(true);
        StartCoroutine(Explode());
    }

    IEnumerator Explode()
    {
        yield return new WaitForSeconds(warningTime); // Chờ 2s

        warningVisual.SetActive(false); // Ẩn cảnh báo
        explosionCollider.SetActive(true);

        yield return new WaitForSeconds(0.4f); // Cho va chạm 0.2s để ghi nhận
        Destroy(gameObject); // Tự hủy
    }
}
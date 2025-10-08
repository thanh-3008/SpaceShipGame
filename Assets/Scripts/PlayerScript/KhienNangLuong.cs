using System.Collections;
using UnityEngine;

public class KhienNangLuong : MonoBehaviour
{
    // Tạo một ô để kéo nhân vật vào
    [Tooltip("Kéo vật thể nhân vật của bạn vào đây")]
    public Transform characterTransform;

    private float currentRotationSpeed = 20f;

    private float timer = 0f;

    public SpriteRenderer spriteRenderer;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        startnhapnhau();
    }
    void Update()
    {     

        // --- PHẦN CODE MỚI ---
        // Đảm bảo khiên luôn ở đúng vị trí của nhân vật
        if (characterTransform != null)
        {
            transform.position = characterTransform.position;
        }
        // ----------------------
        transform.Rotate(0, 0, currentRotationSpeed * Time.deltaTime);
    }
    public void startnhapnhau()
    {
        StopCoroutine(nhapnhay());
        StartCoroutine(nhapnhay());
    }
    public IEnumerator nhapnhay()
    {
        while (timer <= 2f)
        {
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(0.25f);
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(0.25f);
            timer += 0.5f;
        }

    }
}
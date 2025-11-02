using UnityEngine;

public class Aura : MonoBehaviour
{
    public float damageRate = 7f; // Số lần gây sát thương mỗi giây
    private float nextDamageTime = 0f; // Thời điểm được phép gây sát thương tiếp theo
    public PlayerController playerController;
    public Vector3 scaleGoc;
    public SpriteRenderer spriteColor;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scaleGoc = transform.localScale;
        // Gán màu ban đầu (nếu muốn)
        if (spriteColor != null)
        {
            spriteColor.color = new Color(1f, 1f, 1f, 0.4f); // Màu trắng với alpha 0.4
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Đảm bảo playerController đã được gán
        if (playerController != null)
        {
            transform.position = playerController.transform.position;
        }
    }

    public void NangCapAura()
    {
        scaleGoc += new Vector3(3f, 3f, 0);
        transform.localScale = scaleGoc;
    }

    public void NangCapCuoiAura()
    {
        scaleGoc += new Vector3(5.5f, 5.5f, 0);
        transform.localScale = scaleGoc;
        damageRate = 12f;

        if (spriteColor != null)
        {
            spriteColor.color = new Color(0f, 1f, 0f, 0.4f); // Màu xanh lá cây với alpha 0.4
        }
    }

    public void OnTriggerStay2D(Collider2D other)
    {
        // 1. Kiểm tra thời gian trước tiên
        // Nếu chưa đến lúc gây sát thương, thoát ngay lập tức
        if (Time.time < nextDamageTime)
        {
            return;
        }

        // 2. Nếu đã đến lúc, tính sát thương (chỉ 1 lần)
        // Đảm bảo playerController tồn tại trước khi dùng
        if (playerController == null)
        {
            Debug.LogWarning("PlayerController chưa được gán trên Aura!");
            return;
        }
        var damageResult = playerController.CalculateDamage();
        bool targetWasHit = false; // Biến để kiểm tra xem có trúng mục tiêu không

        // 3. Thử gây sát thương cho các loại mục tiêu
        if (other.CompareTag("Enemy"))
        {
            thienthachdichuyen thienthach = other.GetComponent<thienthachdichuyen>();
            if (thienthach != null)
            {
                thienthach.TakeDame(damageResult.damage);
                targetWasHit = true;
            }
        }
        else if (other.CompareTag("Boss"))
        {
            BossController boss = other.GetComponent<BossController>();
            if (boss != null)
            {
                boss.TakeDame(damageResult.damage);
                targetWasHit = true;
            }
        }
        else if (other.CompareTag("Monster"))
        {
            RatMonster monster = other.GetComponent<RatMonster>();
            if (monster != null)
            {
                monster.TakeDame(damageResult.damage);
                targetWasHit = true;
            }
        }

        // 4. Nếu đã trúng bất kỳ mục tiêu hợp lệ nào
        if (targetWasHit)
        {
            Debug.Log("Aura deals damage to " + other.name);

            // Cập nhật thời điểm gây sát thương tiếp theo
            nextDamageTime = Time.time + 1f / damageRate;

            // ***** SỬA LỖI CHÍNH *****
            // Tạo popup tại vị trí của 'other' (kẻ thù), không phải 'transform.position' (người chơi)
            if (DamePopUpGenerator.Instance != null)
            {
                DamePopUpGenerator.Instance.CreatePopUp(other.transform.position, damageResult.damage, damageResult.isCrit);
            }
            else
            {
                Debug.LogWarning("DamePopUpGenerator.Instance không tồn tại!");
            }
        }
    }
}
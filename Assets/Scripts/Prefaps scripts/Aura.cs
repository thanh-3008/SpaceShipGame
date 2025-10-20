using UnityEngine;

public class Aura : MonoBehaviour
{
    public float damageRate = 7f; // Số lần gây sát thương mỗi giây (2f = 2 lần/giây)
    private float nextDamageTime = 0f; // Thời điểm được phép gây sát thương tiếp theo
    public PlayerController playerController;
    public Vector3 scaleGoc;
    public SpriteRenderer spriteColor;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scaleGoc = transform.localScale;
        spriteColor.color = new Color(1f, 1f, 1f, 0.4f); // Màu trắng với alpha 0.5
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = playerController.transform.position;
        
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
        spriteColor.color = new Color(0f, 1f, 0f, 0.4f); // Màu xanh lá cây với alpha 0.5
    }

    public void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Chỉ gây sát thương nếu thời gian hiện tại đã vượt qua thời điểm cho phép
            if (Time.time >= nextDamageTime)
            {
                Debug.Log("Laser deals damage to " + other.name);

                // Cập nhật thời điểm gây sát thương tiếp theo
                nextDamageTime = Time.time + 1f / damageRate;

                // Lấy component và gây sát thương
                thienthachdichuyen thienthach = other.GetComponent<thienthachdichuyen>();
                if (thienthach != null)
                {
                    var damageResult = playerController.CalculateDamage();
                    // Gợi ý: Tên hàm nên là "TakeDamage" để dễ đọc hơn
                    thienthach.TakeDame(damageResult.damage );
                    DamePopUpGenerator.Instance.CreatePopUp(transform.position, damageResult.damage , damageResult.isCrit);
                }
            }

        }
        if (other.CompareTag("Boss"))
        {
            // Chỉ gây sát thương nếu thời gian hiện tại đã vượt qua thời điểm cho phép
            if (Time.time >= nextDamageTime)
            {
                Debug.Log("Laser deals damage to " + other.name);

                // Cập nhật thời điểm gây sát thương tiếp theo
                nextDamageTime = Time.time + 1f / damageRate;

                // Lấy component và gây sát thương
                BossController boss = other.GetComponent<BossController>();
                if (boss != null)
                {
                    var damageResult = playerController.CalculateDamage();
                    // Gợi ý: Tên hàm nên là "TakeDamage" để dễ đọc hơn
                    boss.TakeDame(damageResult.damage );
                    DamePopUpGenerator.Instance.CreatePopUp(transform.position, damageResult.damage , damageResult.isCrit);
                }
            }
        }
        if (other.CompareTag("Monster"))
        {
            // Chỉ gây sát thương nếu thời gian hiện tại đã vượt qua thời điểm cho phép
            if (Time.time >= nextDamageTime)
            {
                Debug.Log("Laser deals damage to " + other.name);

                // Cập nhật thời điểm gây sát thương tiếp theo
                nextDamageTime = Time.time + 1f / damageRate;

                // Lấy component và gây sát thương
                RatMonster boss = other.GetComponent<RatMonster>();
                if (boss != null)
                {
                    var damageResult = playerController.CalculateDamage();
                    // Gợi ý: Tên hàm nên là "TakeDamage" để dễ đọc hơn
                    boss.TakeDame(damageResult.damage);
                    DamePopUpGenerator.Instance.CreatePopUp(transform.position, damageResult.damage, damageResult.isCrit);
                }
            }
        }
    }
}

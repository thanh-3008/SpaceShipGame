using Unity.VisualScripting;
using UnityEngine;

public class spawnthienthach : MonoBehaviour
{
    public GameObject[] gameObjects; // Array of GameObjects to spawn
    public float spawnInterval = 2f; // Time interval between spawns
    private float timer; // Timer to track time since last spawn
    public GameObject thienthachsieubu;
    private bool timestop = false;
    private float i;

    [Header("thiết lập độ khó")]
    public float thoigiantangdokho = 30f; // Interval to increase difficulty
    private float thoigiandaqua; // Sẽ đếm tổng thời gian chơi

    void Awake()
    {
        timer = spawnInterval; // Initialize timer to spawn immediately
    }

    void Update()
    {
        // CÁC BỘ ĐẾM THỜI GIAN NÊN ĐƯỢC CẬP NHẬT Ở ĐÂY
        timer += Time.deltaTime;
        i += Time.deltaTime;
        thoigiandaqua += Time.deltaTime; // <<< ĐÃ DI CHUYỂN LÊN ĐÂY ĐỂ ĐẾM CHO ĐÚNG

        // Kiểm tra để tạo thiên thạch
        if (timer >= spawnInterval && timestop == false)
        {
            
            int randomIndex = Random.Range(0, gameObjects.Length);
            Vector2 spawnPosition = new Vector2(Random.Range(-6f, 6f), transform.position.y);
            // Dòng dưới đây không cần thiết nếu spawner đứng yên, nhưng tôi giữ lại theo code của bạn
            transform.position = spawnPosition;
            GameObject newobject = Instantiate(gameObjects[randomIndex], transform.position, Quaternion.identity);
            thienthachdichuyen thienthachScript = newobject.GetComponent<thienthachdichuyen>();
            // --- LOGIC TĂNG ĐỘ KHÓ NÊN ĐƯỢC ĐẶT Ở ĐÂY ---

            float tangdokho = 1f + (thoigiandaqua / thoigiantangdokho);

            float tocdohientai = thienthachScript.speed * tangdokho/4;
            float mautoidahientai = thienthachScript.thanhmauToiDa * tangdokho;
            float damethienthachhientai = thienthachScript.dame * tangdokho / 4;
          
            if (thienthachScript != null)
            {

                thienthachScript.speed = tocdohientai;
                thienthachScript.thanhmauToiDa = mautoidahientai;
                thienthachScript.dame = damethienthachhientai;
            }
            // --- KẾT THÚC LOGIC TĂNG ĐỘ KHÓ ---

            timer = 0f; // Reset the timer sau khi mọi việc hoàn tất

            // Logic tạo trùm giữ nguyên
            if (i >= 40)
            {
                Vector2 pos = new Vector2(0f, transform.position.y);
                GameObject BossObject = Instantiate(thienthachsieubu, pos, Quaternion.identity);
                thienthachdichuyen thienthachScriptBoss = BossObject.GetComponent<thienthachdichuyen>();
                if (thienthachScriptBoss != null)
                {
                    float tangdokho1 = Mathf.Pow(1.5f, (thoigiandaqua / thoigiantangdokho));
                    thienthachScriptBoss.thanhmauToiDa *= tangdokho1;
                    ; // Boss có nhiều máu hơn
                    thienthachScriptBoss.dame = damethienthachhientai * 2; // Boss
                    timestop = true;
                    i = 0;
                }
            }
          
        }
    }

    public void Resumetime()
    {
        timestop = false;
    }
}
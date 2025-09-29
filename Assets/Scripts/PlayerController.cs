using TMPro;
using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D Rigidbody2D;
    private float traiphai;
    public float speed = 5f;
    private float lenxuong;
    public ThanhMau thanhmau;
    public float thanhmauhientai;
    public float thanhmauToiDa = 100f;
    public TextMeshProUGUI textScore;
    public GameObject danprefap;
    public float damebonus = 2f;
    public float damehientai;
    public TextMeshProUGUI soTenLuaText;
    public GameObject[] spawndan;
    public GameObject Audio;
    public ThanhNo thanhno;
    public float thanhNoToiDa = 100f;
    public float thanhNoHienTai;
    private SpriteRenderer spriteRenderer;
    public float timeFlash;
    private float timer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        thanhmauhientai = thanhmauToiDa;
        damehientai = 5f;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        traiphai = Input.GetAxis("Horizontal");
        Rigidbody2D.linearVelocity = new Vector2(traiphai * speed, Rigidbody2D.linearVelocity.y);

        lenxuong = Input.GetAxis("Vertical");
        Rigidbody2D.linearVelocity = new Vector2(Rigidbody2D.linearVelocity.x, lenxuong * speed);
        if (thanhNoHienTai <= thanhNoToiDa * 3)
        {
            thanhNoHienTai += 2f * Time.deltaTime;
            thanhno.capnhatthanhno(thanhNoHienTai, thanhNoToiDa);
        }
    }
    public void TakeDame(float dame)
    {
        Debug.Log("Player take dame: " + dame);
        thanhmauhientai -= dame;
        thanhmau.capnhatthanhmau(thanhmauhientai, thanhmauToiDa);
        AudioManagement audioManager = Audio.GetComponent<AudioManagement>();
        audioManager.PlaySfxto(audioManager.tiengvacham);
        if (thanhmauhientai <= 0)
        {
            Destroy(gameObject);
            FindObjectOfType<GameOverMenu>().showGameOverScreen(int.Parse(textScore.text));
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        int sotenlua = int.Parse(soTenLuaText.text);
        if (collision.CompareTag("star"))
        {
            int score = int.Parse(textScore.text);
            score += 1;
            textScore.text = score.ToString();
            Dan dan = danprefap.GetComponent<Dan>();
            damehientai += damebonus;
            AudioManagement audioManager = Audio.GetComponent<AudioManagement>();
            audioManager.PlaySfxto(audioManager.tiengancoin); // Phát âm thanh khi nhận sao
            Destroy(collision.gameObject);
        }
        if (collision.CompareTag("bufftenlua") && sotenlua <= 10)
        {
            sotenlua += 1;
            soTenLuaText.text = sotenlua.ToString();
            AudioManagement audioManager = Audio.GetComponent<AudioManagement>();
            audioManager.PlaySfxto(audioManager.tiengancoin);
            Destroy(collision.gameObject);
        }
        if (collision.CompareTag("buffdanpro"))
        {
            // Gọi hàm ActivateBuff trong script spawndanpro và đặt thời gian là 10 giây
            for (int i = 0; i < spawndan.Length; i++)
            {
                GameObject spawner = spawndan[i];
                spawndanpro danproSpawner = spawner.GetComponent<spawndanpro>();
                danproSpawner.ActivateBuff(10f);
                AudioManagement audioManager = Audio.GetComponent<AudioManagement>();
                audioManager.PlaySfxto(audioManager.tiengancoin);
            }

            // Hủy vật phẩm buff
            Destroy(collision.gameObject);
        }      
}
    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("TauMe"))
        {
            if (thanhmauhientai < thanhmauToiDa)
            {
                thanhmauhientai += 3f * Time.deltaTime;
                thanhmau.capnhatthanhmau(thanhmauhientai, thanhmauToiDa);
            }
        }
    }
    public void StartFlashRed()
    {
        StopCoroutine(FlashRed(timeFlash)); // Dừng coroutine nếu nó đang chạy
        StartCoroutine(FlashRed(timeFlash)); // Bắt đầu coroutine mới
    }
    private IEnumerator FlashRed(float thoigianduytri)
    {
        Color originalColor = spriteRenderer.color;
        while (timer < thoigianduytri)
        {          
            spriteRenderer.color = Color.red; // Đổi màu thành đỏ
            yield return new WaitForSeconds(0.2f); 
            spriteRenderer.color = Color.white; // Trả về màu ban đầu}
            yield return new WaitForSeconds(0.2f);
            timer += 0.2f;
        }
        timer = 0f; // Reset timer sau khi hoàn thành
    }
    void LateUpdate()
    {
        // Lấy vị trí hiện tại
        Vector3 currentPosition = transform.position;

        // Kẹp tọa độ X và Y
        float clampedX = Mathf.Clamp(currentPosition.x, -7.5f, 7.5f);
        float clampedY = Mathf.Clamp(currentPosition.y, -4.5f, 4.5f);

        // Cập nhật lại vị trí, giữ nguyên trục Z
        transform.position = new Vector3(clampedX, clampedY, currentPosition.z);
    }
}

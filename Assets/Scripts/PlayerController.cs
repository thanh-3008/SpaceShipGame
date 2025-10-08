using TMPro;
using UnityEngine;
using System.Collections;
using System;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rigidbody2D;
    private float traiphai;
    public float speed = 5f;
    private float lenxuong;
    public bool khoadichuyen = false;

    // Các tham chiếu sẽ được tự động tìm
    public ThanhMau thanhmau;
    public float thanhmauhientai =100f;
    public float thanhmauToiDa = 100f;

    public TextMeshProUGUI textScore;
    public GameObject danprefap;
    public float damebonus = 1f;
    public float damehientai=5f;

    public TextMeshProUGUI soTenLuaText;
    public GameObject[] spawndan;
    public GameObject[] spawndanpro;
    public AudioManagement audioManager;
    public ThanhNo thanhno;

    public float thanhNoToiDa = 100f;
    public float thanhNoHienTai;
    private SpriteRenderer spriteRenderer;
    public float timeFlash=0.8f;
    private float timer=0f;

    public float gocNghiengToiDa = 15f;   // Góc nghiêng tối đa
    public float tocDoNghieng = 20f;      // Tốc độ nghiêng

    public Boolean kimcangbathoai = false;

    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // 🔎 Tự động tìm các object trong Scene
        // Sử dụng Debug.Log để theo dõi quá trình khởi tạo các biến

        // --- Tìm ThanhMau ---
        if (thanhmau == null)
        {
            Debug.Log("Searching for 'ThanhMau' component...");
            GameObject obj = GameObject.Find("ThanhMau");
            if (obj != null)
            {
                thanhmau = obj.GetComponent<ThanhMau>();
                if (thanhmau == null)
                {
                    Debug.LogError("GameObject 'ThanhMau' was found, but it's missing the 'ThanhMau' component.");
                }
                else
                {
                    Debug.Log("'ThanhMau' assigned successfully!");
                }
            }
            else
            {
                Debug.LogError("Could not find GameObject named 'ThanhMau' in the scene.");
            }
        }

        // --- Tìm TextMeshPro cho Score ---
        if (textScore == null)
        {
            Debug.Log("Searching for 'Score' component...");
            GameObject obj = GameObject.Find("Score");
            if (obj != null)
            {
                textScore = obj.GetComponent<TextMeshProUGUI>();
                if (textScore == null)
                {
                    Debug.LogError("GameObject 'Score' was found, but it's missing the 'TextMeshProUGUI' component.");
                }
                else
                {
                    Debug.Log("'Score' text assigned successfully!");
                }
            }
            else
            {
                Debug.LogError("Could not find GameObject named 'Score' in the scene.");
            }
        }

        // --- Tìm TextMeshPro cho SoTenLua ---
        if (soTenLuaText == null)
        {
            Debug.Log("Searching for 'SoTenLua' component...");
            GameObject obj = GameObject.Find("SoTenLua");
            if (obj != null)
            {
                soTenLuaText = obj.GetComponent<TextMeshProUGUI>();
                if (soTenLuaText == null)
                {
                    Debug.LogError("GameObject 'SoTenLua' was found, but it's missing the 'TextMeshProUGUI' component.");
                }
                else
                {
                    Debug.Log("'SoTenLua' text assigned successfully!");
                }
            }
            else
            {
                Debug.LogError("Could not find GameObject named 'SoTenLua' in the scene.");
            }
        }

        // --- Tìm các Spawner ---
        if (spawndan == null || spawndan.Length == 0)
        {
            Debug.Log("Searching for GameObjects with tag 'Spawner'...");
            spawndan = GameObject.FindGameObjectsWithTag("spawndan");
            if (spawndan != null && spawndan.Length > 0)
            {
                Debug.Log($"Found and assigned {spawndan.Length} spawner(s).");
            }
            else
            {
                // Sử dụng LogWarning vì có thể trong một số màn chơi không có spawner
                Debug.LogWarning("No GameObjects with the tag 'Spawner' were found in the scene.");
            }
        }

        if (spawndanpro == null || spawndanpro.Length == 0)
        {
            Debug.Log("Searching for GameObjects with tag 'spawndanpro'...");
            spawndanpro = GameObject.FindGameObjectsWithTag("spawndanpro");
            if (spawndanpro != null && spawndanpro.Length > 0)
            {
                Debug.Log($"Found and assigned {spawndanpro.Length} spawner(s).");
            }
            else
            {
                // Sử dụng LogWarning vì có thể trong một số màn chơi không có spawner
                Debug.LogWarning("No GameObjects with the tag 'Spawner' were found in the scene.");
            }
        }

        // --- Tìm AudioManagement ---
        if (audioManager == null)
        {
            Debug.Log("Searching for 'AudioManagement' component...");
            GameObject obj = GameObject.Find("AudioManagement");
            if (obj != null)
            {
                audioManager = obj.GetComponent<AudioManagement>();
                if (audioManager == null)
                {
                    Debug.LogError("GameObject 'AudioManagement' was found, but it's missing the 'AudioManagement' component.");
                }
                else
                {
                    Debug.Log("'AudioManagement' assigned successfully!");
                }
            }
            else
            {
                Debug.LogError("Could not find GameObject named 'AudioManagement' in the scene.");
            }
        }

        // --- Tìm ThanhNo ---
        if (thanhno == null)
        {
            Debug.Log("Searching for 'ThanhNo' component...");
            GameObject obj = GameObject.Find("ThanhNo");
            if (obj != null)
            {
                thanhno = obj.GetComponent<ThanhNo>();
                if (thanhno == null)
                {
                    Debug.LogError("GameObject 'ThanhNo' was found, but it's missing the 'ThanhNo' component.");
                }
                else
                {
                    Debug.Log("'ThanhNo' assigned successfully!");
                }
            }
            else
            {
                Debug.LogError("Could not find GameObject named 'ThanhNo' in the scene.");
            }
        }
    }

    void Update()
    {
        if (khoadichuyen==false)
        {
            traiphai = Input.GetAxis("Horizontal");
            lenxuong = Input.GetAxis("Vertical");
        }
        else
        {
            rigidbody2D.linearVelocity = Vector2.zero;
            traiphai = 0; lenxuong = 0;
        }

            rigidbody2D.linearVelocity = new Vector2(traiphai * speed, lenxuong * speed);

        // Hồi thanh nộ
        if (thanhNoHienTai <= thanhNoToiDa * 3)
        {
            thanhNoHienTai += 2f * Time.deltaTime;
            thanhno.capnhatthanhno(thanhNoHienTai, thanhNoToiDa);
        }

        // Xử lý nghiêng tàu khi di chuyển
        float gocMucTieu = -traiphai * gocNghiengToiDa;
        Quaternion gocXoayMucTieu = Quaternion.Euler(0, 0, gocMucTieu);
        transform.rotation = Quaternion.Lerp(transform.rotation, gocXoayMucTieu, tocDoNghieng * Time.deltaTime);
    }

    public void TakeDame(float dame)
    {
        if (kimcangbathoai==true)
        {
            Debug.Log("kim cang bat hoai giam dame:" + dame / 4);
            thanhmauhientai -= dame / 4;
        }
        else
        {
            thanhmauhientai -= dame;
            StartFlashRed();
        }
        thanhmau.capnhatthanhmau(thanhmauhientai, thanhmauToiDa);
        audioManager.PlaySfxto(audioManager.tiengvacham);
        if (thanhmauhientai <= 0)
        {
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

            damehientai += damebonus;
            audioManager.PlaySfxto(audioManager.tiengancoin);
            Destroy(collision.gameObject);
        }

        if (collision.CompareTag("bufftenlua") && sotenlua <= 9)
        {
            sotenlua += 1;
            soTenLuaText.text = sotenlua.ToString();
            audioManager.PlaySfxto(audioManager.tiengancoin);
            Destroy(collision.gameObject);
        }

        if (collision.CompareTag("buffdanpro"))
        {
            Debug.Log("an buff dan pro");
            foreach (GameObject spawner in spawndanpro)
            {
                spawndanpro danproSpawner = spawner.GetComponent<spawndanpro>();
                danproSpawner.ActivateBuff(10f);
            }
            audioManager.PlaySfxto(audioManager.tiengancoin);
            Destroy(collision.gameObject);
        }
    }

    public void StartFlashRed()
    {
        StopCoroutine(FlashRed(timeFlash));
        StartCoroutine(FlashRed(timeFlash));
    }

    private IEnumerator FlashRed(float thoigianduytri)
    {
               
        while (timer <= thoigianduytri)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.2f);
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(0.2f);
            timer += 0.4f;
        }
        timer = 0f;
    }

    void LateUpdate()
    {
        Vector3 currentPosition = transform.position;
        float clampedX = Mathf.Clamp(currentPosition.x, -7.5f, 7.5f);
        float clampedY = Mathf.Clamp(currentPosition.y, -4.5f, 4.5f);
        transform.position = new Vector3(clampedX, clampedY, currentPosition.z);
    }
}

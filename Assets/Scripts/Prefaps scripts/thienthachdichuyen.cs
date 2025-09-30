using Unity.VisualScripting;
using UnityEngine;

// GỢI Ý: Đặt tên class theo chuẩn PascalCase
public class thienthachdichuyen : MonoBehaviour
{
    // GỢI Ý: Đặt tên biến theo chuẩn camelCase
    public float speed = 2f;
    public ThanhMauThienThach thanhMau;
    public float thanhmauHienTai;
    public float thanhmauToiDa ;
    public float dame;
    public ParticleSystem boom; // Hiệu ứng nổ
    public float tilerotdo;
    public GameObject[] buffPrefab;
    private GameObject Audio;
    private bool biphahuykhicombat = false;
    private GameObject player;
    private GameObject taume;

    // SỬA: Biến này không cần thiết, đã xóa "public GameObject thienthach;"
    // SỬA: Biến này cũng không cần thiết, chúng ta sẽ dùng Singleton
    // public GameObject Audio;

    void Start()
    {
        thanhmauHienTai = thanhmauToiDa;
        thanhMau.capnhatthanhmau(thanhmauHienTai, thanhmauToiDa);
        Audio = GameObject.FindWithTag("Audio");
        player = GameObject.FindWithTag("Player");
        taume = GameObject.FindWithTag("TauMe");
    }

    void Update()
    {
        // SỬA: Dùng "transform" trực tiếp, không cần "thienthach.transform"
        transform.Translate(Vector2.down * speed * Time.deltaTime);

        // Nếu thiên thạch bay ra khỏi màn hình
        if (transform.position.y < -6f)
        {
            // SỬA: Dùng "gameObject" để hủy chính nó
            Destroy(gameObject);
        }

        // Nếu hết máu
        if (thanhmauHienTai <= 0)
        {
            // Tạo hiệu ứng nổ tại vị trí hiện tại
            Instantiate(boom, transform.position, Quaternion.identity);
            biphahuykhicombat = true;
            PlayerController playerScript = player.GetComponent<PlayerController>();
            if (playerScript.thanhNoHienTai <= playerScript.thanhNoToiDa * 3)
            {
                playerScript.thanhNoHienTai += 5f;
                playerScript.thanhno.capnhatthanhno(playerScript.thanhNoHienTai, playerScript.thanhNoToiDa);
            }
            // Hủy GameObject thiên thạch
            // Hàm OnDestroy() sẽ được tự động gọi để rơi đồ
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Tạo hiệu ứng nổ
            Instantiate(boom, transform.position, Quaternion.identity);

            PlayerController playerScript = player.GetComponent<PlayerController>();
            if (playerScript.thanhNoHienTai <= playerScript.thanhNoToiDa * 3)
            {
                playerScript.thanhNoHienTai += 10f;
                playerScript.thanhno.capnhatthanhno(playerScript.thanhNoHienTai, playerScript.thanhNoToiDa);
            }

            // Gây sát thương cho người chơi
            PlayerController player1 = collision.gameObject.GetComponent<PlayerController>();
            if (player1 != null)
            {
                player1.TakeDame(dame);                
            }       
            // Hủy thiên thạch
            Destroy(gameObject);
        }      
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {       
        if (collision.gameObject.CompareTag("TauMe"))
        {
            PlayerController playerScript = player.GetComponent<PlayerController>();
            if (playerScript.thanhNoHienTai <= playerScript.thanhNoToiDa * 3)
            {
                playerScript.thanhNoHienTai += 5f;
                playerScript.thanhno.capnhatthanhno(playerScript.thanhNoHienTai, playerScript.thanhNoToiDa);
            }
            Instantiate(boom, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    public void TakeDame(float damageAmount)
    {
        thanhmauHienTai -= damageAmount;
        thanhMau.capnhatthanhmau(thanhmauHienTai, thanhmauToiDa);
    }

    public void Rotdokhibiphahuy()
    {
        float randamevalue = Random.Range(0f, 100f);
        if (randamevalue < tilerotdo)
        {
            int randomindex = Random.Range(0, buffPrefab.Length);
            GameObject randombuff = buffPrefab[randomindex];
            // SỬA: Dùng "transform.position"
            Instantiate(randombuff, transform.position, Quaternion.identity);
        }
    }

    // Hàm này được gọi tự động mỗi khi object bị Destroy()
    private void OnDestroy()
    {
        if (biphahuykhicombat)
        {
            AudioManagement audioManager = Audio.GetComponent<AudioManagement>();
            audioManager.PlaySfxto(audioManager.thienthachno);
            Rotdokhibiphahuy();
            PlayerController playerScript = player.GetComponent<PlayerController>();
            if(playerScript.thanhNoHienTai<=playerScript.thanhNoToiDa)
            {
                playerScript.thanhNoHienTai += 5f;
                playerScript.thanhno.capnhatthanhno(playerScript.thanhNoHienTai, playerScript.thanhNoToiDa);
            }
        }       
    }
}
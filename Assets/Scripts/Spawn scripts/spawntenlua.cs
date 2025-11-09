using UnityEngine;
using TMPro;

public class spawntenlua : MonoBehaviour
{
    public GameObject tenluaprefap;
    public TextMeshProUGUI soTenLuaText;
    public AudioManagement Audio;

    public float projectileSpeed = 15f; // THÊM MỚI: Tốc độ của tên lửa (điều chỉnh trong Inspector)
    private Camera mainCamera; // THÊM MỚI: Để lấy vị trí chuột

    void Start()
    {
        GameObject obj = GameObject.Find("SoTenLua");
        soTenLuaText = obj.GetComponent<TextMeshProUGUI>();
        GameObject objAudio = GameObject.Find("AudioManagement");
        Audio = objAudio.GetComponent<AudioManagement>();

        mainCamera = Camera.main; // THÊM MỚI: Lấy camera chính
    }

    void Update()
    {
        int sotenlua = int.Parse(soTenLuaText.text);

        if (Input.GetKeyDown(KeyCode.E) && sotenlua > 0)
        {
            sotenlua -= 1;
            soTenLuaText.text = sotenlua.ToString();

            // --- BẮT ĐẦU LOGIC BẮN VỀ PHÍA CHUỘT (Copy từ SpawnDanIon) ---

            // 1. Lấy vị trí chuột (Screen Space)
            Vector3 mouseScreenPos = Input.mousePosition;

            // 2. Chuyển vị trí chuột sang World Space
            Vector3 spawnPosition = transform.position;
            Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(new Vector3(
                mouseScreenPos.x,
                mouseScreenPos.y,
                mainCamera.transform.position.z - spawnPosition.z
            ));

            mouseWorldPos.z = spawnPosition.z;

            // 3. Tính toán hướng bắn
            Vector3 direction = (mouseWorldPos - spawnPosition).normalized;

            // 4. Xoay viên đạn
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(0, 0, angle);

            // 5. Phát âm thanh
            AudioManagement audioManagement = Audio.GetComponent<AudioManagement>();
            audioManagement.PlaySfxto(audioManagement.tiengtenlua); // Play the sound effect

            // 6. Tạo viên đạn (với hướng xoay ĐÚNG)
            GameObject projectile = Instantiate(tenluaprefap, spawnPosition, rotation);

            // 7. AddForce cho viên đạn
            Rigidbody2D rb2D = projectile.GetComponent<Rigidbody2D>();
            if (rb2D != null)
            {
                rb2D.AddForce(direction * projectileSpeed, ForceMode2D.Impulse);
            }
            else
            {
                Debug.LogError("Prefab 'tenluaprefap' không có Rigidbody2D!");
            }
            // --- KẾT THÚC LOGIC BẮN VỀ PHÍA CHUỘT ---
        }
    }
}
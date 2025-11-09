using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; // Bắt buộc phải có để dùng 'Action'

public class SpawnMonster : MonoBehaviour
{
    // --- KHAI BÁO EVENT ---
    public event Action<int> OnBossSpawned; // Gửi đi ID (index) của boss
    public event Action<int> OnBossDefeated; // Gửi đi ID (index) của boss
    // -----------------------

    [System.Serializable]
    public struct MonsterPrefap
    {
        public GameObject monster;
        public float timeSpawn;
        public float timeOut;
    }

    [System.Serializable]
    public struct BossEvent
    {
        public GameObject boss;
        public float timeSpawn;
        [HideInInspector] public bool hasSpawned;
    }

    public Transform transform;

    [Header("Monster Spawn Settings")]
    public List<MonsterPrefap> monsterPrefaps = new List<MonsterPrefap>();
    List<MonsterPrefap> monstersToSpawn = new List<MonsterPrefap>();
    public float cooldownSpawnMin = 0.5f;
    public float cooldownSpawnStart = 5f;
    public int soQuaiBanDau = 1;
    public int soQuaiCuoiTran = 20;

    [Header("Boss Spawn Settings")]
    public List<BossEvent> bossesToSpawn = new List<BossEvent>();

    [Tooltip("Kéo đối tượng (Empty) làm vị trí spawn boss cố định vào đây")]
    public Transform bossSpawnPoint;

    [Tooltip("Kéo đối tượng AudioManagement vào đây")]
    public AudioManagement audio;

    [Header("Game Timers")]
    public float gameTimer = 0f;
    public float spawnTimer = 0f;
    public float thoiGianTroChoi = 1800f; // 30 phút

    private int soQuai;
    private float tanSuatQuai;
    private bool isBossActive = false;
    private GameObject currentBossInstance = null;
    private int currentBossIndex = -1;

    void Start()
    {
        // Tự động tìm AudioManagement nếu quên kéo vào
        if (audio == null)
        {
            GameObject audioobj = GameObject.Find("AudioManagement");
            if (audioobj != null)
                audio = audioobj.GetComponent<AudioManagement>();
        }
    }

    void Update()
    {
        // --- LOGIC KIỂM TRA BOSS ---
        if (isBossActive)
        {
            if (currentBossInstance == null) // <-- BOSS CHẾT
            {
                Debug.Log("Boss đã bị tiêu diệt! Tiếp tục spawn quái.");
                isBossActive = false;
                spawnTimer = tanSuatQuai;

                // --- KÍCH HOẠT EVENT BOSS BỊ TIÊU DIỆT ---
                if (currentBossIndex != -1)
                {
                    OnBossDefeated?.Invoke(currentBossIndex); // Gửi tín hiệu
                    currentBossIndex = -1; // Reset
                }

                // --- THAY ĐỔI ÂM THANH KHI BOSS CHẾT ---
                if (audio != null)
                {
                    audio.PlayDefaultMusic(); // Quay lại nhạc cũ
                }
                // ----------------------------------------
            }
            else
            {
                // Boss còn sống, dừng mọi thứ
                return;
            }
        }
        // -------------------------

        // --- LOGIC TRÒ CHƠI CHÍNH ---
        gameTimer += Time.deltaTime;
        spawnTimer -= Time.deltaTime;

        ThemQuaiVaoList();

        if (spawnTimer <= 0f)
        {
            spawnWave();
            spawnTimer = tanSuatQuai;
        }

        LoaiQuaiKhoiList();
        UpdateDokho();
        spawnBoss();
        // -----------------------------
    }

    public void UpdateDokho()
    {
        float progress = Mathf.Clamp01(gameTimer / thoiGianTroChoi);
        tanSuatQuai = Mathf.Lerp(cooldownSpawnStart, cooldownSpawnMin, progress);
        soQuai = Mathf.RoundToInt(Mathf.Lerp(soQuaiBanDau, soQuaiCuoiTran, progress));
    }

    public void ThemQuaiVaoList()
    {
        for (int i = monsterPrefaps.Count - 1; i >= 0; i--)
        {
            if (gameTimer >= monsterPrefaps[i].timeSpawn)
            {
                monstersToSpawn.Add(monsterPrefaps[i]);
                monsterPrefaps.RemoveAt(i);
            }
        }
    }

    public void LoaiQuaiKhoiList()
    {
        for (int i = monstersToSpawn.Count - 1; i >= 0; i--)
        {
            if (gameTimer >= monstersToSpawn[i].timeOut)
            {
                monstersToSpawn.RemoveAt(i);
            }
        }
    }

    // Spawn quái nhỏ (vẫn random)
    public void spawnWave()
    {
        if (monstersToSpawn.Count == 0) return;
        for (int j = 0; j < soQuai; j++)
        {
            int randomIndex = UnityEngine.Random.Range(0, monstersToSpawn.Count);
            SpawnSingleMonster(monstersToSpawn[randomIndex].monster);
        }
    }

    // --- HÀM spawnBoss ĐÃ SỬA ĐỔI ---
    public void spawnBoss()
    {
        if (bossesToSpawn.Count == 0 || isBossActive) return;

        for (int i = 0; i < bossesToSpawn.Count; i++)
        {
            if (gameTimer >= bossesToSpawn[i].timeSpawn && !bossesToSpawn[i].hasSpawned)
            {
                Debug.Log("Đang spawn Boss!");

                // Phát âm thanh
                if (audio != null)
                {
                    audio.PlaySfxto(audio.bossSpawn); // SFX
                    audio.PlayBossMusic(); // Chuyển sang nhạc Boss
                }

                // Spawn boss tại VỊ TRÍ CỐ ĐỊNH
                currentBossInstance = SpawnTheBossAtFixedPoint(bossesToSpawn[i].boss);
                isBossActive = true;
                currentBossIndex = i; // <-- LƯU LẠI INDEX CỦA BOSS VỪA SPAWN

                // --- KÍCH HOẠT EVENT BOSS XUẤT HIỆN ---
                OnBossSpawned?.Invoke(currentBossIndex); // Gửi tín hiệu
                // ------------------------------------

                // Tìm tất cả quái (RatMonster) đang hoạt động và tiêu diệt chúng
                RatMonster[] allMonsters = FindObjectsOfType<RatMonster>();
                foreach (RatMonster monster in allMonsters)
                {
                    monster.DieFromBoss(); // Gọi hàm mới để quái chết không cộng EXP
                }

                var clone = bossesToSpawn[i];
                clone.hasSpawned = true;
                bossesToSpawn[i] = clone;

                break;
            }
        }
    }

    // Hàm spawn boss tại vị trí cố định
    public GameObject SpawnTheBossAtFixedPoint(GameObject bossPrefab)
    {
        if (bossSpawnPoint == null)
        {
            Debug.LogError("Chưa gán Boss Spawn Point! Boss sẽ spawn tại vị trí của script SpawnMonster.");
            return Instantiate(bossPrefab, transform.position, Quaternion.identity);
        }

        return Instantiate(bossPrefab, bossSpawnPoint.position, Quaternion.identity);
    }

    // Hàm spawn quái nhỏ (random)
    public GameObject SpawnSingleMonster(GameObject monsterPrefab)
    {
        Vector2 spawnDirection = UnityEngine.Random.insideUnitCircle.normalized;
        Vector3 spawnPosition = transform.position + new Vector3(spawnDirection.x, spawnDirection.y, 0) * 10f;
        return Instantiate(monsterPrefab, spawnPosition, Quaternion.identity);
    }

    public bool GetBossActiveState()
    {
        return isBossActive;
    }
}
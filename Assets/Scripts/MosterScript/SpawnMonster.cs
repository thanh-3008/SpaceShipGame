using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMonster : MonoBehaviour
{
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
    
    public List<MonsterPrefap> monsterPrefaps = new List<MonsterPrefap>();
    public List<BossEvent> bossesToSpawn = new List<BossEvent>();
    List<MonsterPrefap> monstersToSpawn = new List<MonsterPrefap>();

    public float cooldownSpawnMin =0.5f;
    public float cooldownSpawnStart=5f;

    public int soQuaiBanDau=1;
    public int soQuaiCuoiTran=20;

    public float gameTimer = 0f;
    public float spawnTimer = 0f;
    public float thoiGianTroChoi = 1800f; // 30 phút
    
    private int soQuai ;
    private float tanSuatQuai;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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
    }

    public void UpdateDokho()
    {
        float progress = Mathf.Clamp01(gameTimer / thoiGianTroChoi);
         tanSuatQuai = Mathf.Lerp(cooldownSpawnStart,cooldownSpawnMin, progress); 
         soQuai = Mathf.RoundToInt(Mathf.Lerp(soQuaiBanDau, soQuaiCuoiTran, progress));
    }

    public void ThemQuaiVaoList()
    {      
        for (int i = monsterPrefaps.Count-1; i >= 0; i--)
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
        for(int i = monstersToSpawn.Count - 1; i >= 0; i--)
        {
            if (gameTimer >= monstersToSpawn[i].timeOut)
            {
                monstersToSpawn.RemoveAt(i);
            }
        }
    }

    public void spawnWave()
    {
        if(monstersToSpawn.Count == 0) return;
        for (int j = 0; j < soQuai; j++)
            {
                int randomIndex = Random.Range(0, monstersToSpawn.Count);
                SpawnSingleMonster(monstersToSpawn[randomIndex].monster);
            }         
        
    }

    public void spawnBoss() 
    {
        if(bossesToSpawn.Count == 0) return;
        for (int i = 0; i < bossesToSpawn.Count; i++)
        {
            if (gameTimer >= bossesToSpawn[i].timeSpawn && !bossesToSpawn[i].hasSpawned)
            {
                SpawnSingleMonster(bossesToSpawn[i].boss);
                var clone = bossesToSpawn[i];
                clone.hasSpawned = true;
                bossesToSpawn[i] = clone;
            }
        }
    }

    public void SpawnSingleMonster(GameObject monsterPrefab)
    {
        Vector2 spawnDirection = Random.insideUnitCircle.normalized;
        Vector3 spawnPosition = transform.position + new Vector3(spawnDirection.x, spawnDirection.y, 0) * 10f; // Spawn 5 units away from center

        Instantiate(monsterPrefab, spawnPosition, Quaternion.identity);
    }

}

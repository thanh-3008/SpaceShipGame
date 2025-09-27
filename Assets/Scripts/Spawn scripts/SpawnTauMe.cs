using UnityEngine;

public class SpawnTauMe : MonoBehaviour
{
    public GameObject tauMePrefab;
    public GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Spawn();
        }
    }
    public void Spawn()
    {
        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController.thanhNoHienTai>=playerController.thanhmauToiDa)
        {
            Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y ,0);
            Instantiate(tauMePrefab, spawnPosition, Quaternion.identity);
            playerController.thanhNoHienTai = 0f;
        }
    }
}

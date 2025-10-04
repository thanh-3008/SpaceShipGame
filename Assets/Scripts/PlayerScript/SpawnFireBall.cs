using System.Collections;
using UnityEngine;

public class SpawnFireBall : MonoBehaviour
{
    public GameObject fireBallPrefap;
    private float timespawn =1f;  
    public FireBallController fireBallController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
           
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartSkill()
    {
        StopCoroutine(spawnFireBall());
        StartCoroutine(spawnFireBall());
    }
    public IEnumerator spawnFireBall()
    {
        yield return new WaitForSeconds(0.1f);
        float timer = 0f;
        while (timer <= 5f)
        {
            Vector2 spawnPosition = new Vector2(transform.position.x, transform.position.y);
            Instantiate(fireBallPrefap, spawnPosition, fireBallPrefap.transform.rotation);
            yield return new WaitForSeconds(timespawn);
            timer += 1f;
        }
        fireBallController.isskillturn = false;
        timer = 0f;
    }
}

using UnityEngine;

public class spawndan : MonoBehaviour
{
    public float timespawn ; // Time interval between spawns
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public AudioManagement audioManager;
    public GameObject danprefap;
   
    void Start()
    {
        GameObject obj = GameObject.Find("AudioManagement");
        audioManager = obj.GetComponent<AudioManagement>();
    }

    // Update is called once per frame
    void Update()
    {
        timespawn += Time.deltaTime; // Increment timer by the time elapsed since last frame
        if (timespawn >= 0.2) 
        { 
        Vector2 spawnPosition = new Vector2(transform.position.x, transform.position.y); // Position above the spawner
            audioManager.PlaySfx(audioManager.tiengdan); // Play the sound effect
            Dan dan = danprefap.GetComponent<Dan>();
            Instantiate(dan, spawnPosition, dan.transform.rotation); // Spawn the projectile
            timespawn = 0f;
        }                                                      
    }
}

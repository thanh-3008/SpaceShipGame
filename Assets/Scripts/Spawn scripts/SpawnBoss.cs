using Unity.VisualScripting;
using UnityEngine;

public class SpawnBoss : MonoBehaviour
{
    public GameObject bossPrefap;
    GameObject objspawnthienthach;
    public AudioManagement audio;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject audioobj = GameObject.Find("AudioManagement");
        audio = audioobj.GetComponent<AudioManagement>();
        
    }

    public void spawnBoss()
    {
        audio.PlaySfxto(audio.bossSpawn);
        objspawnthienthach = GameObject.Find("thienthachxuathien");
        objspawnthienthach.SetActive(false);

       GameObject boss = Instantiate(bossPrefap, transform.position, Quaternion.identity);
        
        
    }
}

using UnityEngine;
using System.Collections;
public class SpawnDanTauMe : MonoBehaviour
{
    public GameObject dan;
    private float thoigianspawn;
    private float thoigianbatdauspawn;
    public float thoigiancachnhau ;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {       
        thoigianbatdauspawn = 5f;   
        thoigianspawn = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        thoigianbatdauspawn -= Time.deltaTime;
        thoigiancachnhau += Time.deltaTime;
        if (thoigianbatdauspawn <= 0 && thoigianspawn <=10f && thoigiancachnhau>0.1f)
        {
            AudioManagement audio = FindAnyObjectByType<AudioManagement>();
            audio.PlaySfx(audio.tiengdan);
            Instantiate(dan, transform.position, dan.transform.rotation);           
            thoigiancachnhau = 0f;
        }
        thoigianspawn += Time.deltaTime;

    }
}

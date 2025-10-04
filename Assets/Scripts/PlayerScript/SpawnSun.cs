using UnityEngine;

public class SpawnSun : MonoBehaviour
{
    public GameObject sunprefap;
    public PlayerController playerController;
    public BackGroundSKill skill;
    public AudioManagement audio;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject audioobj = GameObject.Find("AudioManagement");
        audio = audioobj.GetComponent<AudioManagement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && playerController.thanhNoHienTai >= playerController.thanhNoToiDa * 3 )
        {
            skill.StartSkill();
            Vector2 tran = new Vector2(0,-13);
            Instantiate(sunprefap, tran, transform.rotation);
            playerController.thanhNoHienTai = 0f;
        }
    }
}
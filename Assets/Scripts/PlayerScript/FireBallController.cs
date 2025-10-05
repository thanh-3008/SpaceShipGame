
using UnityEngine;

public class FireBallController : MonoBehaviour
{
    public SpawnFireBall[] FireBalls;
    public PlayerController playerController;
    public BackGroundSKill skill;
    public bool isskillturn = false;
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
        if (Input.GetKeyDown(KeyCode.F) && playerController.thanhNoHienTai >= playerController.thanhNoToiDa && isskillturn == false)
        {
            audio.PlaySfxto(audio.amthanhfireball);
            playerController.thanhNoHienTai -= 100f;
            isskillturn = true;
            skill.StartSkill();
            foreach (var f in FireBalls) {
                f.StartSkill();
            }
        }
    }
}

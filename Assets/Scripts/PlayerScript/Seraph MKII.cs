using System.Collections;
using UnityEngine;

public class SeraphMKII : MonoBehaviour
{
    public PlayerController player;
    public GameObject dongbangskill;
    public bool lamchamthoigian = false;
    public DongBangImage dongbang;
    public AudioManagement audioManagement;
    public GameObject laserden;
    public bool isturnonlaser = false;
    public spawndan[] dan;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GetComponent<PlayerController>();
        dongbangskill = GameObject.FindWithTag("DongBang");  
        dongbang = dongbangskill.GetComponent<DongBangImage>();
        GameObject objaudio = GameObject.Find("AudioManagement");
        audioManagement = objaudio.GetComponent<AudioManagement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && (player.thanhNoHienTai >= player.thanhmauToiDa * 3) )
        {
            StartSkillBanNang();
        }
        if(Input.GetKeyDown(KeyCode.F)&& (player.thanhNoHienTai>= player.thanhmauToiDa) && isturnonlaser==false)
        {
            isturnonlaser=true;
            startskillBlackLaser();
        }
    }
    public void StartSkillBanNang()
    {
        StopCoroutine(BanNangSatThu());
        StartCoroutine(BanNangSatThu());
    }
    public IEnumerator BanNangSatThu()
    {
        player.thanhNoHienTai = 0;
        audioManagement.PlaySfxto(audioManagement.amthanhngungdong);
        yield return new WaitForSeconds(2f);
        lamchamthoigian = true;
        dongbang.bathieuung();
        yield return new WaitForSeconds(20f);
        lamchamthoigian = false;
        dongbang.tathieuung();
    }

    public void startskillBlackLaser()
    {
            StopCoroutine(BlackLaser());
        StartCoroutine(BlackLaser());
    }
    public IEnumerator BlackLaser()
    {
        audioManagement.PlaySfxto(audioManagement.amthanhlaserden); 
        player.thanhNoHienTai -= 100f;
        spawndan sdan = dan[0];
        spawndan sdan2 = dan[1];
        sdan.isshot = false;
        sdan2.isshot = false;
        player.khoadichuyen = true;
        laserden.SetActive(true);
        yield return new WaitForSeconds(1.8f);
        sdan.isshot = true;
        sdan2.isshot = true;
        laserden.SetActive(false);
        isturnonlaser = false;
        player.khoadichuyen = false;
    }
}

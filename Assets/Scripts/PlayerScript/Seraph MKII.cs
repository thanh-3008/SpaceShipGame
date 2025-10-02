using System.Collections;
using UnityEngine;

public class SeraphMKII : MonoBehaviour
{
    public PlayerController player;
    public GameObject dongbangskill;
    public bool lamchamthoigian = false;
    public DongBangImage dongbang;
    public AudioManagement audioManagement;
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
}

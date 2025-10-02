using System.Collections;
using UnityEngine;

public class SeraphMKII : MonoBehaviour
{
    public PlayerController player;
    public GameObject dongbangskill;
    public bool lamchamthoigian = false;
    public DongBangImage dongbang;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GetComponent<PlayerController>();
        dongbangskill = GameObject.FindWithTag("DongBang");  
        dongbang = dongbangskill.GetComponent<DongBangImage>();
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
        lamchamthoigian = true;
        dongbang.bathieuung();
        yield return new WaitForSeconds(20f);
        lamchamthoigian = false;
        dongbang.tathieuung();
    }
}

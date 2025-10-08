using System.Collections;
using System.Threading;
using UnityEngine;

public class SeraphMKI : MonoBehaviour
{
    public PlayerController player;
    private float thoigian=10f;
    public SpriteRenderer spriteRenderer;
    public GameObject khienNangLuong;
    public float thoigiankhien=8f;
    public spawndan[] dan;
    public GameObject tialaser;
    public GameObject hapthu;
    public AudioManagement Audio;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GetComponent<PlayerController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        GameObject objAudio = GameObject.Find("AudioManagement");
        Audio = objAudio.GetComponent<AudioManagement>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.F) && player.thanhNoHienTai>=player.thanhNoToiDa)
        {
            StartSkill();
        }
        if(Input.GetKeyUp(KeyCode.Space) && player.thanhNoHienTai >= player.thanhmauToiDa * 3 && khienNangLuong != null )
        {
            StartUlti();
            player.thanhNoHienTai = 0;
        }
        
    }
    public void StartSkill()
    {
        StopCoroutine(KimCangBatHoai(thoigian));
        StartCoroutine(KimCangBatHoai(thoigian));
    }

    public IEnumerator KimCangBatHoai(float thoigianhieuluc)
    {
        player.thanhNoHienTai -= 100f;
        float timer = 0f;
        Audio.PlaySfxto(Audio.amthanhbatkimcangbathoai);
        while (timer<=thoigianhieuluc) 
        {
        player.kimcangbathoai = true;
        spriteRenderer.color = Color.orange;
        yield return new WaitForSeconds(0.5f);
        timer += 0.5f;
        }
        while (timer <= thoigianhieuluc + 2)
        {
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(0.25f);
            spriteRenderer.color = Color.orange;
            yield return new WaitForSeconds(0.25f);
            timer += 0.5f;

        }
        player.kimcangbathoai = false;
        spriteRenderer.color = Color.white;
        timer = 0f;

    }
    public void StartUlti() 
    { 
        StopCoroutine(KhienNangLuongQuatai(8f));
        StartCoroutine(KhienNangLuongQuatai(8f));
    }
    public IEnumerator KhienNangLuongQuatai(float thoigianduytri)
    {
        spawndan sdan = dan[0];
        spawndan sdan2 = dan[1];
        float timer = 0f;
        
        while (timer < thoigianduytri )
        {           
            hapthu.SetActive(true);
            sdan.isshot = false;
            sdan2.isshot = false;
            player.khoadichuyen = true;
            khienNangLuong.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            timer += 0.2f;
            if (timer == 1f)
            {
                Audio.PlaySfxto(Audio.amthanhtuluc);
            }
        }
        hapthu.SetActive(false);      
        tialaser.SetActive(true);
        yield return new WaitForSeconds(4f);
        khienNangLuong.SetActive(false);
        sdan.isshot = true;
        sdan2.isshot= true;
        tialaser.SetActive(false);
        player.khoadichuyen = false;
    }
}

using System.Collections;
using System.Threading;
using UnityEngine;

public class SeraphMKI : MonoBehaviour
{
    public PlayerController player;
    public float thoigian;
    public SpriteRenderer spriteRenderer;
    public GameObject khienNangLuong;
    public float thoigiankhien=8f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GetComponent<PlayerController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.F) && player.thanhNoHienTai>=player.thanhNoToiDa)
        {
            StartSkill();
        }
        if(Input.GetKeyUp(KeyCode.Space) && player.thanhNoHienTai >= player.thanhmauToiDa * 3 && khienNangLuong != null)
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
        while (timer<=thoigianhieuluc) 
        {
        player.kimcangbathoai = true;
        spriteRenderer.color = Color.orange;
        yield return new WaitForSeconds(0.5f);
            timer += 0.5f;
            
        }
        player.kimcangbathoai = false;
        spriteRenderer.color = Color.white;
        timer = 0f;

    }
    public void StartUlti() 
    { 
        StopCoroutine(KhienNangLuongQuatai(thoigiankhien));
        StartCoroutine(KhienNangLuongQuatai(thoigiankhien));
    }
    public IEnumerator KhienNangLuongQuatai(float thoigianduytri)
    {     
        float timer = 0f;
        while (timer < thoigianduytri)
        {
            player.khoadichuyen = true;
            khienNangLuong.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            timer += 0.2f;
        }
        player.khoadichuyen = false;
        khienNangLuong.SetActive(false);
    }
}

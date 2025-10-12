using Marmamusic.EpicAdventureMusic;
using System.Collections;
using UnityEngine;

public class Cochechuyendoi : MonoBehaviour
{
    public GameObject doitau;
    public PlayerController playerController;
    public GameObject chuyenDoi;
    public AudioManagement Audio;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController= GetComponent<PlayerController>();
        GameObject objAudio = GameObject.Find("AudioManagement");
        Audio = objAudio.GetComponent<AudioManagement>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G) && playerController.thanhNoHienTai>=playerController.thanhNoToiDa*3)
        {
            Audio.PlaySfxto(Audio.amthanhbiendoi);
            Debug.Log("bat dau chuyen doi");
            StartAnim();
        }
    }

    public void StartAnim()
    {
        StopCoroutine(ChuyenDoiTau(doitau));
        StartCoroutine(ChuyenDoiTau(doitau));
    }

    public IEnumerator ChuyenDoiTau(GameObject Tauchuyendoi)
    {
        Vector3 vitrihientai = transform.position;
        Quaternion gocnghienhientai = transform.rotation;
        chuyenDoi.SetActive(true);
        ChuyenDoi bienhinh = chuyenDoi.GetComponent<ChuyenDoi>();
        bienhinh.BienDoi();
        yield return new WaitForSeconds(0.5f);

        // BƯỚC 1: Tạo tàu mới và LƯU LẠI tham chiếu vào một biến
        GameObject tauMoi = Instantiate(Tauchuyendoi, vitrihientai, gocnghienhientai);

        // BƯỚC 2: Lấy component PlayerController từ chính con tàu MỚI đó
        PlayerController controllerTauMoi = tauMoi.GetComponent<PlayerController>();

        // BƯỚC 3: Gán giá trị máu và nộ từ tàu cũ sang tàu mới
        if (controllerTauMoi != null && playerController != null)
        {
            controllerTauMoi.thanhmauhientai = playerController.thanhmauhientai;
            controllerTauMoi.thanhNoHienTai = 0f;
            controllerTauMoi.damegoc = playerController.damegoc;
        }

        chuyenDoi.SetActive(false);
        Destroy(gameObject);
    }

}

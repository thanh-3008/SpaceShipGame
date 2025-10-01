using System.Collections;
using UnityEngine;

public class Cochechuyendoi : MonoBehaviour
{
    public GameObject doitau;
    public PlayerController playerController;
    public PlayerController playerControllerTauChuyenDoi;
    public GameObject chuyenDoi;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController= GetComponent<PlayerController>();
        playerControllerTauChuyenDoi = doitau.GetComponent<PlayerController>();
            
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G) && playerController.thanhNoHienTai>=playerController.thanhNoToiDa*3)
        {
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
        Instantiate(Tauchuyendoi, vitrihientai, gocnghienhientai);
        playerControllerTauChuyenDoi.thanhmauhientai = playerController.thanhmauhientai;
        playerControllerTauChuyenDoi.thanhNoHienTai = 0f;
        chuyenDoi.SetActive(false);
        Destroy(gameObject);

    }

}

using Unity.Mathematics;
using UnityEngine;

public class Cochechuyendoi : MonoBehaviour
{
    public GameObject doitau;
    public PlayerController playerController;
    public PlayerController playerControllerTauChuyenDoi;
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
            ChuyenDoiTau(doitau);
        }
    }

    public void ChuyenDoiTau(GameObject Tauchuyendoi)
    {
        Vector3 vitrihientai = transform.position;
        Quaternion gocnghienhientai = transform.rotation;

        Destroy(gameObject);        
        Instantiate(Tauchuyendoi, vitrihientai, gocnghienhientai);
        playerControllerTauChuyenDoi.thanhmauhientai = playerController.thanhmauhientai;
        playerControllerTauChuyenDoi.thanhNoHienTai = 0f;

    }

}

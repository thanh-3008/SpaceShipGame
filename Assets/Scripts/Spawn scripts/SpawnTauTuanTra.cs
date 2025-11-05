using JetBrains.Annotations;
using System.Collections;
using UnityEngine;

public class SpawnTauTuanTra : MonoBehaviour
{

    public Vector3 viTriBatDau;
    public Vector3 viTriKetThuc;
    public GameObject tauTuanTraPrefab;
    public float thoiGianDelay = 8f;
    private Camera mainCamera;
    private float Padding = 2f;
    public float lucban = 10f;
    public float scale = 0.8f;
    public int soTau = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartBatDauTauTuanTra()
    {
               StartCoroutine(BatDauTuanTra());
    }
    public void NangCapTauTuanTra()
    {
        thoiGianDelay -= 1f;
        scale += 0.15f;
    }

    public IEnumerator BatDauTuanTra()
    {
        while (true)
        {
            FlyTauTuanTra();
            yield return new WaitForSeconds(thoiGianDelay);
        }
    }

    public void FlyTauTuanTra()
    {
        for(int i=0; i<soTau; i++)
        {
            Vector3 viTriCameraTraiDuoi = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane));
            Vector3 viTriCameraPhaiTren = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, mainCamera.nearClipPlane));

            int viTri = Random.Range(0, 4);

            switch (viTri)
            {
                case 0://tren->xuong
                    {
                        viTriBatDau = new Vector3(Random.Range(viTriCameraTraiDuoi.x, viTriCameraPhaiTren.x), viTriCameraPhaiTren.y + Padding, 0);
                        viTriKetThuc = new Vector3(Random.Range(viTriCameraTraiDuoi.x, viTriCameraPhaiTren.x), viTriCameraTraiDuoi.y - Padding, 0);
                        break;
                    }
                case 1://xuong->tren
                    {
                        viTriBatDau = new Vector3(Random.Range(viTriCameraTraiDuoi.x, viTriCameraPhaiTren.x), viTriCameraTraiDuoi.y - Padding, 0);
                        viTriKetThuc = new Vector3(Random.Range(viTriCameraTraiDuoi.x, viTriCameraPhaiTren.x), viTriCameraPhaiTren.y + Padding, 0);
                        break;
                    }
                case 2://trai->phai
                    {
                        viTriBatDau = new Vector3(viTriCameraTraiDuoi.x - Padding, Random.Range(viTriCameraTraiDuoi.y, viTriCameraPhaiTren.y), 0);
                        viTriKetThuc = new Vector3(viTriCameraPhaiTren.x + Padding, Random.Range(viTriCameraTraiDuoi.y, viTriCameraPhaiTren.y), 0);
                        break;
                    }
                case 3://phai->trai
                    {
                        viTriBatDau = new Vector3(viTriCameraPhaiTren.x + Padding, Random.Range(viTriCameraTraiDuoi.y, viTriCameraPhaiTren.y), 0);
                        viTriKetThuc = new Vector3(viTriCameraTraiDuoi.x - Padding, Random.Range(viTriCameraTraiDuoi.y, viTriCameraPhaiTren.y), 0);
                        break;
                    }
            }
            Vector3 viTriSpawn = viTriBatDau;
            Vector2 huong = (viTriKetThuc - viTriBatDau).normalized;
            float gocXoayTau = Mathf.Atan2(huong.y, huong.x) * Mathf.Rad2Deg;
            TaoRaTau(viTriSpawn, huong, gocXoayTau);

        }
        

    }
    public void TaoRaTau(Vector3 viTriSpawn, Vector2 huong, float gocxoay)
    {
        GameObject Tau =Instantiate(tauTuanTraPrefab, viTriSpawn, Quaternion.Euler(0, 0, gocxoay+90));
        Tau.transform.localScale = new Vector3(scale, scale, 1f);
        Rigidbody2D rb = Tau.GetComponent<Rigidbody2D>();
        if (rb != null)
        { 
            rb.AddForce(huong * lucban, ForceMode2D.Impulse);
        }

    }

}

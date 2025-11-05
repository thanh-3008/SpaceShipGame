using System.Collections.Generic;
using UnityEngine;

public class SpawnSuriken : MonoBehaviour
{
    [Header("Cài đặt Prefab và Container")]
    public GameObject surikenPrefab;

    public GameObject surikenPro;
    // Kéo đối tượng rỗng 'ShurikenContainer' bạn vừa tạo vào đây
    public Transform shurikenContainer;

    [Header("Thông số Shuriken")]
    public int maxSuriken = 5;
    public float banKinh = 1.5f;
    public float tocDoQuay = 100f;

    // Danh sách để quản lý các shuriken đã tạo ra
    private List<GameObject> surikens = new List<GameObject>();
    private int soSurikenHienTai = 0;   
    void Update()
    {

    }

    void LateUpdate()
    {
        // Nếu có shuriken nào đang hoạt động, chỉ cần quay container
        // Tất cả shuriken con sẽ tự động quay theo một cách mượt mà
        if (soSurikenHienTai > 0)
        {
            shurikenContainer.Rotate(Vector3.forward, tocDoQuay * Time.deltaTime);
        }
    }

    public void ThemSuriken()
    {
        if (soSurikenHienTai < maxSuriken)
        {
            // Tạo shuriken mới VÀ đặt nó làm con của container ngay lập tức
            GameObject newSuriken = Instantiate(surikenPrefab, shurikenContainer.position, Quaternion.identity, shurikenContainer);           
            banKinh += 0.5f; // Tăng bán kính mỗi khi thêm shuriken mới
            soSurikenHienTai++;
            surikens.Add(newSuriken);

            // Sắp xếp lại vị trí của TẤT CẢ shuriken
            SapXepLaiShuriken();
        }
    }

    private void SapXepLaiShuriken()
    {
        float buocNhayGoc = 360f / soSurikenHienTai;

        for (int i = 0; i < surikens.Count; i++)
        {
            float gocHienTai = buocNhayGoc * i;
            float gocRad = gocHienTai * Mathf.Deg2Rad;

            float x = banKinh * Mathf.Cos(gocRad);
            float y = banKinh * Mathf.Sin(gocRad);

            // Bây giờ localPosition sẽ hoạt động đúng vì shuriken là con của container
            // Nó sẽ đặt vị trí của shuriken trong một vòng tròn xung quanh tâm của container (tức là tâm của player)
            surikens[i].transform.localPosition = new Vector2(x, y);
        }
    }

    public void NangCapCuoi()
    {
        if(soSurikenHienTai == maxSuriken)
        {

            for(int i = 0; i < surikens.Count; i++)
            {
                GameObject OldShuriken = surikens[i];
                Vector3 position = OldShuriken.transform.localPosition;
                Quaternion rotation = OldShuriken.transform.localRotation;

                GameObject newShuriken = Instantiate(surikenPro, shurikenContainer);
                newShuriken.transform.localPosition = position;
                newShuriken.transform.localRotation = rotation;

                surikens[i] = newShuriken;
                Destroy(OldShuriken);

            }


            for (int i = 0; i < surikens.Count; i++)
            {
                surikens[i].transform.localScale = new Vector3(1f, 1f, 1f);
            }

           

            tocDoQuay = 500f;
            banKinh = 8f;
        }
        
    }
}
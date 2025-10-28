using UnityEngine;

public class ThuTuHoiThoai : MonoBehaviour
{
    public Conversation HoiThoaiBatDau;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HoiThoaiManagement.instance.StartHoiThoai(HoiThoaiBatDau);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using UnityEngine;
using UnityEngine.UI;

public class ThanhNo : MonoBehaviour
{
    public Image thanhNo;
    public void capnhatthanhno(float thanhnohientai, float thanhnoToiDa)
    {
        thanhNo.fillAmount = thanhnohientai / thanhnoToiDa;
    }
}

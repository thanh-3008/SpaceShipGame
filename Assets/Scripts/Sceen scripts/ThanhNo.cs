using UnityEngine;
using UnityEngine.UI;

public class ThanhNo : MonoBehaviour
{
    public Image thanhNo1;
    public Image thanhNo2;
    public Image thanhNo3;    

    public void capnhatthanhno(float thanhnohientai, float thanhnotoida)
    {
        float fill1 = thanhnohientai/ thanhnotoida;
        thanhNo1.fillAmount = fill1;

        float fill2 = (thanhnohientai - thanhnotoida) / thanhnotoida;
        thanhNo2 .fillAmount = fill2;

        float fill3 = (thanhnohientai - thanhnotoida * 2)/ thanhnotoida;
        thanhNo3 .fillAmount = fill3;
    }
}

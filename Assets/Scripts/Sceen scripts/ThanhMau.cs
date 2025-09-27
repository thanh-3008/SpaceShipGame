using UnityEngine;
using UnityEngine.UI;

public class ThanhMau : MonoBehaviour
{
    public Image healthBar;
    public void capnhatthanhmau(float thanhmauhientai, float thanhmauToiDa)
    {
        healthBar.fillAmount = thanhmauhientai / thanhmauToiDa;
    }
}

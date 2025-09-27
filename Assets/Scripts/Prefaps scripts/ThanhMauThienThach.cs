using UnityEngine;
using UnityEngine.UI;

public class ThanhMauThienThach : MonoBehaviour
{
    public Slider thanhMauSlider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void capnhatthanhmau(float thanhmauhientai, float thanhmauToiDa)
    {
        thanhMauSlider.value = thanhmauhientai / thanhmauToiDa;
    }
}

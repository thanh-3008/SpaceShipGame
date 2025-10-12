using UnityEngine;
using UnityEngine.UI;

public class ThanhXP : MonoBehaviour
{
    public Image fillImage;

    public void SetXP(float currentXP, float maxXP)
    {
        if (maxXP <= 0)
        {
            fillImage.fillAmount = 0;
            return;
        }
        float fillAmount = currentXP / maxXP;
        fillImage.fillAmount = Mathf.Clamp01(fillAmount);
    }
}

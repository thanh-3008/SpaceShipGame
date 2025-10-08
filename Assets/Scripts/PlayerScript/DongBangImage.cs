using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class DongBangImage : MonoBehaviour
{
    public Image hieuung;

    public void bathieuung()
    {
        hieuung.enabled = true;
        
    }
    public void tathieuung()
    {
        hieuung.enabled = false;
    }
}

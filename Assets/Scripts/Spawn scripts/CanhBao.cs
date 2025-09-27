using UnityEngine;
using System.Collections;
using System.Threading;

public class CanhBao : MonoBehaviour
{
    public CanvasGroup panelCanhBao;
    private float bodem = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayerController player = FindAnyObjectByType<PlayerController>();
            if (player.thanhNoHienTai >= player.thanhNoToiDa)
            {
                StartCoroutine(StartCanhBao());
            }
        }
    }
    
    IEnumerator StartCanhBao()
    {

        while (bodem<=3)
        {
            panelCanhBao.alpha = 1;
            yield return new WaitForSeconds(0.25f);
            bodem += 0.25f;
            panelCanhBao.alpha = 0;
            yield return new WaitForSeconds(0.25f);
            bodem += 0.25f;
        }
        panelCanhBao.alpha = 0;
        bodem = 0f;
    }
}

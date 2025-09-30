using Unity.VisualScripting;
using UnityEngine;

public class Huythienthachkhixuathien : MonoBehaviour
{
    public thienthachdichuyen thienthachdichuyens;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        GameObject[] obj = GameObject.FindGameObjectsWithTag("Enemy");
        
        foreach (GameObject thienthach in obj)
        {
            thienthachdichuyens = thienthach.GetComponent<thienthachdichuyen>();
            thienthachdichuyens.TakeDame(9999f);
        }
    }
   
}

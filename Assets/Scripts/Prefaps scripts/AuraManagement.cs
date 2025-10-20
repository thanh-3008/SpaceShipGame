using UnityEngine;

public class AuraManagement : MonoBehaviour
{
    public GameObject aura;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            KichHoatAura();
        }
    }
    public void KichHoatAura()
    {
      aura.SetActive(true);
    }
}

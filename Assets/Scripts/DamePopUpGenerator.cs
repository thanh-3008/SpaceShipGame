using TMPro;
using UnityEngine;

public class DamePopUpGenerator : MonoBehaviour
{
    public static DamePopUpGenerator Instance;

    public GameObject PopUpPrefab;
    public void Awake()
    {
        Instance = this;
    }
    public void Update()
    {     
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void CreatePopUp(Vector3 posision, string text) 
    {
        var popUp=Instantiate(PopUpPrefab, posision, Quaternion.identity);
        var temp = popUp.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        temp.text = text;

        Destroy(popUp,1f );
    }
}

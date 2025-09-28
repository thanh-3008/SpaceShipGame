using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChonTau : MonoBehaviour
{
    public ThongTinTau[] ThongTinTau;

    public Image HinhAnhTau;

    public TextMeshProUGUI NameTau;

    public TextMeshProUGUI DescriptionTau;

    public Image HinhAnhSkill1;

    public Image HinhAnhSkill2;

    public TextMeshProUGUI MoTaSkill1;

    public TextMeshProUGUI MoTaSkill2;

    public TextMeshProUGUI NameSkill1;

    public TextMeshProUGUI NameSkill2;

    private int currentShipIndex;

    [Header("Panels")]
    public GameObject selectionPanel; // Panel chứa các nút chọn tàu
    public GameObject upgradePanel; // panel nâng cấp


    void Start()
    {
        selectionPanel.SetActive(true);
        upgradePanel.SetActive(false);
        currentShipIndex = 0;
        HienThiChonTau();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextShip() 
    { 
        currentShipIndex++;
        if (currentShipIndex >= ThongTinTau.Length)
        {
            currentShipIndex = 0;
        }
        HienThiChonTau();
    }
    public void PreviousShip() 
    {
        currentShipIndex--;
        if (currentShipIndex < 0)
        {
            currentShipIndex = ThongTinTau.Length - 1;
        }
        HienThiChonTau();
    }

    public void HienThiChonTau()
    {
        ThongTinTau tauhientai = ThongTinTau[currentShipIndex];
        HinhAnhTau.sprite = tauhientai.Sprite;
        NameTau.text = tauhientai.Name;
        MoTaSkill1.text = tauhientai.DescriptionSkill1;
        MoTaSkill2.text = tauhientai.DescriptionSkill2;
        HinhAnhSkill1.sprite = tauhientai.spiteskill1;
        HinhAnhSkill2.sprite = tauhientai.spiteskill2;
        DescriptionTau.text = tauhientai.Description;
        NameSkill1.text = tauhientai.NameSkill1;
        NameSkill2.text = tauhientai.NameSkill2;
    }

    public void BackMenu()
    {
        SceneManager.LoadScene(0);
    }


}

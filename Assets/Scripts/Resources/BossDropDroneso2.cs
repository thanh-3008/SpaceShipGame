using UnityEngine;

public class BossDropDroneso2 : MonoBehaviour
{
    public GameObject droneItemPrefab;

    [Header("Cài đặt Hộp thoại Boss")]
    public GameObject bossDialogUIPrefab;

    [TextArea(3, 5)]
    public string bossDefeatQuote = "Ta... đã thua sao?!";

    void Start()
    {
    }

    void Update()
    {
    }

    public void OnBossDefeatedSO2()
    {
        Instantiate(droneItemPrefab, transform.position, Quaternion.identity);
        ShowBossDefeatDialog();
    }

    private void ShowBossDefeatDialog()
    {
        Debug.Log($"Boss nói: {bossDefeatQuote}");
    }
}
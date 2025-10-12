using UnityEngine;

public class BossDropDroneso2 : MonoBehaviour
{
    public GameObject droneItemPrefab;

    public void OnBossDefeatedSO2()
    {
        Instantiate(droneItemPrefab, transform.position, Quaternion.identity);
    }
}

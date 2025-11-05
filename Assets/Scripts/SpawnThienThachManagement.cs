using UnityEngine;

public class SpawnThienThachManagement : MonoBehaviour
{
    public GameObject thienThachPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        thienThachPrefab.SetActive(false);
    }

    public void BatSkill()
    {
        thienThachPrefab.SetActive(true);
    }
}

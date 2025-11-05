using UnityEngine;

public class SupportShipManager : MonoBehaviour
{
    public static SupportShipManager Instance;
    public GameObject[] supportShipPrefabs;

    private void Awake() => Instance = this;

    public void CallSupportShip(int index)
    {
        if (index < 0 || index >= supportShipPrefabs.Length) return;
        Instantiate(supportShipPrefabs[index], new Vector3(-15, 0, 0), Quaternion.identity);
    }
}

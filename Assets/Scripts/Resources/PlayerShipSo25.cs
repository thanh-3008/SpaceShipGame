using UnityEngine;

public class PlayerShipSo25 : MonoBehaviour
{
    public ShipPartSo25 leftWing1;
    public ShipPartSo25 rightWing1;
    public ShipPartSo25 booster1;
    public ShipPartSo25 sideGun1;

    public float moveSpeed = 5f;
    public float fireRate = 0.2f;

    void Start()
    {
        leftWing1.OnDestroyed += () => moveSpeed -= 1f;
        rightWing1.OnDestroyed += () => moveSpeed -= 1f;
        booster1.OnDestroyed += () => moveSpeed -= 1.5f;
        sideGun1.OnDestroyed += () => fireRate += 0.1f; // tăng fireRate = bắn chậm hơn
    }
}

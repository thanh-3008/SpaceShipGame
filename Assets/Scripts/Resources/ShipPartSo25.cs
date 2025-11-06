using UnityEngine;

public class ShipPartSo25 : MonoBehaviour
{
    public int maxHP11 = 40;
    public int currentHP1;
    public bool destroyed1 = false;

    public System.Action OnDestroyed; // gọi cho PlayerShip khi vỡ

    void Start()
    {
        currentHP1 = maxHP11;
    }

    public void TakeDamage(int dmg)
    {
        if (destroyed1) return;

        currentHP1 -= dmg;

        if (currentHP1 <= 0)
        {
            destroyed1 = true;
            BreakPart();
        }
    }

    void BreakPart()
    {
        // Tắt chức năng
        gameObject.SetActive(false);

        // Gọi callback cho PlayerShip
        OnDestroyed?.Invoke();

        // Spawn hiệu ứng mảnh vỡ
        // TODO: Instantiate debris prefab
    }
}

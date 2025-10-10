using UnityEngine;

public class LaserBeamAttack25 : MonoBehaviour
{
    private int damage = 30;
    private float duration = 2f;

    void Start()
    {
        Destroy(gameObject, duration);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player hit by laser beam for " + damage + " damage.");
        }
    }
}
